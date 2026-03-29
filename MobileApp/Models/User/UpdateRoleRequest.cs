namespace MobileApp.Models.User;

/// <summary>
/// Роли: "Player" | "Moderator"
/// </summary>
public class UpdateRoleRequest
{
    public string Role { get; set; } = string.Empty;
}