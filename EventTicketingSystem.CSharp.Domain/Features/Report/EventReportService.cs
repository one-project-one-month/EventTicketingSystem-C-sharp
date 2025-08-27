using EventTicketingSystem.CSharp.Domain.Models.Features.Report;

namespace EventTicketingSystem.CSharp.Domain.Features.Report
{
    public class EventReportService : ReportServiceBase
    {
        private readonly AppDbContext _db;
        public EventReportService(AppDbContext db)
        {
            _db = db;
        }
        public override string ReportName => "EventReport";

        public override Dictionary<string, object> GetDataSources(object requestData)
        {
            var request = (EventReportRequest)requestData;

            var eventsQuery = _db.TblEvents
                .Where(e => e.Deleteflag == false)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.eventCode))
                eventsQuery = eventsQuery.Where(e => e.Eventcode == request.eventCode);

            var eventList = eventsQuery.ToList();

            var eventDataList = new List<EventDataSet>();

            foreach (var ev in eventList)
            {
                var category = _db.TblEventcategories
                    .FirstOrDefault(c => c.Eventcategorycode == ev.Eventcategorycode && !c.Deleteflag);

                var owner = _db.TblBusinessowners
                    .FirstOrDefault(b => b.Businessownercode == ev.Businessownercode && !b.Deleteflag);

                var venue = _db.TblVenues
                    .FirstOrDefault(v => v.Venuecode == ev.Venuecode && !v.Deleteflag);

                var ticketTypes = _db.TblTickettypes
                    .Where(tt => tt.Eventcode == ev.Eventcode && !tt.Deleteflag)
                    .ToList();

                var ticketTypeDataSetList = new List<TicketTypeDataSet>();

                foreach (var tt in ticketTypes)
                {
                    var ticketPrice = _db.TblTicketprices
                        .FirstOrDefault(tp => tp.Tickettypecode == tt.Tickettypecode && !tp.Deleteflag);

                    var soldTicketCount = 0;
                    if (ticketPrice != null)
                    {
                        soldTicketCount = _db.TblTickets
                            .Count(t => t.Ticketpricecode == ticketPrice.Ticketpricecode
                                        && t.Soldout
                                        && !t.Deleteflag);
                    }

                    ticketTypeDataSetList.Add(new TicketTypeDataSet
                    {
                        Tickettypename = tt.Tickettypename,
                        Ticketprice = ticketPrice.Ticketprice,
                        Ticketquantity = ticketPrice.Ticketquantity,
                        SoldTicket = soldTicketCount,
                        Eventcode = ev.Eventcode,
                    });
                }

                eventDataList.Add(new EventDataSet
                {
                    Eventcode = ev.Eventcode,
                    Venuename = venue?.Venuename ?? string.Empty,
                    Categoryname = category?.Categoryname ?? string.Empty,
                    Startdate = ev.Startdate,
                    Enddate = ev.Enddate,
                    Eventstatus = ev.Eventstatus,
                    Fullname = owner?.Fullname ?? string.Empty,
                    Totalticketquantity = ev.Totalticketquantity,
                    Soldoutcount = ev.Soldoutcount,
                    Uniquename = ev.Uniquename,
                    Eventname = ev.Eventname,
                    Email = owner?.Email ?? string.Empty,
                    Phone = owner?.Phone ?? string.Empty,
                    Address = venue?.Address ?? string.Empty,
                    Capacity = venue?.Capacity ?? 0,
                    TicketTypes = ticketTypeDataSetList
                });
            }

            return new Dictionary<string, object>
            {
                { "EventDataSet", eventDataList },
                { "TicketTypeDataSet", eventDataList.SelectMany(e => e.TicketTypes).ToList() }
            };

            //var data = (EventDataSet)requestData;

            //return new Dictionary<string, object>
            //{
            //    { "TblEvent", new List<EventDataSet> { data } },

            //    { "TblTickettype", data.TicketTypes ?? new List<TicketTypeDataSet>() }
            //};
        }

        public override Dictionary<string, object>? GetParameters(object requestData)
        {
            var req = requestData as EventReportRequest;
            if (req == null) return null;

            return new Dictionary<string, object>
            {
                { "eventCode", req.eventCode }
            };
        }
    }
}
