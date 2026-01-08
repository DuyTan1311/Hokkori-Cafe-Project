using UnityEngine;
using System;

public class NPCOrderHandler : MonoBehaviour
{
    public NPCOrderData currentOrder { get; private set; }

    public event Action OnNPCSatisfied;
    public event Action OnNPCUnSatisfied;

    public NPCOrderData CreateOrder()
    {
        return currentOrder = OrderGenerator.Generate();
    }

    public bool CheckOrder(DrinkData drink)
    {
        if(currentOrder.requestedDrink == drink)
        {
            OnNPCSatisfied?.Invoke();
            return true;
        }
        OnNPCUnSatisfied?.Invoke();
        return false;
    }
}
