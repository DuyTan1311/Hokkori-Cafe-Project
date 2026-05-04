using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/DecorPlaced")]
public class DecorPlacedEvent : ScriptableObject
{
    public event Action<DecorPlacementData> Raised;

    public void Raise(DecorPlacementData data)
    {
        Raised?.Invoke(data);
    }
}
