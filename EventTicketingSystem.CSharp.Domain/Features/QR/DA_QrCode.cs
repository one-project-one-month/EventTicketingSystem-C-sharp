namespace EventTicketingSystem.CSharp.Domain.Features.QR;

public class DA_QrCode
{
    private readonly ILogger<DA_QrCode> _logger;
    private readonly AppDbContext _db;

    public DA_QrCode(ILogger<DA_QrCode> logger, AppDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<Result<QrGenerateResponseModel>> GenerateQr(QrGenerateRequestModel requestModel)
    {
        var response = new QrGenerateResponseModel();

        var ticketInfo = await (
            from ticket in _db.TblTickets
            join price in _db.TblTicketprices on ticket.Ticketpricecode equals price.Ticketpricecode
            join type in _db.TblTickettypes on price.Tickettypecode equals type.Tickettypecode
            join evt in _db.TblEvents on type.Eventcode equals evt.Eventcode
            join venue in _db.TblVenues on evt.Venuecode equals venue.Venuecode
            where ticket.Ticketcode == requestModel.TicketCode
                  && ticket.Deleteflag == false
                  && price.Deleteflag == false
                  && type.Deleteflag == false
                  && evt.Deleteflag == false
                  && venue.Deleteflag == false
            select new QrGenerateModel
            {
                TicketPriceCode = price.Ticketpricecode,
                TicketPrice = price.Ticketprice,
                TicketTypeCode = type.Tickettypecode,
                TicketTypeName = type.Tickettypename,
                EventCode = evt.Eventcode,
                StartDate = evt.Startdate,
                EndDate = evt.Enddate,
                EventName = evt.Eventname,
                VenueName = venue.Venuename,
                Address = venue.Address
            }).FirstOrDefaultAsync();

        if (ticketInfo == null)
        {
            _logger.LogError("Ticket information not found for the provided ticket code.");
            return Result<QrGenerateResponseModel>.SystemError("Ticket information not found for the provided ticket code.");
        }

        string qrString = $"{ticketInfo.EventName}" +
            $"|{ticketInfo.EventCode}" +
            $"|{DateOnly.FromDateTime((DateTime)ticketInfo.StartDate)}" +
            $"|{ticketInfo.StartDate}" +
            $"|{ticketInfo.EndDate}" +
            $"|GateOpenTime" +
            $"|{requestModel.TicketCode}" +
            $"|{ticketInfo.TicketPrice}" +
            $"|{ticketInfo.TicketTypeName}" +
            $"|{requestModel.FullName}" +
            $"|{requestModel.Email}" +
            $"|{ticketInfo.VenueName}" +
            $"|{ticketInfo.Address}";

        response.QrString = qrString;

        return Result<QrGenerateResponseModel>.Success(response, "QR code generated successfully.");

    }

    public Result<QrCheckResponseModel> CheckQr(string qrString)
    {
        var response = new QrCheckResponseModel();

        if (string.IsNullOrEmpty(qrString))
        {
            _logger.LogError("QR string cannot be null or empty.");
            return Result<QrCheckResponseModel>.SystemError("QR string cannot be null or empty.");
        }

        var qrParts = qrString.Split('|');
        if (qrParts.Length < 11)
        {
            _logger.LogError("Invalid QR string format.");
            return Result<QrCheckResponseModel>.SystemError("Invalid QR string format.");
        }

        response.QrString = qrString;
        response.EventName = qrParts[0];
        response.EventCode = qrParts[1];
        response.EventDate = qrParts[2];
        response.EventTimeFrom = qrParts[3];
        response.EventTimeTo = qrParts[4];
        response.GateOpenTime = qrParts[5];
        response.TicketCode = qrParts[6];
        response.TicketPrice = qrParts[7];
        response.TicketType = qrParts[8];
        response.FullName = qrParts[9];
        response.Email = qrParts[10];
        response.Location = qrParts.Length > 11 ? qrParts[11] : string.Empty;
        response.Address = qrParts.Length > 12 ? qrParts[12] : string.Empty;

        return Result<QrCheckResponseModel>.Success(response, "QR code is valid.");
    }
}
