using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTicketingSystem.CSharp.Domain.Models.Features.UserEvent
{
    public class UserEventListResponseModel: PaginationModel
    {
        public List<UserEventResponseModel> EventList { get; set; }
        public List<UserEventResponseModel> Top3Events { get; set; }
     }

    public class UserEventResponseModel
    {
        public string? Eventcode { get; set; }
        public string? Eventname { get; set; }
        public string? Address { get; set; }
        public string? Venueimage { get; set; }

        public static UserEventResponseModel FromTblEvent(TblEvent tblEvent)
        {
            return new UserEventResponseModel
            {
                Eventcode = tblEvent.Eventcode,
                Eventname = tblEvent.Eventname
            };
        }
    }
}
