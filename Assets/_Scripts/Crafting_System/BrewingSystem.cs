using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BrewingSystem : MonoBehaviour
{
    [SerializeField] BrewingRequestedEvent OnBrewingRequested;
    [SerializeField] BrewingCompletedEvent OnBrewingCompleted;

    private Dictionary<BrewingMachine, Coroutine> brewingRoutines = new Dictionary<BrewingMachine, Coroutine>();

    private void OnEnable()
    {
        OnBrewingRequested.Raised += HandleBrewing;
    }
    private void OnDisable()
    {
        OnBrewingRequested.Raised -= HandleBrewing;
    }

    void HandleBrewing(BrewingMachine machine, BrewingRequest request)
    {
        if (brewingRoutines.ContainsKey(machine))
        {
            return;
        }   
        machine.SetState(BrewingState.Brewing);

        Coroutine routine = StartCoroutine(BrewRoutine(machine, request));
        brewingRoutines[machine]= routine;
    }

   
    IEnumerator BrewRoutine(BrewingMachine machine, BrewingRequest request)
    {
        float remainingTime = request.baseDrink.brewTime;

        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            yield return null;
        }
        Debug.Log("Brewing completed: " + request.baseDrink.itemName);

        machine.NotifyBrewCompleted(request);
        OnBrewingCompleted.Raise(machine, request);
        brewingRoutines.Remove(machine);
    }
}
