using UnityEngine;

public static class OrderGenerator
{
    // use to generate order
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
            orderStyle = Random.value > 0.5f ? OrderStyle.Direct : OrderStyle.Indirect
        };

    }
}
