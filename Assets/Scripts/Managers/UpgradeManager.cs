using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UpgradeManager : Singleton<UpgradeManager>
{
    public UpgradeData[] Upgrades;

    public static void ApplyNodeOnSession(string NodeId, GameSession Session)
    {
        
    }

    [ContextMenu("Load from folder")]
    public void Load()
    {
        string[] guids = AssetDatabase.FindAssets("t:UpgradeData", new[] { "Assets/Upgrades" });

        List<UpgradeData> upgradesList = new();

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            UpgradeData data = AssetDatabase.LoadAssetAtPath<UpgradeData>(path);

            if (data != null)
                upgradesList.Add(data);
        }

        Upgrades = upgradesList.ToArray();
    }
}