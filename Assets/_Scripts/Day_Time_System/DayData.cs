using UnityEngine;

public class DayData
{
    public int currentDay {get; private set; } = 1;
    public DayPhase currentPhase {get; private set; } = DayPhase.Preparation;

    public void NextDay() => currentDay++;

    public void SetPhase(DayPhase phase) => currentPhase = phase;
}
