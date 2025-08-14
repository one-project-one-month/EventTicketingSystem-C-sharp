namespace EventTicketingSystem.CSharp.Domain.Features.BusinessOwner;

public class DA_BusinessOwner : AuthorizationService
{
    private readonly ILogger<DA_BusinessOwner> _logger;
    private readonly AppDbContext _db;
    private readonly CommonService _commonService;

    public DA_BusinessOwner(IHttpContextAccessor httpContextAccessor,
                            ILogger<DA_BusinessOwner> logger,
                            AppDbContext db,
                            CommonService commonService) : base(httpContextAccessor)
    {
        _logger = logger;
        _db = db;
        _commonService = commonService;
    }

    #region Get Business Owner List

    public async Task<Result<BusinessOwnerListResponseModel>> List()
    {
        try
        {
            var businessOwnerList = await _db.TblBusinessowners
                                    .Where(x => !x.Deleteflag)
                                    .OrderByDescending(x => x.Businessownerid)
                                    .ToListAsync();

            if (!businessOwnerList.Any() || businessOwnerList is null)
                return Result<BusinessOwnerListResponseModel>.NotFoundError("No Owner Found.");

            var model = new BusinessOwnerListResponseModel
            {
                BusinessOwners = businessOwnerList
                    .Select(BusinessOwnerListModel.FromTblOwner)
                    .ToList(),
            };

            return Result<BusinessOwnerListResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<BusinessOwnerListResponseModel>.SystemError(ex.Message);
        }
    }

    #endregion

    #region Get Business Owner By Code

    public async Task<Result<BusinessOwnerEditResponseModel>> Edit(string ownerCode)
    {
        var model = new BusinessOwnerEditResponseModel();
        if (ownerCode.IsNullOrEmpty())
        {
            return Result<BusinessOwnerEditResponseModel>.UserInputError("Owner Code can't be Null or Empty!");
        }

        try
        {
            var data = await _db.TblBusinessowners
                        .FirstOrDefaultAsync(
                            x => x.Businessownercode == ownerCode &&
                            x.Deleteflag == false
                        );
            if (data is null)
            {
                return Result<BusinessOwnerEditResponseModel>.NotFoundError("Owner Not Found.");
            }

            model.BusinessOwner = BusinessOwnerEditModel.FromTblOwner(data!);
            return Result<BusinessOwnerEditResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<BusinessOwnerEditResponseModel>.SystemError(ex.Message);
        }
    }

    #endregion

    #region Create Business Owner

    public async Task<Result<BusinessOwnerCreateResponseMOdel>> Create(BusinessOwnerCreateRequestModel owner)
    {
        if (owner.Email.IsNullOrEmpty() || !owner.Email.IsValidEmail() || isEmailUsed(owner.Email))
        {
            return Result<BusinessOwnerCreateResponseMOdel>.ValidationError("Email is already used or invalid.");
        }
        else if (owner.FullName.IsNullOrEmpty() || owner.FullName.Length < 3)
        {
            return Result<BusinessOwnerCreateResponseMOdel>.ValidationError("Name can't be blank or less than 3 characters.");
        }
        else if (owner.Phone.IsNullOrEmpty() || owner.Phone.Length < 9)
        {
            return Result<BusinessOwnerCreateResponseMOdel>.ValidationError("Phone No can't be empty or less than 9 digits!");
        }
        else
        {
            try
            {
                var newOwner = new TblBusinessowner()
                {
                    Businessownerid = GenerateUlid(),
                    Businessownercode = await _commonService.GenerateSequenceCode(EnumTableUniqueName.Tbl_BusinessOwner),
                    Fullname = owner.FullName,
                    Email = owner.Email,
                    Phone = owner.Phone,
                    Createdby = CurrentUserId,
                    Createdat = DateTime.Now,
                    Deleteflag = false
                };
                await _db.TblBusinessowners.AddAsync(newOwner);
                await _db.SaveAndDetachAsync();

                return Result<BusinessOwnerCreateResponseMOdel>.Success("New Owner Created Successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogExceptionError(ex);
                return Result<BusinessOwnerCreateResponseMOdel>.SystemError(ex.Message);
            }
        }
    }

    #endregion

    #region Update Business Owner

    public async Task<Result<BusinessOwnerUpdateResponseMOdel>> Update(BusinessOwnerUpdateRequestModel requestModel)
    {
        string errorMessage = string.Empty;

        if (requestModel.Businessownercode.IsNullOrEmpty())
        {
            return Result<BusinessOwnerUpdateResponseMOdel>.NotFoundError("Owner Code is Required.");
        }

        try
        {
            var data = await _db.TblBusinessowners
                        .FirstOrDefaultAsync(
                            x => x.Businessownercode == requestModel.Businessownercode &&
                            x.Deleteflag == false
                        );
            if (data is null)
            {
                return Result<BusinessOwnerUpdateResponseMOdel>.NotFoundError("Owner Not Found.");
            }

            if (!requestModel.FullName.IsNullOrEmpty() && data.Fullname != requestModel.FullName)
            {
                if (requestModel.FullName.Length >= 3)
                {
                    data.Fullname = requestModel.FullName;
                }
                else
                {
                    errorMessage += "Name is invalid.\n";
                }
            }

            if (requestModel.Phone.IsNullOrEmpty() || requestModel.Phone.Length < 9)
            {
                errorMessage += "Phone No can't be empty or less than 9 digits!\n";
            }
            else if (data.Phone != requestModel.Phone)
            {
                data.Phone = requestModel.Phone;
            }

            data.Modifiedby = CurrentUserId;
            data.Modifiedat = DateTime.Now;
            _db.Entry(data).State = EntityState.Modified;
            await _db.SaveAndDetachAsync();

            if (errorMessage.IsNullOrEmpty())
            {
                return Result<BusinessOwnerUpdateResponseMOdel>.Success("Owner Updated Successfully.");
            }
            else
            {
                return Result<BusinessOwnerUpdateResponseMOdel>.ValidationError(errorMessage);
            }

        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<BusinessOwnerUpdateResponseMOdel>.SystemError(ex.Message);
        }
    }

    #endregion

    #region Delete Business Owner By Code

    public async Task<Result<BusinessOwnerDeleteResponseMOdel>> Delete(string ownerCode)
    {
        if (ownerCode.IsNullOrEmpty())
        {
            return Result<BusinessOwnerDeleteResponseMOdel>.UserInputError("Owner Code is Required.");
        }

        try
        {
            var data = await _db.TblBusinessowners
                        .FirstOrDefaultAsync(
                            x => x.Businessownercode == ownerCode &&
                            x.Deleteflag == false
                        );

            if (data == null)
            {
                return Result<BusinessOwnerDeleteResponseMOdel>.NotFoundError("Owner Not Found.");
            }

            data!.Modifiedby = CurrentUserId;
            data.Modifiedat = DateTime.Now;
            data.Deleteflag = true;
            _db.Entry(data).State = EntityState.Modified;
            await _db.SaveAndDetachAsync();

            return Result<BusinessOwnerDeleteResponseMOdel>.Success("Owner Deleted Successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<BusinessOwnerDeleteResponseMOdel>.SystemError(ex.Message);
        }
    }

    #endregion

    #region Private Helper Functions

    private bool isEmailUsed(String email)
    {
        var owner = _db.TblBusinessowners.AsNoTracking().FirstOrDefault(x => x.Email == email);
        return owner != null;
    }

    #endregion
}
