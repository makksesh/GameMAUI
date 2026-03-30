namespace MobileApp.Models.Inventory;

/// <summary>
/// Представляет одну ячейку в сетке инвентаря.
/// HasItem = false → ячейка пустая (серый квадрат).
/// </summary>
public class InventorySlot
{
    public InventoryItemDto? Item { get; set; }
    public bool HasItem => Item is not null;
}