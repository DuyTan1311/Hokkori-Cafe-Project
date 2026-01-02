using UnityEngine;
using System;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
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

    public InventorySlot GetSlot(InventorySlotType type)
    {
        // check every slot and return the first slot that has the same type of input
        // if not, return null
        return slots.Find(s => s.slotType == type);
    }

    public bool HasItem(ItemData item)
    {
        // Exists return bool
        return slots.Exists(s => s.item == item);
    }

    
}
