namespace EventTicketingSystem.CSharp.Shared.Services;

public static class PaginationExtension
{
    public static async Task<PaginatedListModel<T>> ToPaginatedList<T>(
        this IQueryable<T> query,
        PaginationModel pagination)
    {
        if (pagination.PageNo < 1) pagination.PageNo = 1;
        if (pagination.PageSize < 1) pagination.PageSize = 10;

        var totalRowCounts = await query.CountAsync();

        var items = await query
            .Skip((pagination.PageNo - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        pagination.TotalRowCount = totalRowCounts;

        return new PaginatedListModel<T>(items, pagination);
    }
}