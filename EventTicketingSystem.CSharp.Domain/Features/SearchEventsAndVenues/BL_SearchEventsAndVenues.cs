namespace EventTicketingSystem.CSharp.Domain.Features.SearchEventsAndVenues;

public class BL_SearchEventsAndVenues
{
    private readonly DA_SearchEventsAndVenues _da_SearchEventsAndVenues;

    public BL_SearchEventsAndVenues(DA_SearchEventsAndVenues da_SearchEventsAndVenues)
    {
        _da_SearchEventsAndVenues = da_SearchEventsAndVenues;
    }

    public async Task<Result<SearchListEventsAndVenuesResponseModel>> SearchEventsAndVenues(string searchTerm)
    {
        return await _da_SearchEventsAndVenues.SearchEventsAndVenues(searchTerm);
    }

    public async Task<Result<SearchListEventsResponseModel>> SearchEventsByDate(DateTime StartDate, DateTime EndDate)
    {
        return await _da_SearchEventsAndVenues.SearchEventsByDate(StartDate, EndDate);
    }

    public async Task<Result<SearchListEventsResponseModel>> SearchEventsByAmountAsync(decimal FromAmount, decimal ToAmount)
    {
        return await _da_SearchEventsAndVenues.SearchEventsByAmountAsync(FromAmount, ToAmount);
    }
}