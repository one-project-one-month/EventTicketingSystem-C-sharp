namespace EventTicketingSystem.CSharp.Domain.Features.EventCategory;

public class DA_EventCategory : AuthorizationService
{
    private readonly ILogger<DA_EventCategory> _logger;
    private readonly AppDbContext _db;
    private readonly CommonService _commonService;

    public DA_EventCategory(IHttpContextAccessor httpContextAccessor,
                            ILogger<DA_EventCategory> logger,
                            AppDbContext db,
                            CommonService commonService) : base(httpContextAccessor)
    {
        _logger = logger;
        _db = db;
        _commonService = commonService;
    }

    #region Event Category List

    public async Task<Result<EventCategoryListResponseModel>> List()
    {
        try
        {
            var eventCategoryList = await _db.TblEventcategories
                                    .Where(x => !x.Deleteflag)
                                    .OrderByDescending(x => x.Eventcategoryid)
                                    .ToListAsync();

            if (!eventCategoryList.Any())
                return Result<EventCategoryListResponseModel>.NotFoundError("Event Type Not Found.");

            var model = new EventCategoryListResponseModel
            {
                EventCategories = eventCategoryList
                    .Select(EventCategoryListModel.FromTblCategory)
                    .ToList(),
            };

            return Result<EventCategoryListResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<EventCategoryListResponseModel>.SystemError(ex.Message);
        }
    }

    #endregion

    #region Event Category Edit

    public async Task<Result<EventCategoryEditResponseModel>> Edit(string eventCategoryCode)
    {
        var model = new EventCategoryEditResponseModel();
        if (eventCategoryCode.IsNullOrEmpty())
        {
            return Result<EventCategoryEditResponseModel>.UserInputError("Event Type Not Found.");
        }
        try
        {
            var item = await _db.TblEventcategories
                                        .FirstOrDefaultAsync(
                                            x => x.Eventcategorycode == eventCategoryCode &&
                                            x.Deleteflag == false
                                        );
            if (item is null)
            {
                return Result<EventCategoryEditResponseModel>.NotFoundError("Event Type Not Found.");
            }

            model.Event = EventCategoryEditModel.FromTblCategory(item);
            return Result<EventCategoryEditResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<EventCategoryEditResponseModel>.SystemError(ex.Message);
        }
    }

    #endregion

    #region Event Category Create

    public async Task<Result<EventCategoryCreateResponseModel>> Create(EventCategoryCreateRequestModel requestModel)
    {
        if (requestModel.CategoryName.IsNullOrEmpty())
        {
            return Result<EventCategoryCreateResponseModel>.ValidationError("Event Type Name Required.");
        }
        else if (isCategoryNameExist(requestModel.CategoryName))
        {
            return Result<EventCategoryCreateResponseModel>.ValidationError("Event Type Name already exist.");
        }
        else
        {
            try
            {
                var newCategory = new TblEventcategory
                {
                    Eventcategoryid = Ulid.NewUlid().ToString(),
                    Eventcategorycode = await _commonService.GenerateSequenceCode(EnumTableUniqueName.Tbl_EventCategory),
                    Categoryname = requestModel.CategoryName,
                    Createdat = DateTime.Now,
                    Createdby = CurrentUserId,
                    Deleteflag = false
                };
                await _db.TblEventcategories.AddAsync(newCategory);
                await _db.SaveAndDetachAsync();

                return Result<EventCategoryCreateResponseModel>.Success("Event Type Created Successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogExceptionError(ex);
                return Result<EventCategoryCreateResponseModel>.SystemError(ex.Message);
            }
        }
    }

    #endregion

    #region Event Category Update

    public async Task<Result<EventCategoryUpdateResponseModel>> Update(EventCategoryUpdateRequestModel requestModel)
    {
        if (requestModel.EventCategoryCode.IsNullOrEmpty())
        {
            return Result<EventCategoryUpdateResponseModel>.ValidationError("Event Type Code Not Found.");
        }
        else if (requestModel.CategoryName.IsNullOrEmpty())
        {
            return Result<EventCategoryUpdateResponseModel>.ValidationError("Event Type Name Required.");
        }
        else
        {
            try
            {
                var existingCategory = await _db.TblEventcategories
                                        .FirstOrDefaultAsync(
                                            x => x.Eventcategorycode == requestModel.EventCategoryCode &&
                                            x.Deleteflag == false
                                        );
                if (existingCategory is null)
                {
                    return Result<EventCategoryUpdateResponseModel>.NotFoundError("Event Type Name Not Found");
                }

                existingCategory.Categoryname = requestModel.CategoryName;
                existingCategory.Modifiedby = CurrentUserId;
                existingCategory.Modifiedat = DateTime.Now;
                _db.Entry(existingCategory).State = EntityState.Modified;
                await _db.SaveAndDetachAsync();

                return Result<EventCategoryUpdateResponseModel>.Success("Event Type Updated Successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogExceptionError(ex);
                return Result<EventCategoryUpdateResponseModel>.SystemError(ex.Message);
            }
        }
    }

    #endregion

    #region Event Category Delete

    public async Task<Result<EventCategoryDeleteResponseModel>> Delete(string eventCategoryCode)
    {
        if (eventCategoryCode.IsNullOrEmpty())
        {
            return Result<EventCategoryDeleteResponseModel>.UserInputError("Event Type Not Found.");
        }

        try
        {
            var item = await _db.TblEventcategories
                        .FirstOrDefaultAsync(
                            x => x.Eventcategorycode == eventCategoryCode &&
                            x.Deleteflag == false
                        );
            if (item is null)
            {
                return Result<EventCategoryDeleteResponseModel>.NotFoundError("Event Type Not Found.");
            }

            item.Deleteflag = true;
            item.Modifiedby = CurrentUserId;
            item.Modifiedat = DateTime.Now;
            _db.Entry(item).State = EntityState.Modified;
            await _db.SaveAndDetachAsync();

            return Result<EventCategoryDeleteResponseModel>.Success("Event Type Deleted Successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<EventCategoryDeleteResponseModel>.SystemError(ex.Message);
        }
    }

    #endregion

    #region Private function

    private bool isCategoryNameExist(string categoryName)
    {
        return _db.TblEventcategories
            .AsEnumerable()
            .Any(x => string.Equals(x.Categoryname, categoryName, StringComparison.OrdinalIgnoreCase));
    }

    #endregion
}