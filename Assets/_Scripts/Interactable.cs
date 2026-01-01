using UnityEngine;
using UnityEngine.Events;
using System;

public class Interactable : MonoBehaviour
{
    public event Action OnInteracted;
    public bool canInteract = true;

    public bool IsInteractable()
    {
        return canInteract;
    }
    
    public void Interact()
    {
        if (IsInteractable())
        {
            Debug.Log("Interacted with " + gameObject.name);
            OnInteracted?.Invoke();
        }
    }
}
