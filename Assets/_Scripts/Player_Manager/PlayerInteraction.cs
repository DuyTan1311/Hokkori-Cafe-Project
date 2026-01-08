using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    
    List<Interactable> interactablesInRange = new List<Interactable>();

    PlayerInventory inventory;

    private void Awake()
    {
        inventory = GetComponent<PlayerInventory>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.TryGetComponent(out Interactable interactable))
        {
            
            if (!interactablesInRange.Contains(interactable))
            {
                
                interactablesInRange.Add(interactable);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.TryGetComponent(out Interactable interactable))
        {
            
            interactablesInRange.Remove(interactable);
        }
    }

    public void TryInteract()
    {
        // if there's no interactable object in the list, return
        if (interactablesInRange.Count == 0)
        {
            return;
        }
        Interactable closest = null;
        float closestDistance = float.MaxValue;
        Vector3 playerPos = transform.position;

        foreach (var interactable in interactablesInRange)
        {

            if (interactable == null || !interactable.IsInteractable())
            {
                continue;
            }
            // calculate squared distance for faster performance
            float distance = (interactable.transform.position - playerPos).sqrMagnitude;

            if (distance < closestDistance)
            {
                closestDistance = distance;

                closest = interactable;
            }
        }
        // if there's a closest interactable object, interact with it
        if (closest != null)
        {
            if (TryGiveItem(closest))
            {
                return;
            }
            closest.Interact();
        }
    }

    public bool TryGiveItem(Interactable currentTarget)
    {
        if(currentTarget.TryGetComponent<IItemReceiver>(out var receiver))
        {
            ItemData handItem = inventory.PeekHand();
            if(handItem != null && receiver.CanReceiveItem(handItem))
            {
                receiver.ReceiveItem(inventory.GiveItem());
                return true;
            }
        }
        return false;
    }
}
