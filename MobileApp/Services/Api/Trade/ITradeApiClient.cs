using System;
using MobileApp.Models.Trade;

namespace MobileApp.Services.Api.Trade;

public interface ITradeApiClient
{
    Task<List<TradeLotDto>?> GetActiveLotsAsync();
    Task<TradeLotDto?> CreateLotAsync(CreateTradeLotRequest request);
    Task<TradeLotDto?> BuyAsync(BuyItemRequest request);
    Task CancelLotAsync(Guid lotId);
}