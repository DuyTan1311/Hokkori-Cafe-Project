using UnityEngine;

public class BrewingSystem : MonoBehaviour
{
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
        Debug.Log("Brewing started: " + drinkData.drinkName);
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
}
