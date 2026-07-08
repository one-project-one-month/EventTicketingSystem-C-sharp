namespace EventTicketingSystem.CSharp.Domain.Features.TicketType;

public class DA_TicketType : AuthorizationService
{
    private readonly AppDbContext _db;
    private readonly ILogger _logger;
    private readonly CommonService _commonService;

    public DA_TicketType(IHttpContextAccessor httpContextAccessor,
                         AppDbContext db,
                         ILogger<DA_TicketType> logger,
                         CommonService commonService) : base(httpContextAccessor)
    {
        _db = db;
        _logger = logger;
        _commonService = commonService;
    }

    public async Task<Result<TicketTypeListResponseModel>> List()
    {
        var model = new TicketTypeListResponseModel();
        var responseModel = new Result<TicketTypeListResponseModel>();

        model.TicketTypeList = await (
            from tt in _db.TblTickettypes
            join tp in _db.TblTicketprices on tt.Tickettypecode equals tp.Tickettypecode into prices
            from tp in prices.DefaultIfEmpty()
            join te in _db.TblEvents on tt.Eventcode equals te.Eventcode into events
            from te in events.DefaultIfEmpty()
            where tt.Deleteflag == false
            select new TicketTypeListModel
            {
                TicketTypeId = tt.Tickettypeid,
                TicketTypeCode = tt.Tickettypecode,
                EventCode = tt.Eventcode,
                EventName = te == null ? string.Empty : te.Eventname,
                TicketTypeName = tt.Tickettypename,
                Ticketprice = tp == null ? 0 : tp.Ticketprice,
                TicketQuantity = tp == null ? 0 : tp.Ticketquantity,
                CreatedBy = tt.Createdby,
                CreatedAt = tt.Createdat,
                ModifiedBy = tt.Modifiedby,
                ModifiedAt = tt.Modifiedat
            }).ToListAsync();

        responseModel = Result<TicketTypeListResponseModel>.Success(model, "Ticket types are retrieved successfullly!");
        return responseModel;
    }

    public async Task<Result<TicketTypeEditResponseModel>> Edit(string code)
    {
        var model = new TicketTypeEditResponseModel();        

        try
        {
            var tickettype = await (
                from tt in _db.TblTickettypes
                join tp in _db.TblTicketprices on tt.Tickettypecode equals tp.Tickettypecode into prices
                from tp in prices.DefaultIfEmpty()
                join te in _db.TblEvents on tt.Eventcode equals te.Eventcode into events
                from te in events.DefaultIfEmpty()
                where tt.Deleteflag == false && tt.Tickettypecode == code
                select new TicketTypeEditModel
                {
                    TicketTypeId = tt.Tickettypeid,
                    TicketTypeCode = tt.Tickettypecode,
                    EventCode = tt.Eventcode,
                    EventName = te == null ? string.Empty : te.Eventname,
                    TicketTypeName = tt.Tickettypename,
                    Ticketprice = tp == null ? 0 : tp.Ticketprice,
                    TicketQuantity = tp == null ? 0 : tp.Ticketquantity,
                    CreatedBy = tt.Createdby,
                    CreatedAt = tt.Createdat,
                    ModifiedBy = tt.Modifiedby,
                    ModifiedAt = tt.Modifiedat
                }).FirstOrDefaultAsync();

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


            var ticketList = new List<TblTicket>();
            for (int i = 0; i < requestModel.TicketQuantity; i++)
            {
                var ticket = new TblTicket
                {
                    Ticketid = GenerateUlid(),
                    Ticketcode = await _commonService.GenerateSequenceCode(EnumTableUniqueName.Tbl_Ticket),
                    Isused = false,
                    Ticketpricecode = ticketprice.Ticketpricecode,
                    Createdat = DateTime.Now,
                    Createdby = CurrentUserId,
                    Deleteflag = false
                };
                ticketList.Add(ticket);
            }

            await _db.TblTickets.AddRangeAsync(ticketList);
            await _db.SaveAndDetachAsync();

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
