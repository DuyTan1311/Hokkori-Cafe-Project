using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    // declare a list to store all the interactable object in range
    List<Interactable> interactablesInRange = new List<Interactable>();

    private void OnTriggerEnter(Collider other)
    {
        // check if the collided object has interactable component
        if (other.TryGetComponent(out Interactable interactable))
        {
            // check if the list has contained that object or not
            if (!interactablesInRange.Contains(interactable))
            {
                // if the collided object hasn't been added, add it to the list
                interactablesInRange.Add(interactable);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // check if the collided object has interactable component
        if (other.TryGetComponent(out Interactable interactable))
        {
            // remove the object from the list
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
        // declare a reference to store the closest interactable
        Interactable closest = null;
        // declare a closest distance variable as a large number for easier comparisons
        float closestDistance = float.MaxValue;
        // set player's current position
        Vector3 playerPos = transform.position;
        // check every object in the list
        foreach (var interactable in interactablesInRange)
        {
            // if there's an empty slot or the object is not interactable, skip
            if (interactable == null || !interactable.IsInteractable())
            {
                continue;
            }
            // calculate squared distance for faster performance
            float distance = (interactable.transform.position - playerPos).sqrMagnitude;
            // if the distance is smaller than closest distance, set it to the closest distance
            if (distance < closestDistance)
            {
                closestDistance = distance;
                // set the closest object
                closest = interactable;
            }
        }
        // if there's a closest interactable object, interact with it
        if (closest != null)
        {
            closest.Interact();
        }
    }
}
