using UnityEngine;

public class MoneyService : MonoBehaviour
{
    [SerializeField] DrinkConsumedEvent OnDrinkConsumed;
    [SerializeField] MoneyChangedEvent OnMoneyChanged;

    public int Balance { get; private set; }

    private void OnEnable()
    {
        OnDrinkConsumed.Raised += HandleDrinkConsumed;
    }

    private void OnDisable()
    {
        OnDrinkConsumed.Raised -= HandleDrinkConsumed;
    }

    void HandleDrinkConsumed(DrinkData drink)
    {
        Add(drink.price);
    }

    void Add(int amount)
    {
        Balance += amount;
        OnMoneyChanged.Raise(Balance);
        Debug.Log("Money: " + Balance);
    }
}
