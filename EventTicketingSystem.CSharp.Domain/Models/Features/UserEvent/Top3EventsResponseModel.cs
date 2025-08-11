using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTicketingSystem.CSharp.Domain.Models.Features.UserEvent;

public class Top3EventsResponseModel
{
    public List<TopEventResponseModel> Top3Events { get; set; }
}

public class TopEventResponseModel
{
    public string? Eventcode { get; set; }
    public string? Eventname { get; set; }
    public string? Address { get; set; }

    public static TopEventResponseModel FromTblEvent (TblEvent tblEvent)
    {
        return new TopEventResponseModel
        {
            Eventcode = tblEvent.Eventcode,
            Eventname = tblEvent.Eventname
        };
    }
}
