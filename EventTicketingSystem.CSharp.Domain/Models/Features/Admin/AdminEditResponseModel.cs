namespace EventTicketingSystem.CSharp.Domain.Models.Features.Admin;

public class AdminEditResponseModel
{
    public AdminEditModel? Admin { get; set; }
}

public class AdminEditModel
{
    public string? AdminCode { get; set; }

    public string? Username { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNo { get; set; }

    public string? ProfileImage { get; set; }

    public static AdminEditModel FromTblAdmin(TblAdmin admin)
    {
        return new AdminEditModel
        {
            AdminCode = admin.Admincode,
            Username = admin.Username,
            Email = admin.Email,
            PhoneNo = admin.Phone,
            FullName = admin.Fullname,
            ProfileImage = admin.Profileimage
        };
    }
}