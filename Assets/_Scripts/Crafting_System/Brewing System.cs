using UnityEngine;
using System.Collections;

public class BrewingSystem : MonoBehaviour
{
    [SerializeField] BrewingRequestedEvent OnBrewingRequested;
    [SerializeField] BrewingCompletedEvent OnBrewingCompleted;

    private Coroutine brewingRoutine;
    private bool isBrewing;
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
        if (isBrewing)
        {
            Debug.Log("Already brewing!");
            return;
        }

        Debug.Log("Brewing started: " + drinkData.itemName);

        brewingRoutine = StartCoroutine(BrewRoutine(machine, drinkData));
    }

   
    IEnumerator BrewRoutine(BrewingMachine machine, DrinkData drinkData)
    {
        isBrewing = true;

        float remainingTime = drinkData.brewTime;

        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            yield return null;
        }
        Debug.Log("Brewing completed: " + drinkData.itemName);
        OnBrewingCompleted.Raise(machine, drinkData);

        isBrewing = false;
        brewingRoutine = null;
    }
}
