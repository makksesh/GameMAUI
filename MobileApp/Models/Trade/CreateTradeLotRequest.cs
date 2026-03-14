using System;

namespace MobileApp.Models.Trade;

public class CreateTradeLotRequest
{
    public Guid ItemId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}