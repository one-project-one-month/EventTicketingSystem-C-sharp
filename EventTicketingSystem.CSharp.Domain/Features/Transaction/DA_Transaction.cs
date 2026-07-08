namespace EventTicketingSystem.CSharp.Domain.Features.Transaction;

public class DA_Transaction
{
    private readonly AppDbContext _db;
    private readonly CommonService _commonService;
    private readonly ILogger<DA_Transaction> _logger;

    public DA_Transaction(AppDbContext db, CommonService commonService, ILogger<DA_Transaction> logger)
    {
        _db = db;
        _commonService = commonService;
        _logger = logger;
    }

    public async Task<Result<ProcessTransactionResponseModel>> ProcessTransaction(ProcessTransactionRequestModel requestModel)
    {
        if (requestModel.EventCode.IsNullOrEmpty() ||
            requestModel.Email.IsNullOrEmpty() ||
            requestModel.TicketTypeCode.IsNullOrEmpty() ||
            requestModel.TicketQuantity <= 0)
        {
            return Result<ProcessTransactionResponseModel>.ValidationError("Invalid transaction request.");
        }

        try
        {
            var ticketType = await _db.TblTickettypes
                .FirstOrDefaultAsync(x => x.Tickettypecode == requestModel.TicketTypeCode &&
                                          x.Eventcode == requestModel.EventCode &&
                                          x.Deleteflag == false);

            if (ticketType is null)
            {
                return Result<ProcessTransactionResponseModel>.NotFoundError("Ticket type not found.");
            }

            var ticketPrice = await _db.TblTicketprices
                .FirstOrDefaultAsync(x => x.Tickettypecode == ticketType.Tickettypecode && x.Deleteflag == false);

            if (ticketPrice is null)
            {
                return Result<ProcessTransactionResponseModel>.NotFoundError("Ticket price not found.");
            }

            var tickets = await _db.TblTickets
                .Where(x => x.Ticketpricecode == ticketPrice.Ticketpricecode &&
                            x.Deleteflag == false &&
                            x.Isused == false)
                .OrderBy(x => x.Ticketcode)
                .Take(requestModel.TicketQuantity)
                .ToListAsync();

            if (tickets.Count < requestModel.TicketQuantity)
            {
                return Result<ProcessTransactionResponseModel>.ValidationError("Not enough tickets available.");
            }

            var transactionCode = await _commonService.GenerateSequenceCode(EnumTableUniqueName.Tbl_Transaction);
            var now = DateTime.Now;

            var transaction = new TblTransaction
            {
                Transactionid = GenerateUlid(),
                Transactioncode = transactionCode,
                Email = requestModel.Email,
                Eventcode = requestModel.EventCode,
                Status = "success",
                Paymenttype = "Online",
                Transactiondate = now,
                Totalamount = ticketPrice.Ticketprice * requestModel.TicketQuantity,
                Createdby = "SYSTEM",
                Createdat = now,
                Deleteflag = false
            };

            await _db.TblTransactions.AddAsync(transaction);

            foreach (var ticket in tickets)
            {
                ticket.Isused = true;
                ticket.Modifiedby = "SYSTEM";
                ticket.Modifiedat = now;
                _db.Entry(ticket).State = EntityState.Modified;

                await _db.TblTransactiontickets.AddAsync(new TblTransactionticket
                {
                    Transactionticketid = GenerateUlid(),
                    Transactionticketcode = await _commonService.GenerateSequenceCode(EnumTableUniqueName.Tbl_TransactionTicket),
                    Transactioncode = transactionCode,
                    Ticketcode = ticket.Ticketcode,
                    Qrimage = ticket.Ticketcode,
                    Price = ticketPrice.Ticketprice,
                    Createdby = "SYSTEM",
                    Createdat = now,
                    Deleteflag = false
                });
            }

            var evt = await _db.TblEvents.FirstOrDefaultAsync(x => x.Eventcode == requestModel.EventCode && x.Deleteflag == false);
            if (evt is not null)
            {
                evt.Soldoutcount += requestModel.TicketQuantity;
                _db.Entry(evt).State = EntityState.Modified;
            }

            await _db.SaveAndDetachAsync();

            return Result<ProcessTransactionResponseModel>.Success(new ProcessTransactionResponseModel
            {
                Response = requestModel
            }, "Transaction completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<ProcessTransactionResponseModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<TransactionHistoryListResponseModel>> GetTransactionHistoryList()
    {
        try
        {
            var rows = await (
                from transaction in _db.TblTransactions
                join evt in _db.TblEvents on transaction.Eventcode equals evt.Eventcode
                where transaction.Deleteflag == false
                orderby transaction.Transactiondate descending
                select new
                {
                    transaction.Transactioncode,
                    transaction.Email,
                    transaction.Transactiondate,
                    evt.Eventname
                }).ToListAsync();

            var list = new List<TransactionHistoryModel>();
            foreach (var row in rows)
            {
                list.Add(new TransactionHistoryModel
                {
                    TransactionCode = row.Transactioncode,
                    Email = row.Email,
                    TransactionDate = row.Transactiondate,
                    EventName = row.Eventname,
                    TicketTypeName = await GetTicketTypeName(row.Transactioncode)
                });
            }

            return Result<TransactionHistoryListResponseModel>.Success(new TransactionHistoryListResponseModel
            {
                TransactionList = list
            });
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<TransactionHistoryListResponseModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<TransactionDetailResponseModel>> GetTransactionHistoryDetail(string transactionCode)
    {
        try
        {
            var transaction = await _db.TblTransactions
                .FirstOrDefaultAsync(x => x.Transactioncode == transactionCode && x.Deleteflag == false);

            if (transaction is null)
            {
                return Result<TransactionDetailResponseModel>.NotFoundError("Transaction not found.");
            }

            var detail = await (
                from transactionTicket in _db.TblTransactiontickets
                join ticket in _db.TblTickets on transactionTicket.Ticketcode equals ticket.Ticketcode
                join price in _db.TblTicketprices on ticket.Ticketpricecode equals price.Ticketpricecode
                join type in _db.TblTickettypes on price.Tickettypecode equals type.Tickettypecode
                join evt in _db.TblEvents on transaction.Eventcode equals evt.Eventcode
                where transactionTicket.Transactioncode == transactionCode && transactionTicket.Deleteflag == false
                group new { transactionTicket, price, type, evt } by new
                {
                    transaction.Email,
                    evt.Eventname,
                    evt.Eventcode,
                    evt.Eventstatus,
                    type.Tickettypename,
                    price.Ticketprice,
                    transaction.Paymenttype,
                    transaction.Transactiondate,
                    evt.Isactive
                } into g
                select new TransactionDetailModel
                {
                    Email = g.Key.Email,
                    EventName = g.Key.Eventname,
                    EventCode = g.Key.Eventcode,
                    EventStatus = g.Key.Eventstatus,
                    TicketTypeName = g.Key.Tickettypename,
                    TicketPrice = g.Key.Ticketprice,
                    PaymentType = g.Key.Paymenttype,
                    TransactionDate = g.Key.Transactiondate,
                    IsActive = g.Key.Isactive,
                    Qty = g.Count()
                }).FirstOrDefaultAsync();

            if (detail is null)
            {
                return Result<TransactionDetailResponseModel>.NotFoundError("Transaction detail not found.");
            }

            return Result<TransactionDetailResponseModel>.Success(new TransactionDetailResponseModel
            {
                TransactionDetail = detail
            });
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<TransactionDetailResponseModel>.SystemError(ex.Message);
        }
    }

    private async Task<string> GetTicketTypeName(string transactionCode)
    {
        return await (
            from transactionTicket in _db.TblTransactiontickets
            join ticket in _db.TblTickets on transactionTicket.Ticketcode equals ticket.Ticketcode
            join price in _db.TblTicketprices on ticket.Ticketpricecode equals price.Ticketpricecode
            join type in _db.TblTickettypes on price.Tickettypecode equals type.Tickettypecode
            where transactionTicket.Transactioncode == transactionCode && transactionTicket.Deleteflag == false
            select type.Tickettypename).FirstOrDefaultAsync() ?? string.Empty;
    }
}
