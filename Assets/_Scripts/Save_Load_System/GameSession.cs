using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    private SaveService saveService;
    private GameData gameData;
    private List<ISaveable> saveables = new();

    private void Awake()
    {
        string path = Path.Combine(Application.persistentDataPath, "save.json"); // tạo đường dẫn lưu save game
        saveService = new SaveService(path); // khởi tạo save service
        gameData = saveService.Load(); // load dữ liệu từ save vào game data

        foreach(var s in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
        {
            if(s is ISaveable saveable)
            {
                saveables.Add(saveable); // add tất cả các object saveable vào list để load 1 lần
            }
        }

        LoadAll(); // load tất cả ngay khi game vừa load
    }

    private void OnApplicationQuit()
    {
        SaveAll(); // khi game tắt thì save tất cả lại
    }

    public void SaveAll()
    {
        foreach(var s in saveables)
        {
            s.PopulateSaveData(gameData);
        }
        saveService.Save(gameData);
    }

    private void LoadAll()
    {
        foreach(var s in saveables)
        {
            s.LoadFromSaveData(gameData);
        }
    }
}

