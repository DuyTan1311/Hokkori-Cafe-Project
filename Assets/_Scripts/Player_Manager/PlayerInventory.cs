using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private Inventory inventory;

    public event Action<ItemData> OnHandItemChanged;

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
        if (item == null)
        {
            return false;
        }

        ItemData previousHandItem = PeekHand();
        bool success = inventory.TryAdd(item);

        if (!success)
        {
            return false;
        }

        ItemData currentHandItem = PeekHand();

        if(currentHandItem != previousHandItem)
        {
            NotifyHandChanged(currentHandItem);
        }
        return true;
    }

    public bool SwapItem()
    {
        bool success = inventory.TrySwap(InventorySlotType.Hand, InventorySlotType.Bag);
        if (success)
        {
            NotifyHandChanged();
        }
        return success;
    }

    public ItemData GiveItem()
    {
        ItemData removed = inventory.RemoveFrom(InventorySlotType.Hand);
        if(removed != null)
        {
            NotifyHandChanged();
        }
        return removed;
    }

    public ItemData PeekHand()
    {
        return inventory.GetItem(InventorySlotType.Hand);
    }

    private void NotifyHandChanged()
    {
        OnHandItemChanged?.Invoke(PeekHand());
    }

    private void NotifyHandChanged(ItemData hand)
    {
        OnHandItemChanged?.Invoke(hand);
    }
}
