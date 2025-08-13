namespace EventTicketingSystem.CSharp.Domain.Features.TicketType;

public class DA_TicketType : AuthorizationService
{
    private readonly AppDbContext _db;
    private readonly ILogger _logger;
    private readonly CommonService _commonService;
    private readonly string _connection;

    public DA_TicketType(IHttpContextAccessor httpContextAccessor,
                         AppDbContext db,
                         ILogger<DA_TicketType> logger,
                         IConfiguration configuration,
                         CommonService commonService) : base(httpContextAccessor)
    {
        _db = db;
        _logger = logger;
        _connection = configuration.GetConnectionString("DbConnection")!;
        _commonService = commonService;
    }

    public async Task<Result<TicketTypeListResponseModel>> List()
    {
        var model = new TicketTypeListResponseModel();

        string query = $@"SELECT 
                            tt.tickettypecode,
                            tt.eventcode,
                            te.eventname,
                            tt.tickettypename,
                            tt.createdby,
                            tt.createdat,
                            tt.modifiedby,
                            tt.modifiedat,
                            tt.deleteflag,
                            tt.tickettypeid,
                            tp.ticketprice,
                            tp.ticketquantity
                        FROM tbl_tickettype tt
                        LEFT JOIN tbl_ticketprice tp ON tt.tickettypecode = tp.tickettypecode
                        LEFT JOIN tbl_event te ON te.eventcode = tt.eventcode
                        WHERE tt.deleteflag = false
                        ORDER BY tt.tickettypeid DESC";

        try
        {
            using var dbConnection = new NpgsqlConnection(_connection);
            await dbConnection.OpenAsync();

            var ticketTypeList = (await dbConnection.QueryAsync<TicketTypeListModel>(query)).ToList();
            model.TicketTypeList = ticketTypeList;

            var countQuery = @"SELECT COUNT(*) FROM tbl_tickettype WHERE deleteflag = false";
            var totalCount = await dbConnection.ExecuteScalarAsync<int>(countQuery);

            return Result<TicketTypeListResponseModel>.Success(model, "Ticket types are retrieved successfully!");
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<TicketTypeListResponseModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<TicketTypeEditResponseModel>> Edit(string code)
    {
        var model = new TicketTypeEditResponseModel();

        try
        {
            string query = @"
                SELECT 
                    tt.tickettypecode,
                    tt.eventcode,
                    te.eventname,
                    tt.tickettypename,
                    tt.createdby,
                    tt.createdat,
                    tt.modifiedby,
                    tt.modifiedat,
                    tt.deleteflag,
                    tt.tickettypeid,
                    tp.ticketprice,
                    tp.ticketquantity
               FROM tbl_tickettype tt
               LEFT JOIN tbl_ticketprice tp ON tt.tickettypecode = tp.tickettypecode
               LEFT JOIN tbl_event te ON te.eventcode = tt.eventcode
               WHERE tt.deleteflag = false
                  AND tt.tickettypecode = @TicketTypeCode";

            using IDbConnection dbConnection = new NpgsqlConnection(_connection);
            dbConnection.Open();

            var tickettype = (await dbConnection.QueryAsync<TicketTypeEditModel>(query, new
            {
                TicketTypeCode = code
            })).FirstOrDefault();

            model.TicketType = tickettype;

            return Result<TicketTypeEditResponseModel>.Success(model, "Ticket is retrieved successfully!");
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<TicketTypeEditResponseModel>.SystemError(ex.ToString());
        }
    }

    public async Task<Result<TicketTypeCreateResponseModel>> Create(TicketTypeCreateRequestModel requestModel)
    {
        if (requestModel.EventCode.IsNullOrEmpty())
        {
            return Result<TicketTypeCreateResponseModel>.ValidationError("Event Name cannot be Empty!");
        }
        if (requestModel.TicketTypeName.IsNullOrEmpty())
        {
            return Result<TicketTypeCreateResponseModel>.ValidationError("Ticket Type name cannot be Empty!");
        }
        if (requestModel.Ticketprice <= 0)
        {
            return Result<TicketTypeCreateResponseModel>.ValidationError("Ticket Type price must be greater than 0!");
        }
        if (requestModel.TicketQuantity <= 0)
        {
            return Result<TicketTypeCreateResponseModel>.ValidationError("Ticket Type quantity must be greater than 0!");
        }

        int totalQty = await _db.TblEvents
            .Where(x => x.Eventcode == requestModel.EventCode && !x.Deleteflag)
            .Select(x => x.Totalticketquantity)
            .FirstOrDefaultAsync();

        int existingQty = await _db.TblTickettypes
            .Where(tt => tt.Eventcode == requestModel.EventCode && !tt.Deleteflag)
            .Join(_db.TblTicketprices.Where(tp => !tp.Deleteflag),
                tt => tt.Tickettypecode,
                tp => tp.Tickettypecode,
                (tt, tp) => tp.Ticketquantity)
            .SumAsync();

        int remainingQty = totalQty - existingQty;
        if ((remainingQty - requestModel.TicketQuantity) < 0)
        {
            return Result<TicketTypeCreateResponseModel>.ValidationError("The total number of tickets exceeds the allowed quantity for this event.");
        }

        try
        {
            var TblTickettype = new TblTickettype
            {
                Tickettypeid = GenerateUlid(),
                Tickettypecode = await _commonService.GenerateSequenceCode(EnumTableUniqueName.Tbl_TicketType),
                Tickettypename = requestModel.TicketTypeName,
                Eventcode = requestModel.EventCode,
                Createdat = DateTime.Now,
                Createdby = CurrentUserId,
                Deleteflag = false,
            };

            await _db.TblTickettypes.AddAsync(TblTickettype);
            await _db.SaveAndDetachAsync();

            var ticketprice = new TblTicketprice
            {
                Ticketpriceid = GenerateUlid(),
                Ticketpricecode = await _commonService.GenerateSequenceCode(EnumTableUniqueName.Tbl_TicketPrice),
                Ticketprice = requestModel.Ticketprice,
                Ticketquantity = requestModel.TicketQuantity,
                Tickettypecode = TblTickettype.Tickettypecode,
                Createdat = DateTime.Now,
                Createdby = CurrentUserId,
                Deleteflag = false,
            };

            await _db.TblTicketprices.AddAsync(ticketprice);
            await _db.SaveAndDetachAsync();


            //for (int i = 0; i < requestModel.TicketQuantity; i++)
            //{
            //    var ticket = new TblTicket
            //    {
            //        Ticketid = GenerateUlid(),
            //        Ticketcode = await _commonService.GenerateTicketSequence(eventInfo!.Uniquename, eventInfo.Eventcode),
            //        Isused = false,
            //        Ticketpricecode = ticketprice.Ticketpricecode,
            //        Createdat = DateTime.Now,
            //        Createdby = CurrentUserId,
            //        Deleteflag = false
            //    };
            //    ticketList.Add(ticket);
            //}

            var eventInfo = await _db.TblEvents
                        .FirstOrDefaultAsync(
                            x => x.Eventcode == requestModel.EventCode &&
                            x.Deleteflag == false
                        );
            if (eventInfo is null)
            {
                return Result<TicketTypeCreateResponseModel>.NotFoundError("No Event Found.");
            }

            var codes = await _commonService.GenerateTicketSequenceCode(eventInfo.Uniquename, eventInfo.Eventcode, requestModel.TicketQuantity);

            var now = DateTime.Now;

            var ticketList = codes.Select(code => new TblTicket
            {
                Ticketid = GenerateUlid(),
                Ticketcode = code,
                Isused = false,
                Soldout = false,
                Ticketpricecode = ticketprice.Ticketpricecode,
                Createdat = now,
                Createdby = CurrentUserId,
                Deleteflag = false
            }).ToList();

            await _db.BulkInsertAsync(ticketList);

            //await _db.TblTickets.AddRangeAsync(ticketList);
            //await _db.SaveAndDetachAsync();

            return Result<TicketTypeCreateResponseModel>.Success("Ticket Type and tickets are created successfully!");
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<TicketTypeCreateResponseModel>.SystemError(ex.ToString());
        }
    }

    public async Task<Result<TicketTypeUpdateResponseModel>> Update(TicketTypeUpdateRequestModel requestModel)
    {
        if (requestModel.TicketTypeCode.IsNullOrEmpty())
        {
            return Result<TicketTypeUpdateResponseModel>.ValidationError("There is no ticket type code to update!");
        }

        if (requestModel.TicketTypeName.IsNullOrEmpty())
        {
            return Result<TicketTypeUpdateResponseModel>.ValidationError("Please enter the Ticket type name to update.");
        }

        var tickettype = await _db.TblTickettypes
            .Where(x => !x.Deleteflag)
            .FirstOrDefaultAsync(x => x.Tickettypecode == requestModel.TicketTypeCode);

        if (tickettype is null)
        {
            return Result<TicketTypeUpdateResponseModel>.NotFoundError("There is no ticket type with this code!");
        }

        try
        {
            tickettype.Tickettypename = requestModel.TicketTypeName;
            tickettype.Modifiedat = DateTime.Now;
            tickettype.Modifiedby = CurrentUserId;
            _db.Entry(tickettype).State = EntityState.Modified;
            await _db.SaveAndDetachAsync();

            return Result<TicketTypeUpdateResponseModel>.Success("Ticket type is updated successfully!");
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<TicketTypeUpdateResponseModel>.SystemError(ex.ToString());
        }
    }

    public async Task<Result<TicketTypeDeleteResponseModel>> Delete(string code)
    {
        if (code.IsNullOrEmpty())
        {
            return Result<TicketTypeDeleteResponseModel>.UserInputError("There is no ticket type code to delete!");
        }

        var tickettype = await _db.TblTickettypes
        .Where(x => !x.Deleteflag)
            .FirstOrDefaultAsync(x => x.Tickettypecode == code);

        if (tickettype == null)
        {
            return Result<TicketTypeDeleteResponseModel>.NotFoundError("There is no ticket type with this code!");
        }

        var ticketprice = await _db.TblTicketprices
        .Where(x => !x.Deleteflag)
            .FirstOrDefaultAsync(x => x.Tickettypecode == code);

        var tickets = await _db.TblTickets
        .Where(x => !x.Deleteflag && x.Ticketpricecode == ticketprice!.Ticketpricecode)
            .ToListAsync();

        try
        {
            tickettype.Deleteflag = true;
            tickettype.Modifiedat = DateTime.Now;
            tickettype.Modifiedby = CurrentUserId;
            _db.Entry(tickettype).State = EntityState.Modified;

            if (ticketprice != null)
            {
                ticketprice.Deleteflag = true;
                ticketprice.Modifiedat = DateTime.Now;
                ticketprice.Modifiedby = CurrentUserId;
                _db.Entry(ticketprice).State = EntityState.Modified;

                if (tickets.Count > 0)
                {
                    foreach (var ticket in tickets)
                    {
                        ticket.Deleteflag = true;
                        ticket.Modifiedat = DateTime.Now;
                        ticket.Modifiedby = CurrentUserId;
                        _db.Entry(ticket).State = EntityState.Modified;
                    }
                }
            }

            await _db.SaveAndDetachAsync();

            return Result<TicketTypeDeleteResponseModel>.Success("Ticket type is deleted successfully!");
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<TicketTypeDeleteResponseModel>.SystemError(ex.ToString());
        }
    }
}
