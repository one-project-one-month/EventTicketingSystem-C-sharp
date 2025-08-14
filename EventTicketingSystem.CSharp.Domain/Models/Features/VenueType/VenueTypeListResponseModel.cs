namespace EventTicketingSystem.CSharp.Domain.Models.Features.VenueType;

public class VenueTypeListResponseModel
{
    public List<VenueTypeListModel> VenueTypeList { get; set; }
}
public class VenueTypeListModel
{
    public string VenueTypeCode { get; set; }

    public string VenueTypename { get; set; }

    public string CreatedAt { get; set; }

    public static VenueTypeListModel FromTblVenueType(TblVenuetype tblVenuetype)
    {
        return new VenueTypeListModel
        {
            VenueTypeCode = tblVenuetype.Venuetypecode,
            VenueTypename = tblVenuetype.Venuetypename,
            CreatedAt = tblVenuetype.Createdat.ToDateTimeFormat()
        };
    }
}