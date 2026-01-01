using UnityEngine;
using System;

public class GameEventSystem : MonoBehaviour
{
    public static GameEventSystem instance;

    public static event Action OnEventSystemReady;

    
    public event Action<NPCController, NPCOrderData> OnNPCOrderCreated;

    public bool isReady { get; private set; }

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        isReady = true;

        OnEventSystemReady?.Invoke();
    }

    public void RaiseNPCOrderCreated(NPCController npc, NPCOrderData data)
    {
        OnNPCOrderCreated?.Invoke(npc, data);
    }
}
