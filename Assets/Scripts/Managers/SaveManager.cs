using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SaveManager
{
    private static readonly string FileName = "playerData.json";

    public static event Action<SaveData> OnDataRefresh;

    public static void SaveNewData(SaveData Data)
    {
        string path = Path.Combine(Application.persistentDataPath, FileName);
        string json = JsonUtility.ToJson(Data, true);

        File.WriteAllText(path, json);
        OnDataRefresh?.Invoke(LoadData());
    }

    public static SaveData LoadData()
    {
        string path = Path.Combine(Application.persistentDataPath, FileName);

        if (!File.Exists(path))
        {
            return new SaveData();
        }

        string json = File.ReadAllText(path);
        return string.IsNullOrEmpty(json) ? new() : JsonUtility.FromJson<SaveData>(json);
    }

    [MenuItem("Tools/Clear Saved Data")]
    public static void ClearSavedData()
    {
        string path = Path.Combine(Application.persistentDataPath, FileName);

        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}