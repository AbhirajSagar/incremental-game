using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class TooltipDisplayData
{
    public string Title;
    [Space(5)]
    [TextArea(3,10)]
    public string Description;
    [Space(5)]
    public string Property;
    [Space(5)]
    public string Change;
}

[CreateAssetMenu(fileName = "UpgradeData", menuName = "Settings/New Upgrade")]
public class UpgradeData : ScriptableObject
{
    public string Id = "";
    public int Value;
    public int Price;
    public Sprite Icon;
    public TooltipDisplayData TooltipData;
    public UnityEvent OnApply;
    

    [ContextMenu("Generate Id")]
    public void GenerateId()
    {
        Id = Guid.NewGuid().ToString("N");

    }
}