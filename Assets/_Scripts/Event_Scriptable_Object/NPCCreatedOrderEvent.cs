using System;
using UnityEngine;
[CreateAssetMenu(menuName ="Events/NPC Order Requested")]
public class NPCCreatedOrderEvent : ScriptableObject
{
    public event Action<NPCController, NPCOrderData> Raised;

    public void Raise(NPCController npc, NPCOrderData data)
    {
        Raised?.Invoke(npc, data);
    }
}
