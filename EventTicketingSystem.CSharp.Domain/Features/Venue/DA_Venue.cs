namespace EventTicketingSystem.CSharp.Domain.Features.Venue;

public class DA_Venue : AuthorizationService
{
    private readonly ILogger<DA_Venue> _logger;
    private readonly AppDbContext _db;
    private readonly CommonService _commonService;

    public DA_Venue(IHttpContextAccessor httpContextAccessor,
                    ILogger<DA_Venue> logger,
                    AppDbContext db,
                    CommonService commonService) : base(httpContextAccessor)
    {
        _logger = logger;
        _db = db;
        _commonService = commonService;
    }

    #region Get Venue List
    public async Task<Result<VenueListResponseModel>> List()
    {
        var model = new VenueListResponseModel();
        try
        {
            var data = await _db.TblVenues
                        .Where(x => x.Deleteflag == false)
                        .OrderBy(x => x.Venueid)
                        .ToListAsync();
            if (data is null)
            {
                return Result<VenueListResponseModel>.NotFoundError("No venue found.");
            }

            model.VenueList = data.Select(VenueListModel.FromTblVenue).ToList();
            return Result<VenueListResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<VenueListResponseModel>.SystemError(ex.Message);
        }
    }

    #endregion

    #region Get VenueById

    public async Task<Result<VenueEditResponseModel>> Edit(string venueCode)
    {
        var model = new VenueEditResponseModel();
        if (venueCode.IsNullOrEmpty())
        {
            return Result<VenueEditResponseModel>.ValidationError("Venue Id cannot be null or empty.");
        }

        try
        {
            var venue = await _db.TblVenues
                            .FirstOrDefaultAsync(
                                x => x.Venuecode == venueCode &&
                                x.Deleteflag == false
                            );
            if (venue is null)
            {
                return Result<VenueEditResponseModel>.NotFoundError("No venue found.");
            }

            model.Venue = VenueEditModel.FromTblVenue(venue);
            return Result<VenueEditResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<VenueEditResponseModel>.SystemError(ex.Message);
        }
    }

    #endregion

