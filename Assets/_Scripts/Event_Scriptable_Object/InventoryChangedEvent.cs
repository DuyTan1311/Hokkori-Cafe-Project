using System;
using UnityEngine;
[CreateAssetMenu(menuName = "Events/Inventory changed")]
public class InventoryChangedEvent : ScriptableObject
{
    public event Action<ItemData, ItemData> Raised;
    
    public void Raise(ItemData hand, ItemData bag)
    {
        Raised?.Invoke(hand, bag);
    }
}
