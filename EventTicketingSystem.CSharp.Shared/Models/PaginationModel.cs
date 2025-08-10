namespace EventTicketingSystem.CSharp.Shared.Models;

public class PaginationModel
{
    private int _pageNumber = 1;
    private int _pageSize = 10;

    public int TotalRowCount { get; set; }

    public int PageNo { get => _pageNumber; set => _pageNumber = value < 1 ? 1 : value; }

    public int PageSize { get => _pageSize; set => _pageSize = value < 1 ? 10 : value; }

    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(TotalRowCount / (double)PageSize) : 0;
}

public class PaginatedListModel<T>
{
    public List<T> ItemList { get; }

    public PaginationModel Pagination { get; }

    public PaginatedListModel(List<T> items, PaginationModel pagination)
    {
        ItemList = items;
        Pagination = pagination;
    }
}