    #region Create Venue
    public async Task<Result<VenueCreateResponseModel>> Create(VenueCreateRequestModel requestModel)
    {
        string imageLink = string.Empty;
        string addons = string.Empty;

        try
        {   
            if (requestModel.VenueImage != null && requestModel.VenueImage.Count > 0)
            {
                var uploadResults = await EnumDirectory.VenueImage.UploadFilesAsync(requestModel.VenueImage);

                imageLink = string.Join(",", uploadResults.Select(x => x.FilePath));
            }

            if (requestModel.Addons != null && requestModel.Addons.Count > 0)
            {
                addons = string.Join(",", requestModel.Addons.Select(a => a.Trim()));
            }

            var newVenue = new TblVenue()
            {
                Venueid = GenerateUlid(),
                Venuecode = await _commonService.GenerateSequenceCode(EnumTableUniqueName.Tbl_Venue),
                Venuetypecode = requestModel.VenueTypeCode,
                Venuename = requestModel.VenueName,
                Description = requestModel.Description,
                Address = requestModel.Address!,
                Capacity = requestModel.Capacity,
                Facilities = requestModel.Facilities,
                Addons = addons,
                Venueimage = imageLink,
                Createdby = CurrentUserId,
                Createdat = DateTime.Now,
                Deleteflag = false
            };

            await _db.TblVenues.AddAsync(newVenue);
            await _db.SaveAndDetachAsync();

            return Result<VenueCreateResponseModel>.Success("Venue created successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<VenueCreateResponseModel>.SystemError(ex.Message);
        }
    }

    #endregion

    #region Update Venue

    public async Task<Result<VenueUpdateResponseModel>> Update(VenueUpdateRequestModel requestModel)
    {
        string addons = string.Empty;

        if (requestModel.VenueCode.IsNullOrEmpty())
        {
            return Result<VenueUpdateResponseModel>.UserInputError("Venue code cannot be null or empty.");
        }
        
        if (requestModel.Address.IsNullOrEmpty() || requestModel.Description.IsNullOrEmpty() || 
            requestModel.Facilities.IsNullOrEmpty() || requestModel.Addons.IsNullOrEmpty())
        {
            return Result<VenueUpdateResponseModel>.UserInputError("All fields are empty. Please provide at least one field to update.");
        }

        try
        {
            var existingVenue = await _db.TblVenues
                                        .FirstOrDefaultAsync(
                                            x => x.Venuecode == requestModel.VenueCode &&
                                            x.Deleteflag == false
                                        );

            if (existingVenue is null)
            {
                return Result<VenueUpdateResponseModel>.NotFoundError("No venue found.");
            }

            if (requestModel.Addons != null && requestModel.Addons.Count > 0)
            {
                addons = string.Join(",", requestModel.Addons.Select(a => a.Trim()));
            }

            existingVenue.Description = requestModel.Description;
            existingVenue.Address = requestModel.Address!;
            existingVenue.Facilities = requestModel.Facilities;
            existingVenue.Addons = addons;
            existingVenue.Modifiedby = CurrentUserId;
            existingVenue.Modifiedat = DateTime.Now;

            _db.Entry(existingVenue).State = EntityState.Modified;
            await _db.SaveAndDetachAsync();

            return Result<VenueUpdateResponseModel>.Success("Venue updated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<VenueUpdateResponseModel>.SystemError(ex.Message);
        }
    }

    #endregion

    #region Delete Venue

    public async Task<Result<VenueDeleteResponseModel>> Delete(string venueCode)
    {
        if (venueCode.IsNullOrEmpty())
        {
            return Result<VenueDeleteResponseModel>.UserInputError("Venue Code cannot be null or empty.");
        }

        try
        {
            var venue = await _db.TblVenues.FirstOrDefaultAsync(x => x.Venuecode == venueCode && x.Deleteflag == false);

            if (venue is null)
            {
                return Result<VenueDeleteResponseModel>.NotFoundError("There is no venue with this venue id.");
            }

            venue.Deleteflag = true;
            venue.Modifiedby = CurrentUserId;
            venue.Modifiedat = DateTime.Now;

            _db.Entry(venue).State = EntityState.Modified;
            await _db.SaveAndDetachAsync();

            return Result<VenueDeleteResponseModel>.Success("Venue deleted successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<VenueDeleteResponseModel>.SystemError(ex.Message);
        }
    }

    #endregion

    #region User Venues

    public async Task<Result<UserVenueListResponseModel>> UserVenueList(int pageNo)
    {
        const int pageSize = 9;
        pageNo = pageNo <= 0 ? 1 : pageNo;

        try
        {
            var query =
                from venue in _db.TblVenues
                join venueType in _db.TblVenuetypes on venue.Venuetypecode equals venueType.Venuetypecode
                where venue.Deleteflag == false && venueType.Deleteflag == false
                orderby venue.Createdat descending
                select new
                {
                    venue.Venuecode,
                    venue.Venuetypecode,
                    venueType.Venuetypename,
                    venue.Venuename,
                    venue.Capacity,
                    venue.Address,
                    venue.Venueimage
                };

            var total = await query.CountAsync();
            var rows = await query.Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();

            var model = new UserVenueListResponseModel
            {
                VenueList = rows.Select(x => new UserVenueListModel
                {
                    VenueCode = x.Venuecode,
                    VenueTypeCode = x.Venuetypecode,
                    Venuetypename = x.Venuetypename,
                    VenueName = x.Venuename,
                    Capacity = x.Capacity,
                    Address = x.Address,
                    Venueimage = SplitCsv(x.Venueimage)
                }).ToList(),
                TotalRowCount = total,
                PageNo = pageNo,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(total / (double)pageSize)
            };

            return Result<UserVenueListResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<UserVenueListResponseModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<UserVenueDetailResponseModel>> UserVenueDetail(string venueCode)
    {
        try
        {
            var venue = await _db.TblVenues
                .FirstOrDefaultAsync(x => x.Venuecode == venueCode && x.Deleteflag == false);

            if (venue is null)
            {
                return Result<UserVenueDetailResponseModel>.NotFoundError("Venue not found.");
            }

            var model = new UserVenueDetailResponseModel
            {
                Venue = new UserVenueDetailModel
                {
                    VenueCode = venue.Venuecode,
                    VenueTypeCode = venue.Venuetypecode,
                    VenueName = venue.Venuename,
                    Capacity = venue.Capacity,
                    Address = venue.Address,
                    Description = venue.Description ?? string.Empty,
                    Addons = SplitCsv(venue.Addons),
                    Facilities = venue.Facilities ?? string.Empty,
                    VenueImage = SplitCsv(venue.Venueimage)
                }
            };

            return Result<UserVenueDetailResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<UserVenueDetailResponseModel>.SystemError(ex.Message);
        }
    }

    private static List<string> SplitCsv(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? new List<string>()
            : value.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();
    }

    #endregion
}
