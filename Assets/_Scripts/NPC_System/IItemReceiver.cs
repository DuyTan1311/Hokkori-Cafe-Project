using UnityEngine;

public interface IItemReceiver
{
    bool CanReceiveItem(ItemData item);
    void ReceiveItem(ItemData item);
}
