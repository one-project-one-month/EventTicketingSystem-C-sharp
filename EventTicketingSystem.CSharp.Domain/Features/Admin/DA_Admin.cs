namespace EventTicketingSystem.CSharp.Domain.Features.Admin;

public class DA_Admin : AuthorizationService
{
    private readonly AppDbContext _db;
    private readonly ILogger<DA_Admin> _logger;
    private readonly CommonService _commonService;
    private string specialCharacters = @"!@#$%^&*(),.?""{}|<>";
    private int minLength = 8;
    private int maxLength = 20;

    public DA_Admin(IHttpContextAccessor httpContextAccessor,
                    ILogger<DA_Admin> logger,
                    AppDbContext db,
                    CommonService commonService) : base(httpContextAccessor)
    {
        _logger = logger;
        _db = db;
        _commonService = commonService;
    }

    #region Get Admin List

    public async Task<Result<AdminListResponseModel>> List()
    {
        try
        {
            var admins = await _db.TblAdmins
                            .Where(x=>!x.Deleteflag)
                            .OrderByDescending(x=>x.Adminid)
                            .ToListAsync();            

            if (!admins.Any())
            {
                return Result<AdminListResponseModel>.Success("No admin user found.");
            };

            var model = new AdminListResponseModel
            {
                AdminList = admins.Select(AdminListModel.FromTblAdmin).ToList(),
            };

            return Result<AdminListResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<AdminListResponseModel>.SystemError(ex.Message);
        }
    }

    #endregion

    #region Get AdminByCode

    public async Task<Result<AdminEditResponseModel>> Edit(string adminCode)
    {
        var model = new AdminEditResponseModel();

        if (adminCode.IsNullOrEmpty())
        {
            return Result<AdminEditResponseModel>.ValidationError("Admin Code cannot be null or empty.");
        }

        try
        {
            var admin = await _db.TblAdmins
                        .FirstOrDefaultAsync(
                            x => x.Admincode == adminCode &&
                            x.Deleteflag == false
                        );
            if (admin is null)
            {
                return Result<AdminEditResponseModel>.NotFoundError("Admin Not Found.");
            }

            model.Admin = AdminEditModel.FromTblAdmin(admin);
            return Result<AdminEditResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<AdminEditResponseModel>.SystemError(ex.Message);
        }
    }

    #endregion

    #region Create Admin

