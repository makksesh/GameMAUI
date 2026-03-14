using System;

namespace MobileApp.Models.Support;

public class SupportTicketDto
{
    public Guid Id { get; set; }
    public Guid AuthorId { get; set; }
    public string AuthorUsername { get; set; } = string.Empty;
    public Guid? AssignedModeratorId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Resolution { get; set; }
    /// <summary>New | InProgress | Resolved | Closed</summary>
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<SupportMessageDto> Messages { get; set; } = [];

}