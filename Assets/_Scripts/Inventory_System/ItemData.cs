using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    public string itemName;
    public string itemID;
    public Sprite icon;

    [Header("Item Flags")]
    public bool canBeHeld = true;
    public bool canBeStored = true;

    [Header("Visual")]
    public string poolKey; // key used for spawn system
}
