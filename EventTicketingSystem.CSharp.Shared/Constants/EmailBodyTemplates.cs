namespace EventTicketingSystem.CSharp.Shared.Constants;

public class EmailBodyTemplates
{
    public static string Otp { get; } = "Your Verification Code is: <b>(@otp)</b>.";

    public static string TicketPurchaseSuccess { get; } = @"
    <h2>Thank you for your purchase!</h2>
    <p>Your tickets have been successfully purchased.</p>

    <p><strong>Event:</strong> (@eventName)</p>
    <p><strong>Number of Tickets:</strong> (@ticketQuantity)</p>
    <p><strong>Total Amount:</strong> (@totalAmount) MMK</p>
    <p><strong>Transaction Code:</strong> (@transactionCode)</p>

    <p>Please keep this email for your records. Your tickets QR codes had been sent below.</p>

    <hr>
    <p>If you have any questions, please contact our support team.</p>
    <p>Enjoy the event!</p>";
}