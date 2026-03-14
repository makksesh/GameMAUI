namespace MobileApp.Models.Support;

public class CreateTicketRequest
{
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}