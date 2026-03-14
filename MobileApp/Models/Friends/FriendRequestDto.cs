using System;

namespace MobileApp.Models.Friends;

public class FriendRequestDto
{
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
    public string SenderUsername { get; set; } = string.Empty;
    public Guid ReceiverId { get; set; }
    public string ReceiverUsername { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}