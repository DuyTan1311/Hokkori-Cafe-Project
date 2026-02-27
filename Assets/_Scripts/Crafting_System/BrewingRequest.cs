using UnityEngine;
[System.Serializable]
public struct BrewingRequest
{
    public DrinkData baseDrink;
    public BrewingOptions options;
}

[System.Serializable]
public struct BrewingOptions
{
    public bool isHot;
    public bool isSweet;
}