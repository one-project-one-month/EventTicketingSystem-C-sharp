namespace EventTicketingSystem.CSharp.Domain.Models.Features.TicketType;

public class TicketTypeExportRequestModel
{
    public string Format { get; set; }
    public List<TicketTypeListModel> TicketTypeList { get; set; }
}