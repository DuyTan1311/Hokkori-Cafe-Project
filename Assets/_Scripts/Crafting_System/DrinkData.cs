using UnityEngine;

[CreateAssetMenu(fileName = "New Drink", menuName = "Drink Data")]
public class DrinkData : ScriptableObject
{
    public string drinkName;
    public float brewTime;
    public int price;
}
