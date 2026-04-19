using UnityEngine;
using System.IO;

public class SaveService
{
    private readonly string filePath; // readonly là chỉ được gán 1 lần duy nhất

    public SaveService(string filePath) //constructor
    {
        this.filePath = filePath;
    }

    public void Save(GameData data)
    {
        string json = JsonUtility.ToJson(data, prettyPrint: true); // chuyển object sang định dạng json
        File.WriteAllText(filePath, json); // ghi nội dung json vào file trong đường dẫn
    }

    public GameData Load()
    {
        if (!File.Exists(filePath))
        {
            return new GameData(); // nếu file không tồn tại, tạo mới
        }
        string json = File.ReadAllText(filePath); // đọc toàn bộ file thành string
        return JsonUtility.FromJson<GameData>(json); // chuyển string json thành game data
    }
}
