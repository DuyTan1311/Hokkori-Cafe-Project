using UnityEngine;
using System.Collections;

public class BrewingSystem : MonoBehaviour
{
    private Coroutine brewingRoutine;
    private bool isBrewing;
    private void OnEnable()
    {
        // subscribe only if event system has ready
        TrySubscribe();
        GameEventSystem.OnEventSystemReady += TrySubscribe;
    }
    private void OnDisable()
    {
        // if event system still alive, unsub OnBrewingRequested like normal
        if(GameEventSystem.instance != null)
        {
            GameEventSystem.instance.OnBrewingRequested -= HandleBrewing;
        }
        // if event system instance is destroyed, unsub TrySubscribe from it
        GameEventSystem.OnEventSystemReady -= TrySubscribe;
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

    void TrySubscribe()
    {
        if(GameEventSystem.instance == null)
        {
            return;
        }
        if (!GameEventSystem.instance.isReady)
        {
            return;
        }
        GameEventSystem.instance.OnBrewingRequested -= HandleBrewing; // prevent double subscribes
        GameEventSystem.instance.OnBrewingRequested += HandleBrewing;
    }
    // set cooldown for brewing
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
