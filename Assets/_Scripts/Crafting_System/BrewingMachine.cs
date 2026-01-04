using UnityEngine;

public class BrewingMachine : MonoBehaviour
{
    [SerializeField] DrinkData drinkData;
    [SerializeField] BrewingRequestedEvent OnBrewingRequested;
    [SerializeField] BrewingCompletedEvent OnBrewingCompleted;
    [SerializeField] PlayerInventory inventory;

    Interactable interactable;

    private bool readyToCollect = false;
    private bool isBrewing = false;

    private void Awake()
    {
        interactable = GetComponent<Interactable>();
    }

    private void OnEnable()
    {
        interactable.OnInteracted += HandleInteract;
        OnBrewingCompleted.Raised += BrewReady;
    }

    private void OnDisable()
    {
        interactable.OnInteracted -= HandleInteract;
        OnBrewingCompleted.Raised -= BrewReady;
    }

    void RequestBrew()
    {
        OnBrewingRequested.Raise(this, drinkData);
        isBrewing = true;
        Debug.Log("Coffee Machine brew requested: " + drinkData.itemName);
    }

    void BrewReady(BrewingMachine machine, DrinkData completedDrink)
    {
        
        if(machine == this)
        {
            readyToCollect = true;
            isBrewing = false;
        }
    }

    void CollectBrew()
    {

        if (inventory.ReceiveItem(drinkData))
        {
            readyToCollect = false;
        }
        
    }

    void HandleInteract()
    {
        if (!readyToCollect && !isBrewing)
        {
            RequestBrew();
        }
        else if (readyToCollect && !isBrewing)
        {
            CollectBrew();
        }
        else
        {
            // brewing in progress, do nothing
        }
    }
}