    public async Task<Result<AdminCreateResponseModel>> Create(AdminCreateRequestModel requestModel)
    {
        var responseModel = ValidateRequest(requestModel, ValidateEmail(requestModel.Email));
        if (responseModel != null)
        {
            return responseModel;
        }

        try
        {
            var newAdmin = new TblAdmin()
            {
                Adminid = GenerateUlid(),
                Admincode = await _commonService.GenerateSequenceCode(EnumTableUniqueName.Tbl_Admin),
                Username = requestModel.Username,
                Email = requestModel.Email,
                Fullname = requestModel.FullName,
                Password = requestModel.Password.HashPassword(requestModel.Username),
                Phone = requestModel.PhoneNo,
                Profileimage = null,
                Isfirsttime = true,
                Createdby = CurrentUserId,
                Createdat = DateTime.Now,
                Deleteflag = false,
            };

            await _db.TblAdmins.AddAsync(newAdmin);
            await _db.SaveAndDetachAsync();

            return Result<AdminCreateResponseModel>.Success("Admin Created Successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<AdminCreateResponseModel>.SystemError(ex.ToString());
        }
    }

    #endregion

    #region Update Admin

    public async Task<Result<AdminUpdateResponseModel>> Update(AdminUpdateRequestModel requestModel)
    {
        var responseModel = new Result<AdminUpdateResponseModel>();
        var model = new AdminUpdateResponseModel();
        string errorMsg = string.Empty;

        if (requestModel.AdminCode.IsNullOrEmpty())
        {
            return Result<AdminUpdateResponseModel>.UserInputError("Admin Not Found");
        }

        if (requestModel.PhoneNo.IsNullOrEmpty() || requestModel.FullName.IsNullOrEmpty())
        {
            return Result<AdminUpdateResponseModel>.UserInputError("All fields are empty. Please provide at least one field to update.");
        }

        var admin = await _db.TblAdmins
                            .FirstOrDefaultAsync(
                                x => x.Admincode == requestModel.AdminCode &&
                                x.Deleteflag == false
                            );
        if (admin is null)
        {
            return Result<AdminUpdateResponseModel>.NotFoundError("Admin Not Found.");
        }

        //if (requestModel.Password != admin.Password)
        //{
        //    var validationError = ValidateFields(requestModel);
        //    if (validationError != null)
        //    {
        //        return Result<AdminUpdateResponseModel>.ValidationError(validationError);
        //    }
        //}
        if (!requestModel.PhoneNo.IsNullOrEmpty())
        {
            admin.Phone = requestModel.PhoneNo;
        }
        if (!requestModel.FullName.IsNullOrEmpty())
        {
            admin.Fullname = requestModel.FullName;
        }
        //if (!requestModel.Password.IsNullOrEmpty())
        //{
        //    admin.Password = requestModel.Password.HashPassword(admin.Username);
        //}

        try
        {
            admin.Modifiedby = CurrentUserId;
            admin.Modifiedat = DateTime.Now;
            _db.Entry(admin).State = EntityState.Modified;
            await _db.SaveAndDetachAsync();

            responseModel = Result<AdminUpdateResponseModel>.Success("Admin Updated Successufully.");
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<AdminUpdateResponseModel>.SystemError(ex.ToString());
        }

        return responseModel;
    }

    #endregion

    #region Delete Admin

    public async Task<Result<AdminDeleteResponseModel>> Delete(AdminDeleteRequestModel requestModel)
    {
        if (requestModel.AdminCode.IsNullOrEmpty())
        {
            return Result<AdminDeleteResponseModel>.UserInputError("Admin Not Found.");
        }
        if (requestModel.Password.IsNullOrEmpty())
        {
            return Result<AdminDeleteResponseModel>.UserInputError("Password is Required.");
        }

        try
        {
            var data = await _db.TblAdmins
                        .FirstOrDefaultAsync(
                            x => x.Admincode == requestModel.AdminCode &&
                            x.Deleteflag == false
                        );
            if (data is null)
            {
                return Result<AdminDeleteResponseModel>.NotFoundError("Admin Not Found.");
            }

            var passwordHash = requestModel.Password.HashPassword(data.Username);
            if (data.Password != passwordHash)
            {
                return Result<AdminDeleteResponseModel>.UserInputError("Incorrect Password.");
            }

            data.Modifiedby = CurrentUserId;
            data.Modifiedat = DateTime.Now;
            data.Deleteflag = true;
            _db.Entry(data).State = EntityState.Modified;
            await _db.SaveAndDetachAsync();

            return Result<AdminDeleteResponseModel>.Success("Admin data is deleted successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<AdminDeleteResponseModel>.SystemError(ex.Message);
        }
    }

    #endregion

    #region Edit Profile Image

    public async Task<Result<AdminEditProfileResponseModel>> EditProfile(AdminEditProfileRequestModel requestModel)
    {
        if (requestModel.AdminCode.IsNullOrEmpty())
        {
            return Result<AdminEditProfileResponseModel>.UserInputError("Admin Code cannot be null or empty.");
        }
        try
        {
            var admin = await _db.TblAdmins
                            .FirstOrDefaultAsync(
                                x => x.Admincode == requestModel.AdminCode &&
                                x.Deleteflag == false
                            );
            if (admin is null)
            {
                return Result<AdminEditProfileResponseModel>.NotFoundError("Admin Not Found.");
            }

            var uploadResults = await EnumDirectory.ProfileImage.UploadFilesAsync(new List<IFormFile>() { requestModel.ProfileImage });
            admin.Profileimage = string.Join(",", uploadResults.Select(x => x.FilePath));
            admin.Fullname = requestModel.FullName;
            admin.Phone = requestModel.Phone;
            admin.Modifiedby = CurrentUserId;
            admin.Modifiedat = DateTime.Now;
            _db.Entry(admin).State = EntityState.Modified;
            await _db.SaveAndDetachAsync();

            return Result<AdminEditProfileResponseModel>.Success("Profile updated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<AdminEditProfileResponseModel>.SystemError(ex.Message);
        }
    }

    #endregion

    #region Admin Change Password

    public async Task<Result<AdminChangePasswordResponseModel>> ChangePassword(AdminChangePasswordRequestModel requestModel)
    {
        if (requestModel.AdminCode.IsNullOrEmpty())
        {
            return Result<AdminChangePasswordResponseModel>.UserInputError("Admin Code cannot be null or empty.");
        }
        if (requestModel.UserName.IsNullOrEmpty())
        {
            return Result<AdminChangePasswordResponseModel>.UserInputError("Username Required.");
        }
        if (requestModel.OldPassword.IsNullOrEmpty() ||
            requestModel.NewPassword.IsNullOrEmpty() ||
            requestModel.ConfirmNewPassword.IsNullOrEmpty())
        {
            return Result<AdminChangePasswordResponseModel>.UserInputError("Old Password and New Password fields cannot be empty.");
        }
        if (requestModel.NewPassword != requestModel.ConfirmNewPassword)
        {
            return Result<AdminChangePasswordResponseModel>.ValidationError("New Password and Confirm Password do not match.");
        }
        if (requestModel.NewPassword == requestModel.ConfirmNewPassword)
        {
            var validationError = ValidatePassword(requestModel.UserName!, requestModel.NewPassword!);
            if (validationError is not null)
            {
                return Result<AdminChangePasswordResponseModel>.ValidationError(validationError);
            }
        }

        try
        {
            var admin = await _db.TblAdmins
                            .FirstOrDefaultAsync(
                                x => x.Admincode == requestModel.AdminCode &&
                                x.Deleteflag == false
                            );
            if (admin is null)
            {
                return Result<AdminChangePasswordResponseModel>.NotFoundError("Admin not found.");
            }

            var oldPasswordHash = requestModel.OldPassword!.HashPassword(admin.Username);
            if (admin.Password != oldPasswordHash)
            {
                return Result<AdminChangePasswordResponseModel>.UserInputError("Incorrect old password.");
            }

            admin.Password = requestModel.NewPassword!.HashPassword(admin.Username);
            admin.Modifiedby = CurrentUserId;
            admin.Modifiedat = DateTime.Now;
            _db.Entry(admin).State = EntityState.Modified;
            await _db.SaveAndDetachAsync();

            return Result<AdminChangePasswordResponseModel>.Success("Password changed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<AdminChangePasswordResponseModel>.SystemError(ex.Message);
        }
    }

    #endregion

    #region Helper Function

    private static Result<AdminCreateResponseModel> ValidateRequest(AdminCreateRequestModel requestModel, bool isEmailUsed)
    {
        if (requestModel.Username.IsNullOrEmpty())
        {
            return Result<AdminCreateResponseModel>.ValidationError("Username cannot be empty!");
        }

        if (requestModel.Email.IsNullOrEmpty())
        {
            return Result<AdminCreateResponseModel>.ValidationError("Email Cannot be blank!");
        }

        if (!requestModel.Email.IsValidEmail() || isEmailUsed)
        {
            return Result<AdminCreateResponseModel>.ValidationError("Email is either already used or invalid!");
        }

        if (requestModel.PhoneNo.IsNullOrEmpty() || requestModel.PhoneNo!.Length < 9)
        {
            return Result<AdminCreateResponseModel>.ValidationError("Phone number cannot be empty or less than 9 numbers!");
        }

        if (requestModel.Password.IsNullOrEmpty() || requestModel.Password!.Length < 8)
        {
            return Result<AdminCreateResponseModel>.ValidationError("Password cannot be empty or must contain at least 8 characters");
        }

        if (!requestModel.Password.Any(char.IsUpper))
        {
            return Result<AdminCreateResponseModel>.ValidationError("Password must contain at least one upper letter!");
        }

        if (!requestModel.Password.Any(char.IsLower))
        {
            return Result<AdminCreateResponseModel>.ValidationError("Password must contain at least one lower letter!");
        }

        if (!requestModel.Password.Any(char.IsDigit))
        {
            return Result<AdminCreateResponseModel>.ValidationError("Password must contain at least one digit!");
        }

        if (!requestModel.Password.Any(c => !char.IsLetterOrDigit(c)))
        {
            return Result<AdminCreateResponseModel>.ValidationError("Password must contain at least one special character!");
        }

        return null!;
    }

    private string? ValidatePassword(string userName, string password)
    {
        if (password.IsNullOrEmpty())
            return "Password cannot be empty";

        if (password.Length < minLength)
            return $"Password must be at least {minLength} characters";

        if (password.Length > maxLength)
            return $"Password cannot exceed {maxLength} characters";

        if (password.IsSameDigit())
            return "Password cannot contain all repeating characters";

        if (password.IsSequential())
            return "Password contains predictable sequences (e.g., 12345 or qwerty)";

        if (!Regex.IsMatch(password, $"[{Regex.Escape(specialCharacters)}]"))
            return $"Password must contain at least one special character: {specialCharacters}";

        if (!Regex.IsMatch(password, "[A-Z]"))
            return "Password must contain at least one uppercase letter (A-Z)";

        if (!Regex.IsMatch(password, "[a-z]"))
            return "Password must contain at least one lowercase letter (a-z)";

        if (!Regex.IsMatch(password, "[0-9]"))
            return "Password must contain at least one digit (0-9)";

        if (password.Contains(userName, StringComparison.OrdinalIgnoreCase))
            return "Password cannot contain your username";

        return null;
    }


    private bool ValidateEmail(string email)
    {
        var admin = _db.TblAdmins.FirstOrDefault(x => x.Email == email);
        if (admin is null) return false;
        return true;
    }

    #endregion  
}