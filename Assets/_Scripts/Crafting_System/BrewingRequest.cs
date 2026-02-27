using UnityEngine;
[System.Serializable]
public struct BrewingRequest
{
    public DrinkData baseDrink;
    public BrewingOptions options;

    public BrewingRequest(DrinkData drink, BrewingOptions brewingOptions)
    {
        baseDrink = drink;
        options = brewingOptions;
    }
}

[System.Serializable]
public struct BrewingOptions
{
    public bool isHot;
    public bool isSweet;
}