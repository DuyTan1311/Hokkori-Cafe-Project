using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/DecorRemoved")]
public class DecorRemovedEvent : ScriptableObject
{
    public event Action<DecorPlacementData> Raised;
    public void Raise(DecorPlacementData data)
    {
        Raised?.Invoke(data);
    }
}
