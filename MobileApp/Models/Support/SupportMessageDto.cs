namespace MobileApp.Models.Support;

public class SupportMessageDto
{
    public Guid Id { get; set; }
    public Guid TicketId { get; set; }
    public Guid AuthorId { get; set; }
    public string AuthorUsername { get; set; } = string.Empty;
    public bool IsFromModerator { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
