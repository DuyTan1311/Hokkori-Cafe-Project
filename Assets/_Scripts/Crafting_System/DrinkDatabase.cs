using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Drink Database", menuName = "Drink Database")]
public class DrinkDatabase : ScriptableObject
{
    public List<DrinkData> drinks;
}
