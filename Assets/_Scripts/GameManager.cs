using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] DrinkDatabase drinkDatabase;

    private void Awake()
    {
        OrderGenerator.InitializeDrinkDatabase(drinkDatabase);
    }

    //gonna change this later
}
