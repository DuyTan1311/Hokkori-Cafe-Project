using UnityEngine;
using System;

public class NPCController : MonoBehaviour, IPoolable
{
    [SerializeField] NPCCreatedOrderEvent OnNPCOrderCreated;
    [SerializeField] NPCSpawnEvent OnNPCSpawned;
    [SerializeField] NPCLeftEvent OnNPCLeft;
    

    public NPCOrderData currentOrder;
    public NPCStateMachine stateMachine;

    private NPCBehavior behavior;
    private Interactable interactable;
    private NPCPatienceController patienceController;
    private NPCOrderHandler orderHandler;

    public string Poolkey { get; private set; }

    public void Init(string poolKey)
    {
        Poolkey = poolKey;
    }

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

    #region Order Behavior
    public void GenerateOrder()
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

    
    #endregion
    public void Leave()
    {
        stateMachine.ChangeState(NPCState.Leaving);
    }
    
    public void OnSpawn()
    {
        stateMachine.Reset(); 

        interactable.OnInteracted += AcceptOrder;
        patienceController.OnPatienceExpired += Leave;

        OnNPCSpawned.Raise(this);
    }

    public void OnDespawn()
    {
        interactable.OnInteracted -= AcceptOrder;
        patienceController.OnPatienceExpired -= Leave;

        currentOrder = null;
        patienceController.StopWaiting();
        stateMachine.Reset();
    }

    public void ChangeState(NPCState newState)
    {
        stateMachine.ChangeState(newState);
    }

    public void StartBehavior()
    {
        stateMachine.ChangeState(NPCState.WalkingToSeat);
    }

    public void NotifyExit()
    {
        OnNPCLeft.Raise(this);
    }
}
