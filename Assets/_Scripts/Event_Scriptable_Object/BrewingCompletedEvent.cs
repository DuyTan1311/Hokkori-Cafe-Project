using UnityEngine;
using System;
[CreateAssetMenu(menuName = "Events/BrewingCompleted")]
public class BrewingCompletedEvent : ScriptableObject
{
    public event Action<BrewingMachine, BrewingRequest> Raised;

    public void Raise(BrewingMachine machine, BrewingRequest request)
    {
        Raised?.Invoke(machine, request);
    }
}
