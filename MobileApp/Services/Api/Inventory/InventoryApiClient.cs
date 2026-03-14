using System;
using MobileApp.Models.Inventory;
using MobileApp.Services.Storage;

namespace MobileApp.Services.Api.Inventory;

public class InventoryApiClient : ApiClientBase, IInventoryApiClient
{
    public InventoryApiClient(IHttpClientFactory factory, ITokenStorage tokenStorage)
        : base(factory, tokenStorage) { }

    public Task<List<InventoryItemDto>?> GetMyAsync() =>
        GetAsync<List<InventoryItemDto>>("inventory");

    public Task<InventoryItemDto?> EquipAsync(Guid inventoryItemId) =>
        PostAsync<InventoryItemDto>($"inventory/{inventoryItemId}/equip");

    public Task<InventoryItemDto?> UnequipAsync(Guid inventoryItemId) =>
        PostAsync<InventoryItemDto>($"inventory/{inventoryItemId}/unequip");
}