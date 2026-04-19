using UnityEngine;

public class TestMoney : MonoBehaviour, ISaveable
{
    public int money = 0;

    private void Start()
    {
        money += 10;
        Debug.Log("Auto add money: " + money);
    }

    public void PopulateSaveData(GameData data)
    {
        data.money = money;
        Debug.Log("SAVE money = " + money);
    }

    public void LoadFromSaveData(GameData data)
    {
        money = data.money;
        Debug.Log("LOAD money = " + money);
    }
}