using UnityEngine;

[CreateAssetMenu(fileName = "New Drink", menuName = "Drink Data")]
public class DrinkData : ItemData
{
    public float brewTime;
    public int price;

    public bool isHot;
    public bool isSweet;
}
