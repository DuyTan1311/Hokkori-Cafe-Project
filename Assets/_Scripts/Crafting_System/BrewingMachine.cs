using UnityEngine;

public class BrewingMachine : MonoBehaviour
{
    [SerializeField] BrewingRequestedEvent OnBrewingRequested;
    [SerializeField] PlayerInventory inventory;

    Interactable interactable;
    BrewingStateMachine stateMachine;

    BrewingRequest pendingResult;

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

    void CollectBrew()
    {

        if (inventory.ReceiveItem(pendingResult.baseDrink))
        {
            stateMachine.ChangeState(BrewingState.Idle);
        }
        
    }

    void HandleInteract()
    {
        if (stateMachine.currentState == BrewingState.Idle)
        {
            OpenBrewingUI();
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

    public void SubmitRequest(BrewingRequest request)
    {
        stateMachine.ChangeState(BrewingState.Brewing);
        OnBrewingRequested.Raise(this, request);
    }

    public void NotifyBrewCompleted(BrewingRequest result)
    {
        pendingResult = result;
        stateMachine.ChangeState(BrewingState.Completed);
    }

    void OpenBrewingUI()
    {
        // UI system will call SubmitRequest
    }

    public void SetState(BrewingState newState)
    {
        stateMachine.ChangeState(newState);
    }
}
