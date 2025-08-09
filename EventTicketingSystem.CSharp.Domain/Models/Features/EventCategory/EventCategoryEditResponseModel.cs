namespace EventTicketingSystem.CSharp.Domain.Models.Features.EventCategory;

public class EventCategoryEditResponseModel
{
    public EventCategoryEditModel? Event { get; set; }
}

public class EventCategoryEditModel
{
    public string? EventCategoryid { get; set; }

    public string? EventCategorycode { get; set; }

    public string? Categoryname { get; set; }

    public string? Createdby { get; set; }

    public string Createdat { get; set; }


    public static EventCategoryEditModel FromTblCategory(TblEventcategory category)
    {
        return new EventCategoryEditModel
        {
            EventCategoryid = category.Eventcategoryid,
            EventCategorycode = category.Eventcategorycode,
            Categoryname = category.Categoryname,
            Createdby = category.Createdby,
            Createdat = category.Createdat.ToDateTimeFormat()
        };
    }
}
