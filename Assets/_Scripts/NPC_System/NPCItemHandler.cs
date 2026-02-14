using UnityEngine;

public class NPCItemHandler : MonoBehaviour, IItemReceiver
{
    private NPCController controller;
    private NPCOrderHandler orderHandler;

    private void Awake()
    {
        controller = GetComponent<NPCController>();
        orderHandler = GetComponent<NPCOrderHandler>();
    }

    public bool CanReceiveItem(ItemData item)
    {
        if(controller == null)
        {
            return false;
        }
        if(controller.stateMachine.currentState != NPCState.WaitingForDrink)
        {
            return false;
        }
        return item is DrinkData;
    }

    public void ReceiveItem(ItemData item)
    {
        if (!CanReceiveItem(item))
        {
            return;
        }

        var drink = item as DrinkData;

        if (orderHandler.CheckOrder(drink))
        {
            controller.ChangeState(NPCState.GotCorrectDrink);
        }
        else
        {
            controller.ChangeState(NPCState.GotWrongDrink);
        }
    }
}
