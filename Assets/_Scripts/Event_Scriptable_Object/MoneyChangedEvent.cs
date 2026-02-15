using System;
using UnityEngine;
[CreateAssetMenu(menuName = "Events/Money Changed")]
public class MoneyChangedEvent : ScriptableObject
{
    public event Action<int> Raised;

    public void Raise(int amount)
    {
        Raised?.Invoke(amount);
    }
}
