using System;

namespace MobileApp.Models.Friends;

/// <summary>Pending | Accepted | Declined</summary>
public class FriendshipDto
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
}