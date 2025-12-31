using UnityEngine;

public static class OrderGenerator
{
    static DrinkDatabase database;

    public static void InitializeDrinkDatabase(DrinkDatabase db)
    {
        database = db;
    }
    public static NPCOrderData Generate()
    {
        DrinkData drink = database.drinks[Random.Range(0, database.drinks.Count)];
        return new NPCOrderData
        {
            requestedDrink = drink,
            waitTime = 10f,
            orderStyle = Random.value > 0.5f ? OrderStyle.Direct : OrderStyle.Indirect
        };

    }
}
