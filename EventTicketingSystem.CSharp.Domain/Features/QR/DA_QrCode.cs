namespace EventTicketingSystem.CSharp.Domain.Features.QR;

public class DA_QrCode
{
    private readonly ILogger<DA_QrCode> _logger;
    private readonly DapperService _dapper;

    public DA_QrCode(ILogger<DA_QrCode> logger, DapperService dapper)
    {
        _logger = logger;
        _dapper = dapper;
    }

    public async Task<Result<QrGenerateResponseModel>> GenerateQr(QrGenerateRequestModel requestModel)
    {
        var response = new QrGenerateResponseModel();

        var param = new { p_ticketcode = requestModel.TicketCode };
        var ticketInfo = await _dapper.QueryStoredProcedureFirstOrDefault<QrGenerateModel>(Queries.sp_ticket_info, param);

        if (ticketInfo == null)
        {
            _logger.LogError("Ticket information not found for the provided ticket code.");
            return Result<QrGenerateResponseModel>.SystemError("Ticket information not found for the provided ticket code.");
        }

        string qrString = $"{ticketInfo.EventCode}" +
            $"|{ticketInfo.EventName}" +
            $"|{DateOnly.FromDateTime((DateTime)ticketInfo.StartDate)}" +
            $"|{ticketInfo.StartDate}" +
            $"|{ticketInfo.EndDate}" +
            $"|GateOpenTime" +
            $"|{requestModel.TicketCode}" +
            $"|{ticketInfo.TicketPrice}" +
            $"|{ticketInfo.TicketTypeName}" +
            $"|{requestModel.FullName}" +
            $"|{requestModel.Email}" +
            $"|{ticketInfo.VenueName}"+
            $"|{ticketInfo.Address}";

        response.QrString = qrString;

        return Result<QrGenerateResponseModel>.Success(response, "QR code generated successfully.");

    }

    public async Task<Result<QrCheckResponseModel>> CheckQr(string qrString)
    {
        var response = new QrCheckResponseModel();

        if (string.IsNullOrEmpty(qrString))
        {
            _logger.LogError("QR string cannot be null or empty.");
            return Result<QrCheckResponseModel>.SystemError("QR string cannot be null or empty.");
        }

        var qrParts = qrString.Split('|');
        if (qrParts.Length < 10)
        {
            _logger.LogError("Invalid QR string format.");
            return Result<QrCheckResponseModel>.SystemError("Invalid QR string format.");
        }


        response.EventName = qrParts[0];
        response.EventCode = qrParts[1];
        response.EventDate = qrParts[2];
        response.EventTimeFrom = qrParts[3];
        var gateOpenTime = response.EventTimeFrom.ToDateTime();
        var getDateTime = gateOpenTime - TimeSpan.FromMinutes(30);
        response.EventTimeTo = qrParts[4];
        response.GateOpenTime = getDateTime?.ToString("hh:mm tt")!;
        response.TicketCode = qrParts[6];
        response.TicketPrice = qrParts[7];
        response.TicketType = qrParts[8];
        response.FullName = qrParts[9];
        response.Email = qrParts[10];
        response.Location = qrParts.Length > 11 ? qrParts[11] : string.Empty;
        response.Address = qrParts.Length > 12 ? qrParts[12] : string.Empty;

        return Result<QrCheckResponseModel>.Success(response);
    }
}