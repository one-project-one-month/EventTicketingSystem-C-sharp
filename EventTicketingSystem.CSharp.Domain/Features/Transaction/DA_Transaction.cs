namespace EventTicketingSystem.CSharp.Domain.Features.Transaction;

public class DA_Transaction : AuthorizationService
{
    private readonly ILogger<DA_Transaction> _logger;
    private readonly AppDbContext _db;
    private readonly CommonService _commonService;
    private readonly BL_QrCode _blQrCode;
    private readonly EmailService _emailService;

    public DA_Transaction(IHttpContextAccessor httpContextAccessor,
                          ILogger<DA_Transaction> logger,
                          AppDbContext db,
                          CommonService commonService,
                          BL_QrCode blQrCode,
                          EmailService emailService) : base(httpContextAccessor)
    {
        _logger = logger;
        _db = db;
        _commonService = commonService;
        _blQrCode = blQrCode;
        _emailService = emailService;
    }

    public async Task<Result<EventTicketTypeListResponseModel>> GetTicketTypeList(string eventCode)
    {
        var model = new EventTicketTypeListResponseModel();
        try
        {
            if (eventCode.IsNullOrEmpty())
            {
                return Result<EventTicketTypeListResponseModel>.ValidationError("Event ID cannot be null or empty.");
            }

            var result = await (
                            from type in _db.TblTickettypes
                            join price in _db.TblTicketprices
                                on type.Tickettypecode equals price.Tickettypecode
                            where type.Eventcode == eventCode
                                && type.Deleteflag == false
                                && price.Deleteflag == false
                            select new
                            {
                                TicketTypeCode = type.Tickettypecode,
                                TicketTypeName = type.Tickettypename,
                                TicketPrice = price.Ticketprice
                            }
                        ).ToListAsync();

            model.TicketTypes = result.Select(x => new TicketTypeData
            {
                TicketTypeCode = x.TicketTypeCode,
                TicketTypeName = x.TicketTypeName,
                TicketPrice = x.TicketPrice
            }).ToList();

            return Result<EventTicketTypeListResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<EventTicketTypeListResponseModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<TransactionResponseModel>> ProcessTransaction(TransactionRequestModel requestModel)
    {
        try
        {
            #region Validation

            var requestValidation = ValidateTransactionRequest(requestModel);
            if (requestValidation != null)
            {
                return requestValidation;
            }

            #endregion

            #region Event Validation

            var eventInfo = await _db.TblEvents
                        .FirstOrDefaultAsync(
                            x => x.Eventcode == requestModel.EventCode &&
                            (x.Eventstatus == EnumEventStatus.Upcoming.ToString() ||
                            x.Eventstatus == EnumEventStatus.Ongoing.ToString()) &&
                            x.Isactive == true &&
                            x.Deleteflag == false
                        );
            if (eventInfo is null)
            {
                return Result<TransactionResponseModel>.NotFoundError("No Event Found.");
            }

            #endregion

            #region Ticket Validation

            var ticketPriceCode = await _db.TblTicketprices
                                    .Where(
                                        x => x.Tickettypecode == requestModel.TicketTypeCode &&
                                        x.Deleteflag == false)
                                    .Select(x => x.Ticketpricecode)
                                    .FirstOrDefaultAsync();
            if (ticketPriceCode is null)
            {
                return Result<TransactionResponseModel>.NotFoundError("No Ticket Type Found.");
            }

            var availableTicketsCount = await _db.TblTickets
                                        .CountAsync(
                                            x => x.Ticketpricecode == ticketPriceCode &&
                                            x.Soldout == false &&
                                            x.Deleteflag == false);

            if (availableTicketsCount < requestModel.TicketQuantity)
            {
                return Result<TransactionResponseModel>.ValidationError
                    ($"Tickets Out of Stocks. {availableTicketsCount} tickets left for this ticket type.");
            }

            #endregion

            using var dbTransaction = await _db.Database.BeginTransactionAsync();

            #region Get Ticket Price

            decimal ticketPrice = await _db.TblTicketprices
                                          .Where(
                                                x => x.Tickettypecode == requestModel.TicketTypeCode &&
                                                x.Deleteflag == false)
                                          .Select(x => x.Ticketprice)
                                          .FirstOrDefaultAsync();
            if (ticketPrice <= 0)
            {
                return Result<TransactionResponseModel>.ValidationError("Invalid ticket price.");
            }

            var totalAmount = requestModel.TicketQuantity * ticketPrice;

            #endregion

            #region Get Available Tickets

            var availableTickets = await _db.TblTickets
                                    .FromSqlRaw(@"
                                        SELECT * FROM ""tbl_ticket""
                                        WHERE ""soldout"" = false AND ""deleteflag"" = false
                                        AND ""ticketpricecode"" = {0}
                                        ORDER BY ""ticketcode""
                                        LIMIT {1}
                                        FOR UPDATE SKIP LOCKED", ticketPriceCode, requestModel.TicketQuantity)
                                    .ToListAsync();

            if (availableTickets.Count < requestModel.TicketQuantity)
            {
                return Result<TransactionResponseModel>.ValidationError("Not enough available tickets.");
            }

            #endregion

            #region Update Soldout Status

            foreach (var ticket in availableTickets)
            {
                ticket.Soldout = true;
                ticket.Modifiedby = CurrentUserId;
                ticket.Modifiedat = DateTime.Now;
                _db.Entry(ticket).State = EntityState.Modified;
            }
            await _db.SaveAndDetachAsync();

            var eventDetail = await _db.TblEvents
                                    .FromSqlRaw(@"
                                        SELECT * FROM ""tbl_event""
                                        WHERE ""eventcode"" = {0} 
                                          AND ""deleteflag"" = false 
                                          AND ""isactive"" = true
                                          AND (""eventstatus"" = 'Ongoing' OR ""eventstatus"" = 'UpComing')
                                        FOR UPDATE SKIP LOCKED
                                        LIMIT 1", requestModel.EventCode)
                                    .FirstOrDefaultAsync();
            if (eventDetail is null)
            {
                return Result<TransactionResponseModel>.NotFoundError("No Event Found.");
            }

            eventDetail.Soldoutcount += requestModel.TicketQuantity;
            _db.Entry(eventDetail).State = EntityState.Modified;
            await _db.SaveAndDetachAsync();

            #endregion

            #region Insert into Tbl_Transaction

            var transaction = new TblTransaction
            {
                Transactionid = GenerateUlid(),
                Transactioncode = await _commonService.GenerateTransactionSequenceCode(EnumTableUniqueName.Tbl_Transaction),
                Fullname = requestModel.FullName,
                Phone = requestModel.Phone,
                Email = requestModel.Email,
                Eventcode = requestModel.EventCode,
                Gender = requestModel.Gender,
                Status = EnumTransactionStatus.Success.ToString(),
                Paymenttype = "KBZ Pay",
                Transactiondate = DateTime.Now,
                Totalamount = totalAmount,
                Createdby = CurrentUserId,
                Createdat = DateTime.Now,
                Deleteflag = false
            };

            await _db.TblTransactions.AddAsync(transaction);
            await _db.SaveAndDetachAsync();

            #endregion

            #region Generate Transaction Tickets Codes

            var sequenceTransactionCodes = await _commonService.GenerateTransactionSequenceCode(
                eventInfo.Uniquename,
                eventInfo.Eventcode,
                requestModel.TicketQuantity
            );
            if (sequenceTransactionCodes.Count != requestModel.TicketQuantity)
            {
                return Result<TransactionResponseModel>.ValidationError("Mismatch between requested quantity and generated transaction codes.");
            }

            #endregion

            await dbTransaction.CommitAsync();

            #region Generate QR Image and FilePath

            var qrStringList = new List<string>();
            var qrFilePathList = new List<string>();
            foreach (var ticket in availableTickets)
            {
                var qrGeneratedData = await _blQrCode.Generate(new QrGenerateRequestModel
                {
                    TicketCode = ticket.Ticketcode,
                    FullName = requestModel.FullName,
                    Email = requestModel.Email
                });
                qrStringList.Add(qrGeneratedData.Data.QrString);
                qrFilePathList.Add(qrGeneratedData.Data.FilePath);
            }

            #endregion

            #region Insert into Tbl_TransactionTicket

            DateTime now = DateTime.Now;

            var transactionTickets = sequenceTransactionCodes
                    .Select((code, index) => new TblTransactionticket
                    {
                        Transactionticketid = GenerateUlid(),
                        Transactionticketcode = code,
                        Transactioncode = transaction.Transactioncode,
                        Ticketcode = availableTickets[index].Ticketcode,
                        Qrimage = qrFilePathList[index],
                        Price = ticketPrice,
                        Createdby = CurrentUserId,
                        Createdat = now,
                        Deleteflag = false
                    })
                    .ToList();

            await _db.BulkInsertAsync(transactionTickets);

            #endregion

            #region Send QR Image via Email for Successful Purchased

            var body = EmailBodyTemplates.TicketPurchaseSuccess
                        .Replace("(@eventName)", eventInfo.Eventname)
                        .Replace("(@ticketQuantity)", requestModel.TicketQuantity.ToString())
                        .Replace("(@totalAmount)", totalAmount.ToString("F2"))
                        .Replace("(@transactionCode)", transaction.Transactioncode);

            var actualFilePathList = new List<string>();
            foreach (var filePath in qrFilePathList)
            {
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), EnumDirectory.QrImage.ToString());
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                var fileName = Path.Combine(uploadPath, filePath);
                actualFilePathList.Add(fileName);
            }

            var emailTemplate = new AttachmentEmailModel()
            {
                Email = requestModel.Email!,
                Subject = EmailSubjectTemplates.TransactionSuccessful,
                Body = body,
                RecipientName = requestModel.FullName,
                AttachmentPaths = actualFilePathList
            };

            var result = await _emailService.SendAttachmentEmail(emailTemplate);
            if (result is false)
            {
                return Result<TransactionResponseModel>.SystemError("Failed to send verification code via email.");
            }

            #endregion

            return Result<TransactionResponseModel>.Success("Transaction Successful. Please check your Email.");
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<TransactionResponseModel>.SystemError(ex.Message);
        }
    }

    private Result<TransactionResponseModel> ValidateTransactionRequest(TransactionRequestModel requestModel)
    {
        if (requestModel is null)
        {
            return Result<TransactionResponseModel>.ValidationError("Please fill out the fields.");
        }

        if (string.IsNullOrWhiteSpace(requestModel.EventCode))
        {
            return Result<TransactionResponseModel>.ValidationError("Event Code is required.");
        }

        if (string.IsNullOrWhiteSpace(requestModel.FullName))
        {
            return Result<TransactionResponseModel>.ValidationError("Full Name is required.");
        }

        if (string.IsNullOrWhiteSpace(requestModel.Phone))
        {
            return Result<TransactionResponseModel>.ValidationError("Phone number is required.");
        }

        if (string.IsNullOrWhiteSpace(requestModel.Email))
        {
            return Result<TransactionResponseModel>.ValidationError("Email is required.");
        }

        if (string.IsNullOrWhiteSpace(requestModel.Gender))
        {
            return Result<TransactionResponseModel>.ValidationError("Gender is required.");
        }

        if (string.IsNullOrWhiteSpace(requestModel.TicketTypeCode))
        {
            return Result<TransactionResponseModel>.ValidationError("Ticket Type Code is required.");
        }

        if (requestModel.TicketQuantity <= 0)
        {
            return Result<TransactionResponseModel>.ValidationError("Ticket Quantity must be greater than 0.");
        }

        return null!;
    }
}