using System;
using MobileApp.Models.Inventory;

namespace MobileApp.Services.Api.Inventory;

public interface IInventoryApiClient
{
    Task<List<InventoryItemDto>?> GetMyAsync();
    Task<InventoryItemDto?> EquipAsync(Guid inventoryItemId);
    Task<InventoryItemDto?> UnequipAsync(Guid inventoryItemId);
}