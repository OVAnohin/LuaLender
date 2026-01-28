using System.IO;
using UnityEngine;

public class SaveService
{
    private static string Path => System.IO.Path.Combine(Application.persistentDataPath, "save.json");

    public static void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Path, json);
    }

    public static SaveData Load()
    {
        if (!File.Exists(Path))
            return new SaveData();

        string json = File.ReadAllText(Path);
        return JsonUtility.FromJson<SaveData>(json);
    }
}
