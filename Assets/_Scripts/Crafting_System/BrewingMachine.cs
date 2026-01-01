using UnityEngine;

public class BrewingMachine : MonoBehaviour
{
    [SerializeField] DrinkData drinkData;
    [SerializeField] BrewingRequestedEvent OnBrewingRequested;

    Interactable interactable;

    private void Awake()
    {
        interactable = GetComponent<Interactable>();
    }

    private void OnEnable()
    {
        interactable.OnInteracted += RequestBrew;
    }

    private void OnDisable()
    {
        interactable.OnInteracted -= RequestBrew;
    }

    void RequestBrew()
    {
        OnBrewingRequested.Raise(drinkData);
        Debug.Log("Coffee Machine brew requested: " + drinkData.drinkName);
    }
}
