using UnityEngine;
using System;
using System.Collections.Generic;

public class Inventory 
{
    #region Fields and Constructor
    private List<InventorySlot> slots = new List<InventorySlot>();

    // use interface, only allow other script to read the list and can't add or remove
    // "=>" mean whenever a script read the list, it will return the private list
    public IReadOnlyList<InventorySlot> InventorySlots => slots;

    // constructor for inventory
    // IEnumerable<InventorySlotType> means anything that can be foreach and has InventorySlotType data type
    public Inventory(IEnumerable<InventorySlotType> slotTypes)
    {
        // go through every slotTypes with InventorySlotType data type
        foreach(var type in slotTypes)
        {
            // add a new Inventory Slot with a slot type 
            slots.Add(new InventorySlot(type));
        }
    }
    #endregion

    #region Query
    // get an inventory slot from the list to use
    public InventorySlot GetSlot(InventorySlotType type)
    {
        // check every slot and return the first slot that has the same type of input
        // if not, return null
        return slots.Find(s => s.slotType == type);
    }

    // check if an item exists in the inventory
    public bool HasItem(ItemData item)
    {
        // Exists return bool
        return slots.Exists(s => s.item == item);
    }
    #endregion

    #region Inventory Operations
    // try add an item to a slot with a slot type
    public bool TryAddToSlot(InventorySlotType type, ItemData item)
    {
        // get reference to the slot
        InventorySlot slot = GetSlot(type);
        if (slot == null)
        {
            return false;
        }
        // this will change the data in the inventory slot object
        return slot.TrySet(item);
    }

    // try add an item to inventory
    public bool TryAdd(ItemData item)
    {
        if (item == null)
        {
            return false;
        }
        if (TryAddToSlot(InventorySlotType.Hand, item))
        {
            return true;
        }
        if(TryAddToSlot(InventorySlotType.Bag, item))
        {
            return true;
        }
        return false;
    }

    // try move an item to another slot
    public bool TryMove(InventorySlotType from, InventorySlotType to)
    {
        InventorySlot fromSlot = GetSlot(from);
        InventorySlot toSlot = GetSlot(to);

        // if one of the slots is null (doesn't exist in inventory), return false
        if(fromSlot == null || toSlot == null)
        {
            return false;
        }
        // if the first slot is empty, or the second slot has item, return false
        if(fromSlot.isEmpty || !toSlot.isEmpty)
        {
            return false;
        }
        // take item from the old slot and set to the new slot
        ItemData item = fromSlot.Clear();
        return toSlot.TrySet(item);
    }

    // swap item between hand and bag
    public bool TrySwap(InventorySlotType from, InventorySlotType to)
    {
        InventorySlot fromSlot = GetSlot(from);
        InventorySlot toSlot = GetSlot(to);

        // if one of the slots is null (doesn't exist in inventory), return false
        if (fromSlot == null || toSlot == null)
        {
            return false;
        }
        // if both slots is empty, return false
        if(fromSlot.isEmpty && toSlot.isEmpty)
        {
            return false;
        }
        else
        {
            if(!fromSlot.isEmpty && toSlot.isEmpty)
            {
                ItemData item = fromSlot.Clear();
                return toSlot.TrySet(item);
            }
            else if(fromSlot.isEmpty && !toSlot.isEmpty)
            {
                ItemData item =toSlot.Clear();
                return fromSlot.TrySet(item);
            }
            else
            {
                ItemData itemFrom=fromSlot.Clear();
                ItemData itemTo=toSlot.Clear();
                fromSlot.TrySet(itemTo);
                toSlot.TrySet(itemFrom);
                return true;
            }
        }
    }

    // remove item from slot
    public ItemData RemoveFrom(InventorySlotType type)
    {
        InventorySlot slot = GetSlot(type);
        if(slot == null || slot.isEmpty)
        {
            return null;
        }
        return slot.Clear();
    }
    #endregion
}
