using UnityEngine;
using System.Collections;

public class BrewingSystem : MonoBehaviour
{
    [SerializeField] BrewingRequestedEvent OnBrewingRequested;
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

    void HandleBrewing(DrinkData drinkData)
    {
        if (isBrewing)
        {
            Debug.Log("Already brewing!");
            return;
        }

        Debug.Log("Brewing started: " + drinkData.drinkName);

        brewingRoutine = StartCoroutine(BrewRoutine(drinkData));
    }

   
    IEnumerator BrewRoutine(DrinkData drinkData)
    {
        isBrewing = true;

        float remainingTime = drinkData.brewTime;

        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            yield return null;
        }
        Debug.Log("Brewing completed: " + drinkData.drinkName);

        isBrewing = false;
        brewingRoutine = null;
    }
}
