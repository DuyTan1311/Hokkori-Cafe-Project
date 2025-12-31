using UnityEngine;

public class NPCController : MonoBehaviour
{
    public NPCOrderData currentOrder;

    public NPCStateMachine stateMachine;
    private NPCBehavior behavior;
    private Interactable interactable;

    private void Awake()
    {
        stateMachine = new NPCStateMachine();
        behavior = GetComponent<NPCBehavior>();
        interactable = GetComponent<Interactable>();

        stateMachine.OnStateChanged += behavior.HandleStateChanged;
    }

    private void OnEnable()
    {
        GenerateOrder();
        stateMachine.ChangeState(NPCState.WaitingForOrderAccept);
        interactable.OnInteracted += AcceptOrder;
    }

    void GenerateOrder()
    {
        currentOrder = OrderGenerator.Generate();
        GameEventSystem.instance.RaiseNPCOrderCreated(this, currentOrder);
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
