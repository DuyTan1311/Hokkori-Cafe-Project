using UnityEngine;

public class BrewingMachine : MonoBehaviour
{
    [SerializeField] DrinkData drinkData;
    [SerializeField] BrewingRequestedEvent OnBrewingRequested;
    [SerializeField] BrewingCompletedEvent OnBrewingCompleted;
    [SerializeField] PlayerInventory inventory;

    Interactable interactable;
    BrewingStateMachine stateMachine;

    // let brewingSystem change state but keep stateMachine private
    public BrewingState CurrentState => stateMachine.currentState;

    private void Awake()
    {
        interactable = GetComponent<Interactable>();
        stateMachine = new BrewingStateMachine();
    }

    private void OnEnable()
    {
        interactable.OnInteracted += HandleInteract;
    }

    private void OnDisable()
    {
        interactable.OnInteracted -= HandleInteract;
    }

    void RequestBrew()
    {
        OnBrewingRequested.Raise(this, drinkData);
        Debug.Log("Coffee Machine brew requested: " + drinkData.itemName);
    }

    void CollectBrew()
    {

        if (inventory.ReceiveItem(drinkData))
        {
            stateMachine.ChangeState(BrewingState.Idle);
        }
        
    }

    void HandleInteract()
    {
        if (stateMachine.currentState == BrewingState.Idle)
        {
            RequestBrew();
        }
        else if (stateMachine.currentState == BrewingState.Completed)
        {
            CollectBrew();
        }
        else
        {
            // brewing in progress, do nothing
        }
    }

    public void SetState(BrewingState newState)
    {
        stateMachine.ChangeState(newState);
    }
}
