using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool canInteract = true;

    public bool IsInteractable()
    {
        return canInteract;
    }
    
    public void Interact()
    {
        if (IsInteractable())
        {
            Debug.Log("Interacted with " +  gameObject.name);
        }
        else
        {
            Debug.Log("Can't interact with " + gameObject.name);
        }
    }
}
