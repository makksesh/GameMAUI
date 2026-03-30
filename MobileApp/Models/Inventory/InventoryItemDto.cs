using System;

namespace MobileApp.Models.Inventory;

public class InventoryItemDto
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    /// <summary>Weapon | Armor | Helmet | Boots | Ring | Potion | Material | QuestItem</summary>
    public string ItemType { get; set; } = string.Empty;
    /// <summary>Common | Uncommon | Rare | Epic | Legendary</summary>
    public string Rarity { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public bool IsEquipped { get; set; }
    public decimal BasePrice { get; set; }

    /// <summary>Флаг пустой заглушки — не сериализуется с сервера, только клиентский.</summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public bool IsEmpty { get; set; }
}
