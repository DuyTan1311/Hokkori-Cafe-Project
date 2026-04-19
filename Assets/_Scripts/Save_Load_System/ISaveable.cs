using UnityEngine;

public interface ISaveable
{
    void PopulateSaveData(GameData data); // save data vào gamedata
    void LoadFromSaveData(GameData data); // load data từ gamedata vào game
}
