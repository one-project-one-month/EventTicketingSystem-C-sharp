namespace EventTicketingSystem.CSharp.Domain.Models.Features.Report
{
    public class EventReportDataModel
    {
       
    }
    public class EventDataSet
    {
        public string Eventcode { get; set; }
        public string Venuename { get; set; }
        public string Categoryname { get; set; }
        public DateTime Startdate { get; set; }
        public DateTime Enddate { get; set; }
        public string Eventstatus { get; set; }
        public string Fullname { get; set; }
        public int Totalticketquantity { get; set; }
        public int Soldoutcount { get; set; }
        public string Uniquename { get; set; }
        public string Eventname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int Capacity { get; set; }
        public ICollection<TicketTypeDataSet> TicketTypes { get; set; }
    }
    public class TicketTypeDataSet
    {
        public string Tickettypename { get; set; }
        public decimal Ticketprice { get; set; }
        public int Ticketquantity { get; set; }
        public int SoldTicket {  get; set; }
        public string Eventcode { get; set; }
    }
}
