using UnityEngine;
using System;
[CreateAssetMenu(menuName = "Events/OnNPCSpawned")]
public class NPCSpawnEvent : ScriptableObject
{
    public event Action<NPCController> Raised;

    public void Raise(NPCController controller)
    {
        Raised?.Invoke(controller);
    }
}
