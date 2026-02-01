using UnityEngine;
using System;

public class NPCController : MonoBehaviour, IItemReceiver
{
    [SerializeField] NPCCreatedOrderEvent OnNPCOrderCreated;
    public NPCOrderData currentOrder;
    public NPCStateMachine stateMachine;

    private NPCBehavior behavior;
    private Interactable interactable;
    private NPCPatienceController patienceController;
    private NPCOrderHandler orderHandler;

    private void Awake()
    {
        stateMachine = new NPCStateMachine();

        behavior = GetComponent<NPCBehavior>();
        interactable = GetComponent<Interactable>();
        patienceController = GetComponent<NPCPatienceController>();
        orderHandler = GetComponent<NPCOrderHandler>();

        // initialize patience controller
        behavior.InitializedPatienceController(patienceController);

        stateMachine.OnStateChanged += behavior.HandleStateChanged;
    }

    private void OnEnable()
    {
        // Generate order and change state
        GenerateOrder();
        stateMachine.ChangeState(NPCState.WaitingForOrderAccept);

        // subscribe to event
        interactable.OnInteracted += AcceptOrder;
        patienceController.OnPatienceExpired += Leave;
    }

    private void OnDisable()
    {
        // unsubscribe to event
        interactable.OnInteracted -= AcceptOrder;
        patienceController.OnPatienceExpired -= Leave;
    }
    #region Order Behavior
    void GenerateOrder()
    {
        currentOrder = orderHandler.CreateOrder();
        Debug.Log("NPC order is: " + currentOrder.requestedDrink);
        OnNPCOrderCreated.Raise(this, currentOrder);
    }

    public void AcceptOrder()
    {
        if(stateMachine.currentState == NPCState.WaitingForOrderAccept)
        {
            stateMachine.ChangeState(NPCState.WaitingForDrink);
            return;
        }
    }

    public bool CanReceiveItem(ItemData item)
    {
        if(stateMachine.currentState != NPCState.WaitingForDrink)
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

        DrinkData drink = (DrinkData)item;

        if(orderHandler.CheckOrder(drink))
        {
            stateMachine.ChangeState(NPCState.GotCorrectDrink);
        }
        else
        {
            stateMachine.ChangeState(NPCState.GotWrongDrink);
        }
    }
    #endregion
    public void Leave()
    {
        stateMachine.ChangeState(NPCState.Leaving);
    }
    
    
}
