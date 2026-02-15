using System;
using UnityEngine;
[CreateAssetMenu(menuName = "Events/Drink Consumed")]
public class DrinkConsumedEvent : ScriptableObject
{
    public event Action<DrinkData> Raised;

    public void Raise(DrinkData drink)
    {
        Raised?.Invoke(drink);
    }
}
