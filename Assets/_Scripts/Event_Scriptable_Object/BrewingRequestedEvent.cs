using System;
using UnityEngine;
[CreateAssetMenu(menuName = "Events/BrewingRequested")]
public class BrewingRequestedEvent : ScriptableObject
{
    public event Action<BrewingMachine,BrewingRequest> Raised;

    public void Raise(BrewingMachine machine,BrewingRequest request)
    {
        Raised?.Invoke(machine, request);
    }
}
