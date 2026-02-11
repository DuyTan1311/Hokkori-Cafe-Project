using UnityEngine;

public enum NPCState 
{
    None,
    WalkingToSeat,
    WaitingForOrderAccept,
    WaitingForDrink,
    GotCorrectDrink,
    Drinking,
    GotWrongDrink,
    Leaving
}
