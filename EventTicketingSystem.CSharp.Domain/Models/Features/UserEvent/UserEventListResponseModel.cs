using EventTicketingSystem.CSharp.Database.AppDbContext;
using System;
namespace EventTicketingSystem.CSharp.Domain.Models.Features.UserEvent;

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

    public List<string>? Venueimage { get; set; }

    public static UserEventResponseModel FromTblEvent(TblEvent tblEvent, TblVenue tblVenue)
    {
        var eventModel = new UserEventResponseModel
        {
            Eventcode = tblEvent.Eventcode,
            Eventname = tblEvent.Eventname,
            Venueimage = new List<string>(),
            Address = tblVenue.Address
        };

        if (!tblVenue.Venueimage.IsNullOrEmpty())
        {
            eventModel.Venueimage = tblVenue.Venueimage
                .Split([','], StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();
        }

        return eventModel;
    }
}