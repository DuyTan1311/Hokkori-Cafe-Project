using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    Interactable currentInteractable;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Interactable interactable))
        {
            currentInteractable = interactable;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out Interactable interactable))
        {
            if(currentInteractable == interactable)
            {
                currentInteractable = null;
            }
        }
    }

    public void TryInteract()
    {
        if(currentInteractable == null)
        {
            return;
        }
        currentInteractable.Interact();
    }

}
