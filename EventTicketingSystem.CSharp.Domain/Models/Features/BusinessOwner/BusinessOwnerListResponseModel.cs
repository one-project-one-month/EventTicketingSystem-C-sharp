namespace EventTicketingSystem.CSharp.Domain.Models.Features.BusinessOwner;

public class BusinessOwnerListResponseModel
{
    public List<BusinessOwnerListModel>? BusinessOwners { get; set; }
}

public class BusinessOwnerListModel
{
    public string? Businessownercode { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public static BusinessOwnerListModel FromTblOwner(TblBusinessowner owner)
    {
        return new BusinessOwnerListModel
        {
            Businessownercode = owner.Businessownercode,
            FullName = owner.Fullname,
            Email = owner.Email,
            Phone = owner.Phone
        };
    }
}