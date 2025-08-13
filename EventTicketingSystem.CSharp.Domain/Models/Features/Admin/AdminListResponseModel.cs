namespace EventTicketingSystem.CSharp.Domain.Models.Features.Admin;

public class AdminListResponseModel
{
    public List<AdminListModel>? AdminList { get; set; }
}

public class AdminListModel
{
    public string? AdminCode { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNo { get; set; }

    public static AdminListModel FromTblAdmin(TblAdmin admin)
    {
        return new AdminListModel
        {
            AdminCode = admin.Admincode,
            FullName = admin.Fullname,
            Email = admin.Email,
            PhoneNo = admin.Phone
        };
    }
}