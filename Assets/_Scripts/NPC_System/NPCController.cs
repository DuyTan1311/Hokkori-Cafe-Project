using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] NPCCreatedOrderEvent OnNPCOrderCreated;
    public NPCOrderData currentOrder;
    public NPCStateMachine stateMachine;

    private NPCBehavior behavior;
    private Interactable interactable;
    private NPCPatienceController patienceController;

    private void Awake()
    {
        stateMachine = new NPCStateMachine();

        behavior = GetComponent<NPCBehavior>();
        interactable = GetComponent<Interactable>();
        patienceController = GetComponent<NPCPatienceController>();

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

    void GenerateOrder()
    {
        currentOrder = OrderGenerator.Generate();
        OnNPCOrderCreated.Raise(this, currentOrder);
    }

    public void AcceptOrder()
    {
        stateMachine.ChangeState(NPCState.WaitingForDrink);
    }

    public void ReceiveDrink(DrinkData drink)
    {
        if(drink == currentOrder.requestedDrink)
        {
            stateMachine.ChangeState(NPCState.GotCorrectDrink);
        }
        else
        {
            stateMachine.ChangeState(NPCState.GotWrongDrink);
        }
    }

    public void Leave()
    {
        stateMachine.ChangeState(NPCState.Leaving);
    }
}
