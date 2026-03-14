using System;

namespace MobileApp.Models.Trade;

public class TradeLotDto
{
    public Guid Id { get; set; }
    public Guid SellerId { get; set; }
    public Guid ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    /// <summary>Common | Uncommon | Rare | Epic | Legendary</summary>
    public string ItemRarity { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    /// <summary>Active | Sold | Cancelled</summary>
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}