using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private Inventory inventory;

    private void Awake()
    {
        inventory = new Inventory(new[]
        {
            InventorySlotType.Hand,
            InventorySlotType.Bag
        });
    }

    public bool ReceiveItem(ItemData item)
    {
        return inventory.TryAdd(item);
    }

    public bool SwapItem()
    {
        return inventory.TrySwap(InventorySlotType.Hand, InventorySlotType.Bag);
    }

    public ItemData giveItem()
    {
        return inventory.RemoveFrom(InventorySlotType.Hand);
    }
}
