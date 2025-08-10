namespace EventTicketingSystem.CSharp.Domain.Features.Ticket;

public class BL_Ticket
{
    private readonly DA_Ticket _da_Ticket;

    public BL_Ticket(DA_Ticket da_Ticket)
    {
        _da_Ticket = da_Ticket;
    }

    public async Task<Result<TicketEditResponseModel>> CreateTicket(TicketCreateRequestModel requestModel)
    {
        return await _da_Ticket.CreateTicket(requestModel);
    }

    public async Task<Result<TicketEditResponseModel>> GetTicketByCode(string ticketCode)
    {
        return await _da_Ticket.GetTicketByCode(ticketCode);
    }

    public async Task<Result<TicketListResponseModel>> List(int pageNo)
    {
        return await _da_Ticket.List(pageNo);
    }

    public async Task<Result<TicketResponseModel>> DeleteByCode(string ticketCode)
    {
        return await _da_Ticket.DeleteByCode(ticketCode);
    }

    public async Task<Result<TicketResponseModel>> UpdateTicket(string ticketCode, bool isUsed)
    {
        return await _da_Ticket.UpdateTicket(ticketCode, isUsed);
    }
}
