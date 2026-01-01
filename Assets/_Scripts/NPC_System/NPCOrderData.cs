using UnityEngine;
[System.Serializable]
public class NPCOrderData 
{
    // only used to store order data
    public DrinkData requestedDrink;
    public OrderStyle orderStyle;
    
}

public enum OrderStyle
{
    Direct,
    Indirect
}
