using UnityEngine;

public class BrewingMachine : MonoBehaviour
{
    [SerializeField] DrinkData drinkData;

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
        GameEventSystem.instance.RaiseBrewingRequested(drinkData);
        Debug.Log("Coffee Machine brew requested: " + drinkData.drinkName);
    }
}
