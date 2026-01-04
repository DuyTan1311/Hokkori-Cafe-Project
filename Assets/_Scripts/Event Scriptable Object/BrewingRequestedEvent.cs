using System;
using UnityEngine;
[CreateAssetMenu(menuName = "Events/BrewingRequested")]
public class BrewingRequestedEvent : ScriptableObject
{
    public event Action<BrewingMachine,DrinkData> Raised;

    public void Raise(BrewingMachine machine,DrinkData data)
    {
        Raised?.Invoke(machine, data);
    }
}
