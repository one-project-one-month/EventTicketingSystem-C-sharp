namespace EventTicketingSystem.CSharp.Domain.Models.Features.EventCategory;

public class EventCategoryListResponseModel
{
    public List<EventCategoryListModel>? EventCategories { get; set; }
}

public class EventCategoryListModel
{
    public string? EventCategoryid { get; set; }

    public string? EventCategorycode { get; set; }

    public string? Categoryname { get; set; }

    public string? Createdby { get; set; }

    public string Createdat { get; set; }

    public static EventCategoryListModel FromTblCategory(TblEventcategory category)
    {
        return new EventCategoryListModel
        {
           EventCategoryid = category.Eventcategoryid,
           EventCategorycode = category.Eventcategorycode,
           Categoryname = category.Categoryname,
           Createdby = category.Createdby,
           Createdat = category.Createdat.ToDateTimeFormat()
        };
    }
}