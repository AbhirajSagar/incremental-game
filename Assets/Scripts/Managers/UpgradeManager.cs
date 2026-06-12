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

    private List<GameObject> _lines = new();
    private Dictionary<UpgradeNode, GameObject> _nodeToLine = new();

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
        foreach (UpgradeNode node in _nodes)
            Destroy(node.gameObject);
        _nodes.Clear();

        foreach (GameObject line in _lines)
            Destroy(line);
        _lines.Clear();
        _nodeToLine.Clear();

        Dictionary<int, int> nodesAtDepth = new();
        Dictionary<int, int> totalNodesAtDepth = new();
        Dictionary<UpgradeData, UpgradeNode> nodeLookup = new();

        foreach (var upgrade in SortedUpgrades)
        {
            int d = GetDepth(upgrade);
            if (!totalNodesAtDepth.ContainsKey(d))
                totalNodesAtDepth[d] = 0;
            totalNodesAtDepth[d]++;
        }

        Vector3 startPos = CanvasParent.InverseTransformPoint(RootPos.position);
        for (int i = 0; i < SortedUpgrades.Count; i++)
        {
            var upgrade = SortedUpgrades[i];
            int depth = GetDepth(upgrade);
            
            if (!nodesAtDepth.ContainsKey(depth))
                nodesAtDepth[depth] = 0;
            
            int indexAtDepth = nodesAtDepth[depth];
            nodesAtDepth[depth]++;
            
            int countAtDepth = totalNodesAtDepth[depth];
            
            float xOffset = depth * DistBtwNodes;
            float yOffset = (indexAtDepth - (countAtDepth - 1) / 2f) * DistBtwNodes; // Spread vertically

            UpgradeNode node = Instantiate(NodePrefab, CanvasParent);
            node.transform.localPosition = startPos + new Vector3(xOffset, yOffset, 0);

            node.Initialize(SortedUpgrades[i], i, out bool Purchased);
            _nodes.Add(node);
            nodeLookup[upgrade] = node;
            
            node.gameObject.SetActive(false);
        }

        // Draw connecting lines
        foreach (var upgrade in SortedUpgrades)
        {
            if (upgrade.UnlockAfter != null && nodeLookup.ContainsKey(upgrade.UnlockAfter))
            {
                UpgradeNode childNode = nodeLookup[upgrade];
                UpgradeNode parentNode = nodeLookup[upgrade.UnlockAfter];
                GameObject line = CreateConnectionLine(parentNode.transform.localPosition, childNode.transform.localPosition);
                line.SetActive(false);
                _nodeToLine[childNode] = line;
            }
        }
        
        UpdateTree(true);
    }

    public void UpdateTree(bool initialSpawn = false)
    {
        int animIndex = 0;
        foreach (var node in _nodes)
        {
            node.RefreshState();
            
            bool shouldBeVisible = node.IsPrerequisiteMet || node.PurchasedAlready;
            
            if (shouldBeVisible && !node.gameObject.activeSelf)
            {
                if (initialSpawn)
                {
                    node.SpawnNode(animIndex * node.DelayInNodeAnim);
                    animIndex++;
                }
                else
                {
                    node.SpawnNode(0f);
                }
                
                if (_nodeToLine.TryGetValue(node, out GameObject line))
                {
                    line.SetActive(true);
                }
            }
            else if (!shouldBeVisible)
            {
                node.gameObject.SetActive(false);
                if (_nodeToLine.TryGetValue(node, out GameObject line))
                {
                    line.SetActive(false);
                }
            }
        }
    }

    private GameObject CreateConnectionLine(Vector3 startLocal, Vector3 endLocal)
    {
        GameObject lineObj = new GameObject("ConnectionLine", typeof(RectTransform), typeof(UnityEngine.UI.Image));
        lineObj.transform.SetParent(CanvasParent, false);
        lineObj.transform.SetAsFirstSibling(); // Draw behind the nodes

        RectTransform rt = lineObj.GetComponent<RectTransform>();
        UnityEngine.UI.Image img = lineObj.GetComponent<UnityEngine.UI.Image>();
        img.color = new Color(1, 1, 1, 0.3f); // Semi-transparent white line

        Vector3 dir = endLocal - startLocal;
        float distance = dir.magnitude;

        rt.localPosition = startLocal + dir / 2f;
        rt.sizeDelta = new Vector2(distance, 5f); // 5 units thick
        rt.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);

        _lines.Add(lineObj);
        return lineObj;
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

    public int GetAffordableUpgradesCount()
    {
        int AffordableCount = 0;
        foreach (UpgradeNode node in _nodes)
        {
            if(node.IsPrerequisiteMet && !node.PurchasedAlready && node.Affordable)
            {
                AffordableCount++;
            }
        }

        return AffordableCount;
    }
}