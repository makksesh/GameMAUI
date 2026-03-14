using System;
using MobileApp.Models.Trade;
using MobileApp.Services.Storage;

namespace MobileApp.Services.Api.Trade;

public class TradeApiClient : ApiClientBase, ITradeApiClient
{
    public TradeApiClient(IHttpClientFactory factory, ITokenStorage tokenStorage)
        : base(factory, tokenStorage) { }

    public Task<List<TradeLotDto>?> GetActiveLotsAsync() =>
        GetAsync<List<TradeLotDto>>("trades");

    public Task<TradeLotDto?> CreateLotAsync(CreateTradeLotRequest request) =>
        PostAsync<TradeLotDto>("trades/lots", request);

    public Task<TradeLotDto?> BuyAsync(BuyItemRequest request) =>
        PostAsync<TradeLotDto>("trades/buy", request);

    public Task CancelLotAsync(Guid lotId) =>
        DeleteAsync($"trades/lots/{lotId}");
}