using UnityEngine;
using System;

public class GameEventSystem : MonoBehaviour
{
    public static GameEventSystem instance;

    public static event Action OnEventSystemReady;

    public event Action<DrinkData> OnBrewingRequested;
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

    public void RaiseBrewingRequested(DrinkData data)
    {
        OnBrewingRequested?.Invoke(data);
    }

    public void RaiseNPCOrderCreated(NPCController npc, NPCOrderData data)
    {
        OnNPCOrderCreated?.Invoke(npc, data);
    }
}
