using UnityEngine;

public class NPCOrderHandler : MonoBehaviour
{
    public NPCOrderData currentOrder { get; private set; }

    public NPCOrderData CreateOrder()
    {
        return currentOrder = OrderGenerator.Generate();
    }

}
