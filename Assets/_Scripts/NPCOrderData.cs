using UnityEngine;
[System.Serializable]
public class NPCOrderData 
{
    public DrinkData requestedDrink;
    public OrderStyle orderStyle;
    public float waitTime;
}

public enum OrderStyle
{
    Direct,
    Indirect
}
