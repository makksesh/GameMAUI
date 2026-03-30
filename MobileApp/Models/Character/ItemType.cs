namespace MobileApp.Models;

/// <summary>
/// Клиентское зеркало серверного enum — используется только для строковых сравнений.
/// Значения должны совпадать с тем, что присылает API.
/// </summary>
public enum ItemType
{
    Weapon,
    Armor,
    Helmet,
    Boots,
    Ring,
    Potion,
    Material,
    QuestItem
}