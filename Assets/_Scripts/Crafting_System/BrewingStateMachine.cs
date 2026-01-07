using UnityEngine;

public class BrewingStateMachine
{
    public BrewingState currentState { get; private set; }

    public event System.Action<BrewingState> OnStateChanged;

    public void ChangeState(BrewingState state)
    {
        if(currentState == state)
        {
            return;
        }
        currentState = state;
        Debug.Log("State changed to " +  currentState);
        OnStateChanged?.Invoke(currentState);
    }
}
