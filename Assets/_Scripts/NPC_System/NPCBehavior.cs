using UnityEngine;
using System.Collections;
using System;

public class NPCBehavior : MonoBehaviour
{
    private SeatManager seatManager;
    private Transform exitPoint;
    [SerializeField] private NPCProfile profile;

    private NPCMovement movement;
    private NPCController controller;
    private NPCPatienceController patienceController;

    private Seat currentSeat;
    private Coroutine moveRoutine;
    private Coroutine drinkRoutine;

    private void Awake()
    {
        movement = GetComponent<NPCMovement>();
        controller = GetComponent<NPCController>();
    }

    // initializer
    public void Init(SeatManager manager, Transform exit)
    {
        seatManager = manager;
        exitPoint = exit;
    }

    public void InitializedPatienceController(NPCPatienceController patience)
    {
        patienceController = patience;
    }

    public void HandleStateChanged(NPCState state)
    {
        patienceController.StopWaiting();

        switch (state)
        {
            case NPCState.WalkingToSeat:
                MoveToSeat();
                break;

            case NPCState.WaitingForOrderAccept:
                controller.GenerateOrder();
                Debug.Log("Waiting for order accept");
                break;

            case NPCState.WaitingForDrink:
                Debug.Log("NPC is waiting for drink");
                patienceController.StartWaiting();
                break;

            case NPCState.GotCorrectDrink:
                StartDrinking();
                Debug.Log("Thank you so much");
                break;

            case NPCState.GotWrongDrink:
                StartDrinking();
                Debug.Log("This is not what I wanted");
                break;

            case NPCState.Leaving:
                LeaveSeatAndExit();
                Debug.Log("NPC is leaving...");
                break;
        }
    }

    void MoveToSeat()
    {
        currentSeat = seatManager.GetRandomEmptySeat();

        if(currentSeat == null)
        {
            controller.Leave();
            return;
        }

        currentSeat.Occupy();
        movement.MoveTo(currentSeat.GetSitPosition());

        if(moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
        }

        moveRoutine = StartCoroutine(CheckReachedSeat());
    }

    IEnumerator CheckReachedSeat()
    {
        while (!movement.HasReached())
        {
            yield return null;
        }

        movement.SitDown();
        controller.ChangeState(NPCState.WaitingForOrderAccept);
    }

    void StartDrinking()
    {
        if(drinkRoutine != null)
        {
            StopCoroutine(drinkRoutine);
        }

        drinkRoutine = StartCoroutine(DrinkRoutine());
    }

    IEnumerator DrinkRoutine()
    {
        yield return new WaitForSeconds(profile.drinkDuration);
        controller.Leave();
    }

    void LeaveSeatAndExit()
    {
        if(currentSeat != null)
        {
            currentSeat.Release();
            currentSeat = null;
        }

        movement.MoveTo(exitPoint.position);

        if(moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
        }

        moveRoutine = StartCoroutine(CheckReachedExit());
    }

    IEnumerator CheckReachedExit()
    {
        while (!movement.HasReached())
        {
            yield return null;
        }

        controller.Leave();
    }
}
