using System;
using UnityEngine;
[CreateAssetMenu(menuName = "Events/BrewingRequested")]
public class BrewingRequestedEvent : ScriptableObject
{
    public event Action<DrinkData> Raised;

    public void Raise(DrinkData data)
    {
        Raised?.Invoke(data);
    }
}
