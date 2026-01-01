using UnityEngine;
using System.Collections;
using System;

public class NPCBehavior : MonoBehaviour
{
    NPCPatienceController patienceController;

    // let NPC Controller initialize to avoid timing error
    public void InitializedPatienceController(NPCPatienceController patience)
    {
        patienceController = patience;
    }

    public void HandleStateChanged(NPCState state)
    {
        patienceController.StopWaiting();

        switch (state)
        {
            case NPCState.WaitingForOrderAccept:
                Debug.Log("Waiting for order accept");
                break;

            case NPCState.WaitingForDrink:
                Debug.Log("NPC is waiting for drink");
                patienceController.StartWaiting();
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

    
}
