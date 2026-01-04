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
        if (inventory.TryAdd(item))
        {
            Debug.Log("Added item " + item.itemName + " to inventory.");
            return true;
        }
        return false;
    }
}
