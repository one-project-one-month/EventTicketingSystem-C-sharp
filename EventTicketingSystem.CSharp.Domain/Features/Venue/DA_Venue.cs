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

    public async Task<Result<VenueListResponseModel>> List(int pageNo)
    {
        try
        {
            var pagination = new PaginationModel { PageNo = pageNo, PageSize = 10 };

            var paginatedResult = await _db.TblVenues
                                    .Where(x => !x.Deleteflag)
                                    .OrderByDescending(x => x.Venueid)
                                    .ToPaginatedList(pagination);

            if (!paginatedResult.ItemList.Any())
                return Result<VenueListResponseModel>.NotFoundError("No venue found.");

            var model = new VenueListResponseModel
            {
                VenueList = paginatedResult.ItemList
                    .Select(VenueListModel.FromTblVenue)
                    .ToList(),
                PageNo = paginatedResult.Pagination.PageNo,
                PageSize = paginatedResult.Pagination.PageSize,
                TotalRowCount = paginatedResult.Pagination.TotalRowCount
            };

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
            var adminDomainUrl = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, EnumDirectory.wwwroot.ToString());

            var venue = await _db.TblVenues
                            .FirstOrDefaultAsync(
                                x => x.Venuecode == venueCode &&
                                x.Deleteflag == false
                            );
            if (venue is null)
            {
                return Result<VenueEditResponseModel>.NotFoundError("No venue found.");
            }

            model.Venue = VenueEditModel.FromTblVenue(venue, adminDomainUrl!);

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
        string imageLink = string.Empty;
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

            if (requestModel.VenueImage != null && requestModel.VenueImage.Count > 0)
            {
                var uploadResults = await EnumDirectory.VenueImage.UploadFilesAsync(requestModel.VenueImage);

                imageLink = string.Join(",", uploadResults.Select(x => x.FilePath));
            }

            existingVenue.Description = requestModel.Description;
            existingVenue.Address = requestModel.Address!;
            existingVenue.Facilities = requestModel.Facilities;
            existingVenue.Addons = addons;
            existingVenue.Venueimage = imageLink;
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

    #region Get Venue List For User

    public async Task<Result<VenueListResponseModeForUser>> UserVenueList(int pageNo)
    {
        try
        {
            var pagination = new PaginationModel { PageNo = pageNo, PageSize = 9 };

            var paginatedResult = await _db.TblVenues
                                    .Join(_db.TblVenuetypes,
                                        ve => ve.Venuetypecode,
                                        vt => vt.Venuetypecode,
                                        (ve, vt) => new { ve, vt })
                                    .Where(x => !x.ve.Deleteflag)
                                    .OrderByDescending(x => x.ve.Venueid)
                                    .ToPaginatedList(pagination);

            var top3VenueCodes = await _db.TblEvents
                                    .Where(e => e.Deleteflag == false)
                                    .GroupBy(e => e.Venuecode)
                                    .OrderByDescending(g => g.Count())
                                    .Select(g => g.Key)
                                    .Take(3)
                                    .ToListAsync();

            var VenuesResult = await _db.TblVenues
                                    .Where(v => top3VenueCodes.Contains(v.Venuecode) && v.Deleteflag == false)
                                    .Join(
                                        _db.TblVenuetypes,
                                        v => v.Venuetypecode,
                                        vt => vt.Venuetypecode,
                                        (v, vt) => new { v, vt }
                                    )
                                    .ToListAsync();

            if (!paginatedResult.ItemList.Any())
                return Result<VenueListResponseModeForUser>.NotFoundError("No venue found.");

            var userDomainUrl = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, EnumDirectory.wwwroot.ToString());

            var model = new VenueListResponseModeForUser
            {
                VenueList = paginatedResult.ItemList
                    .Select(x => VenueListModelForUser.FromTblVenue(x.ve, x.vt, userDomainUrl!))
                    .ToList(),
                Top3Venues = VenuesResult.Select(x => VenueListModelForUser.FromTblVenue(x.v, x.vt, userDomainUrl!)).ToList(),
                PageNo = paginatedResult.Pagination.PageNo,
                PageSize = paginatedResult.Pagination.PageSize,
                TotalRowCount = paginatedResult.Pagination.TotalRowCount
            };

            return Result<VenueListResponseModeForUser>.Success(model);
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<VenueListResponseModeForUser>.SystemError(ex.Message);
        }
    }

    #endregion
}