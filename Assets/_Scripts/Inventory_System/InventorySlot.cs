using UnityEngine;

public class InventorySlot
{
    public ItemData item { get; private set; }
    public InventorySlotType slotType { get; }

    public bool isEmpty => item == null; // when read, check if item is null and return to the bool variable

    public InventorySlot(InventorySlotType SlotType)
    {
        // when construct a new Inventory Slot, item can be null, but slot type has to be set
        slotType = SlotType;
    }

    public bool Set(ItemData itemData)
    {
        if (!isEmpty || itemData == null)
        {
            return false;
        }
        item = itemData;
        return true;
    }

    public ItemData Clear()
    {
        ItemData old = item;
        item = null;
        return old;
    }
}
