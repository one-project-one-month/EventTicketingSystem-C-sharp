namespace EventTicketingSystem.CSharp.Shared.Constants;

public class Queries
{
    public static string sp_sequencecode { get; } = "SELECT sp_sequencecode(@id)";

    public static string sp_sequencecode_bulk { get; } = "SELECT * FROM sp_sequencecode_bulk(@id, @qty)";

    public static string sp_ticket_info { get; } = "SELECT * FROM sp_ticket_info(@p_ticketcode)";

    public static string fn_gettransactionhistorylist { get; } = "SELECT * FROM fn_gettransactionhistorylist()";

    public static string fn_gettransactionhistorydetail { get; } = "SELECT * FROM fn_gettransactionhistorydetail(@transaction_code)";
}