using UnityEngine;
using System;
[CreateAssetMenu(menuName = "Events/BrewingCompleted")]
public class BrewingCompletedEvent : ScriptableObject
{
    public event Action<BrewingMachine, DrinkData> Raised;

    public void Raise(BrewingMachine machine, DrinkData data)
    {
        Raised?.Invoke(machine, data);
    }
}
