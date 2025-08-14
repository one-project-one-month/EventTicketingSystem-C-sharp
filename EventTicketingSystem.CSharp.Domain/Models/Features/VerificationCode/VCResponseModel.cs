namespace EventTicketingSystem.CSharp.Domain.Models.Features.VerificationCode;

public class VCResponseModel 
{
    public List<VCodeModel> VerificationCodes {  get; set; }

    public VCodeModel VerificationCode { get; set; }
}

public class VCodeModel{
    public string? VerificationId { get; set; }

    public string? VerificationCode { get; set; }

    public string? Email { get; set; }

    public DateTime? ExpiredTime { get; set; }

    public bool? Isused { get; set; }

    public string? Createdby { get; set; }

    public DateTime? Createdat { get; set; }

    public string? Modifiedby { get; set; }

    public DateTime? Modifiedat { get; set; }

    public bool? Deleteflag { get; set; }

    public static VCodeModel FromTblVerification(TblVerification verification)
    {
        return new VCodeModel
        {
            VerificationId = verification.Verificationid,
            VerificationCode = verification.Verificationcode,
            Email = verification.Email,
            ExpiredTime = verification.Expiredtime,
            Isused = verification.Isused,
            Createdby = verification.Createdby,
            Createdat = verification.Createdat,
            Modifiedat = verification.Modifiedat,
            Modifiedby = verification.Modifiedby,
            Deleteflag = verification.Deleteflag,
        };
    }
}