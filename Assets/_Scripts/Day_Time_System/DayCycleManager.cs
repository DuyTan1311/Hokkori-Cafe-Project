using System;
using System.Collections;
using UnityEngine;

public class DayCycleManager : MonoBehaviour, ISaveable
{
    [SerializeField] private DayCycleConfig config;
    [SerializeField] private NPCLeftEvent onNPCLeft;
    [SerializeField] private Interactable bellInteractable;
    [SerializeField] private Interactable bedInteractable;

    public DayData DayData {get; private set; } = new DayData();
    public event Action<DayPhase> OnPhaseChanged;

    private int activeNPCCount;
    private Coroutine openingTimer;
    private bool hasLoaded = false;

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
        if (!hasLoaded)
        {
            EnterPhase(DayPhase.Preparation); // nếu chưa load save thì enter phase preparation
        }
        Debug.Log("Today is day " + DayData.currentDay);
    }

    // Phase transitions

    private void EnterPhase(DayPhase phase)
    {
        if(openingTimer != null) // nếu có coroutine đang chạy thì dừng để chuyển phase
        {
            StopCoroutine(openingTimer);
            openingTimer = null;
        }

        DayData.SetPhase(phase);
        Debug.Log("Current phase: " + DayData.currentPhase);
        UpdateInteractables(phase); // update các object tương tác liên quan
        OnPhaseChanged?.Invoke(phase);

        if(phase == DayPhase.Opening)
        {
            openingTimer = StartCoroutine(OpeningRoutine()); // nếu là phase opening thì chạy coroutine
        }
    }

    private IEnumerator OpeningRoutine()
    {
        yield return new WaitForSeconds(config.dayDuration);
        EnterPhase(DayPhase.Closing); // chạy xong thời gian thì tự chuyển sang closing
    }

    // Bell: preparation -> opening
    private void HandleBellInteracted()
    {
        if(DayData.currentPhase == DayPhase.Preparation) // nếu đang ở preparation thì chuyển sang opening
        {
            activeNPCCount = 0;
            EnterPhase(DayPhase.Opening);
        }
        else if(DayData.currentPhase == DayPhase.Closing && activeNPCCount <= 0) // nếu đang ở closing và npc count = 0 thì chuyển sang night
        {
            EnterPhase(DayPhase.Night);
        }
        
    }

    private void HandleNPCLeft(NPCController _)
    {
        activeNPCCount = Mathf.Max(0, activeNPCCount - 1);
        if(DayData.currentPhase == DayPhase.Closing && activeNPCCount <= 0)
        {
            UpdateInteractables(DayData.currentPhase); // khi đang ở closing và npc count = 0 thì update interactable để unlock bell
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
        activeNPCCount = 0; // sang ngày mới và reset npc count

        Debug.Log("Today is day " + DayData.currentDay);
        EnterPhase(DayPhase.Preparation); // vào phase preparation
    }

    public void RegisterNPCSpawn() => activeNPCCount++; // gọi ở npc spanwer để tăng npc count dựa theo spawner

    private void UpdateInteractables(DayPhase phase) // hàm này để update trạng thái bell và bed để interact tùy theo phase
    {
        bellInteractable.canInteract = phase == DayPhase.Preparation || (phase == DayPhase.Closing && activeNPCCount <= 0);
        bedInteractable.canInteract = phase == DayPhase.Night;
    }

    // implement ISaveable
    public void PopulateSaveData(GameData data)
    {
        data.currentDay = DayData.currentDay;
    }

    public void LoadFromSaveData(GameData data)
    {
        DayData = new DayData();

        // load day number bằng cách chạy next day liên tục vì không thể sửa curentDay trực tiếp
        for(int i = 1; i < data.currentDay; i++)
        {
            DayData.NextDay();
        }

        hasLoaded = true;

        Debug.Log("Loaded day: " + DayData.currentDay);

        EnterPhase(DayPhase.Preparation);
    }
}
