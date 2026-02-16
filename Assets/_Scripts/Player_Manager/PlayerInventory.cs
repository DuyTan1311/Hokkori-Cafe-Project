using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private Inventory inventory;

    public event Action<ItemData> OnHandItemChanged;

    [SerializeField] private InventoryChangedEvent OnInventoryChanged;

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
        else
        {
            NotifyInventoryChanged();

            ItemData currentHandItem = PeekHand();

            if (currentHandItem != previousHandItem)
            {
                NotifyHandChanged(currentHandItem);
            }
        }
        return true;
    }

    public bool SwapItem()
    {
        bool success = inventory.TrySwap(InventorySlotType.Hand, InventorySlotType.Bag);
        if (success)
        {
            NotifyHandChanged();
            NotifyInventoryChanged();
        }
        return success;
    }

    public ItemData GiveItem()
    {
        ItemData removed = inventory.RemoveFrom(InventorySlotType.Hand);
        if(removed != null)
        {
            NotifyHandChanged();
            NotifyInventoryChanged();
        }
        return removed;
    }

    public ItemData PeekHand()
    {
        return inventory.GetItem(InventorySlotType.Hand);
    }

    public ItemData PeekBag()
    {
        return inventory.GetItem(InventorySlotType.Bag);
    }

    private void NotifyHandChanged()
    {
        OnHandItemChanged?.Invoke(PeekHand());
    }

    private void NotifyHandChanged(ItemData hand)
    {
        OnHandItemChanged?.Invoke(hand);
    }

    private void NotifyInventoryChanged()
    {
        OnInventoryChanged.Raise(PeekHand(), PeekBag());
    }
}
