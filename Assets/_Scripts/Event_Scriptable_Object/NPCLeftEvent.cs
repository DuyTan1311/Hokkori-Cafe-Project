using UnityEngine;
using System;
[CreateAssetMenu(menuName = "Events/OnNPCLeft")]
public class NPCLeftEvent : ScriptableObject
{
    public event Action<NPCController> Raised;

    public void Raise(NPCController controller)
    {
        Raised?.Invoke(controller);
    }
}
