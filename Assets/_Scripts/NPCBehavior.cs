using UnityEngine;
using System.Collections;
using System;

public class NPCBehavior : MonoBehaviour
{
    Coroutine waitCoroutine;

    public event Action OnNPCLeaving;

    public void HandleStateChanged(NPCState state, NPCOrderData orderData)
    {
        StopAllCoroutines();

        switch (state)
        {
            case NPCState.WaitingForOrderAccept:
                Debug.Log("Waiting for order: " + orderData.requestedDrink);
                break;

            case NPCState.WaitingForDrink:
                waitCoroutine = StartCoroutine(WaitForDrink(orderData));
                OnNPCLeaving?.Invoke();
                break;

            case NPCState.GotCorrectDrink:
                Debug.Log("Thank you so much");
                break;

            case NPCState.GotWrongDrink:
                Debug.Log("This is not what I wanted");
                break;

            case NPCState.Leaving:
                Debug.Log("NPC is leaving...");
                break;
        }
    }

    IEnumerator WaitForDrink(NPCOrderData orderData)
    {
        float timer = 0;
        while (timer < orderData.waitTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        
    }
}
