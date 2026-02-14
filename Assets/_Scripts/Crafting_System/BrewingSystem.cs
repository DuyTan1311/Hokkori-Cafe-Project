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

    void HandleBrewing(BrewingMachine machine, DrinkData drinkData)
    {
        if (brewingRoutines.ContainsKey(machine))
        {
            return;
        }   
        machine.SetState(BrewingState.Brewing);

        Coroutine routine = StartCoroutine(BrewRoutine(machine, drinkData));
        brewingRoutines[machine]= routine;
    }

   
    IEnumerator BrewRoutine(BrewingMachine machine, DrinkData drinkData)
    {
        float remainingTime = drinkData.brewTime;

        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            yield return null;
        }
        Debug.Log("Brewing completed: " + drinkData.itemName);
        OnBrewingCompleted.Raise(machine, drinkData);

        machine.SetState(BrewingState.Completed);
        brewingRoutines.Remove(machine);
    }
}
