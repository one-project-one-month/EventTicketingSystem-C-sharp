namespace EventTicketingSystem.CSharp.Api.Controllers;

public class TicketTypeExportRequestModel
{
    public string Format { get; set; }
    public List<TicketTypeListModel> TicketTypeList { get; set; }
}