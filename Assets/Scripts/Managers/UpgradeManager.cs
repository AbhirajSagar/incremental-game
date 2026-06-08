using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class UpgradeManager : Singleton<UpgradeManager>
{
    public UpgradeData[] Upgrades;
    public UpgradeNode NodePrefab;
    public RectTransform CanvasParent; 

    public void Initialize()
    {
        CreateUpgradeTreeNodes();
    }

    private void CreateUpgradeTreeNodes()
    {
        foreach(var upgrade in Upgrades)
        {
            UpgradeNode node = Instantiate(NodePrefab, CanvasParent);
            node.Initialize(upgrade);
        }
    }

    public static void ApplyNodeOnSession(string NodeId, GameSession Session)
    {
        if (Instance == null || Instance.Upgrades == null) return;

        var upgrade = Instance.Upgrades.FirstOrDefault(u => u.Id == NodeId);
        if (upgrade != null)
        {
            upgrade.Apply(Session.State);
        }
        else
        {
            Debug.LogWarning($"[UpgradeManager] Failed to apply node: Upgrade with ID '{NodeId}' not found.");
        }
    }

    [ContextMenu("Load from folder")]
    public void Load()
    {
#if UNITY_EDITOR
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
#endif
    }
}