using UnityEngine;

public class NPCStateMachine 
{
    public NPCState currentState { get; private set; }

    public NPCOrderData currentOrder { get; private set; }

    public event System.Action<NPCState, NPCOrderData> OnStateChanged;

    public void ChangeState(NPCState newState)
    {
        if(currentState == newState)
        {
            return;
        }
        currentState = newState;
        OnStateChanged?.Invoke(currentState, currentOrder);
    }
}
