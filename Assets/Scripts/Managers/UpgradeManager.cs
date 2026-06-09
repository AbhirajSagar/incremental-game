using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class UpgradeManager : Singleton<UpgradeManager>
{
    public float DistBtwNodes = 1f;
    public UpgradeData[] Upgrades;
    public UpgradeNode NodePrefab;
    public RectTransform CanvasParent; 
    public RectTransform RootPos;

    private List<UpgradeNode> _nodes = new();
    private List<UpgradeData> SortedUpgrades = new();

    public void Initialize()
    {
        SortedUpgrades = Upgrades.OrderBy(GetDepth).ToList();
        UIManager.Instance.OnUpgradeScreenShow += CreateUpgradeTreeNodes;
    }

    private int GetDepth(UpgradeData upgrade)
    {
        int depth = 0;

        while (upgrade.UnlockAfter != null)
        {
            depth++;
            upgrade = upgrade.UnlockAfter;
        }

        return depth;
    }

    private void CreateUpgradeTreeNodes()
    {
        foreach(UpgradeNode node in _nodes)
            Destroy(node.gameObject);
        
        _nodes.Clear();

        Vector3 Base = new Vector3(DistBtwNodes * RootPos.transform.position.x, 0f, DistBtwNodes * RootPos.transform.position.z);
        for (int i = 0; i < SortedUpgrades.Count; i++)
        {
            UpgradeNode node = Instantiate(NodePrefab, Base * i, Quaternion.identity, CanvasParent);
            node.Initialize(SortedUpgrades[i], i, out bool Purchased);
            _nodes.Add(node);

            if(!Purchased) 
                break;
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