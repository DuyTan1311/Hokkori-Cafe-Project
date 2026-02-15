using UnityEngine;

public class MoneyService : MonoBehaviour
{
    [SerializeField] DrinkConsumedEvent OnDrinkConsumed;

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
        Debug.Log("Money: " + Balance);
    }
}
