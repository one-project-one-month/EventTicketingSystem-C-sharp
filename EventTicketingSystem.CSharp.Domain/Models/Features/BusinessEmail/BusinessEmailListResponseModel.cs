namespace EventTicketingSystem.CSharp.Domain.Models.Features.BusinessEmail;

public class BusinessEmailListResponseModel
{
    public List<BusinessEmailListModel>? BusinessEmailList { get; set; } = new List<BusinessEmailListModel>();
}

public class BusinessEmailListModel
{
    public string BusinessEmailId { get; set; }

    public string BusinessEmailCode { get; set; }

    public string FullName { get; set; }

    public string Phone { get; set; }

    public string Email { get; set; }

    public string? Createdby { get; set; }

    public DateTime? Createdat { get; set; }


    public static BusinessEmailListModel FromTblBusinessEmail(TblBusinessemail businessEmail)
    {
        return new BusinessEmailListModel()
        {
            BusinessEmailId = businessEmail.Businessemailid,
            BusinessEmailCode = businessEmail.Businessemailcode,
            FullName = businessEmail.Fullname,
            Phone = businessEmail.Phone,
            Email = businessEmail.Email,
            Createdby = businessEmail.Createdby,
            Createdat = businessEmail.Createdat
        };
    }
}