using System;
using System.Collections;
using UnityEngine;

public class DayCycleManager : MonoBehaviour
{
    [SerializeField] private DayCycleConfig config;
    [SerializeField] private NPCLeftEvent onNPCLeft;
    [SerializeField] private Interactable bellInteractable;
    [SerializeField] private Interactable bedInteractable;

    public DayData DayData {get; private set; } = new DayData();
    public event Action<DayPhase> OnPhaseChanged;

    private int activeNPCCount;
    private Coroutine openingTimer;

    private void OnEnable()
    {
        onNPCLeft.Raised += HandleNPCLeft;
        bellInteractable.OnInteracted += HandleBellInteracted;
        bedInteractable.OnInteracted += HandleBedInteracted;
    }

    private void OnDisable()
    {
        onNPCLeft.Raised -= HandleNPCLeft;
        bellInteractable.OnInteracted -= HandleBellInteracted;
        bedInteractable.OnInteracted -= HandleBedInteracted;
    }

    private void Start()
    {
        Debug.Log("Today is day " + DayData.currentDay);
        EnterPhase(DayPhase.Preparation);
    }

    // Phase transitions

    private void EnterPhase(DayPhase phase)
    {
        DayData.SetPhase(phase);
        UpdateInteractables(phase);
        OnPhaseChanged?.Invoke(phase);

        if(phase == DayPhase.Opening)
        {
            openingTimer = StartCoroutine(OpeningRoutine());
        }
    }

    private IEnumerator OpeningRoutine()
    {
        yield return new WaitForSeconds(config.dayDuration);
        EnterPhase(DayPhase.Closing);
    }

    // Bell: preparation -> opening
    private void HandleBellInteracted()
    {
        if(DayData.currentPhase != DayPhase.Preparation)
        {
            return;
        }
        activeNPCCount = 0;
        EnterPhase(DayPhase.Opening);
    }

    // closing -> night: khi npc = 0, unlock bed để interact

    private void HandleNPCLeft(NPCController _)
    {
        activeNPCCount = Mathf.Max(0, activeNPCCount - 1);
        if(DayData.currentPhase == DayPhase.Closing && activeNPCCount <= 0)
        {
            bedInteractable.canInteract = true;
        }
    }

    // bed: night -> preparation (next day)
    private void HandleBedInteracted()
    {
        if(DayData.currentPhase != DayPhase.Night)
        {
            return;
        }
        DayData.NextDay();
        Debug.Log("Today is day " + DayData.currentDay);
        EnterPhase(DayPhase.Preparation);
    }

    public void RegisterNPCSpawn() => activeNPCCount++;

    private void UpdateInteractables(DayPhase phase)
    {
        bellInteractable.canInteract = phase == DayPhase.Preparation;
        bedInteractable.canInteract = phase == DayPhase.Night;
    }
}
