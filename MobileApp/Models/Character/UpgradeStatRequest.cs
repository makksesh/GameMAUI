namespace MobileApp.Models;

/// <summary>
/// Статистика: "health" | "mana" | "armor" | "damage"
/// </summary>
public class UpgradeStatRequest
{
    public string Stat { get; set; } = string.Empty;
    public int Amount { get; set; }
}