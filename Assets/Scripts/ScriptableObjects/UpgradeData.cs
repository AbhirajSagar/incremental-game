using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public enum ModifierType { Add, Multiply, Override }

[Serializable]
public class StatModifier
{
    [Tooltip("Exact name of the field in GameState, e.g. 'BaseDamage'")]
    [GameStateSelectorAttribute(GameStateFieldType.Stat)]
    public string StatName;
    public float Value;
    public ModifierType ModType;
}

[Serializable]
public class FlagModifier
{
    [Tooltip("Exact name of the boolean field in GameState, e.g. 'HasSniper'")]
    [GameStateSelectorAttribute(GameStateFieldType.Flag)]
    public string FlagName;
    public bool Value = true;
}

[Serializable]
public class TooltipDisplayData
{
    public string Title;
    [Space(5)]
    [TextArea(3, 10)]
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
    public int Price;
    public Sprite Icon;
    public TooltipDisplayData TooltipData;

    [Header("Modifiers")]
    public List<StatModifier> StatModifiers = new List<StatModifier>();
    public List<FlagModifier> FlagModifiers = new List<FlagModifier>();

    [ContextMenu("Generate Id")]
    public void GenerateId()
    {
        Id = Guid.NewGuid().ToString("N");
    }

    public virtual void Apply(GameState state)
    {
        foreach (var mod in StatModifiers)
        {
            var field = typeof(GameState).GetField(mod.StatName);
            if (field != null)
            {
                if (field.FieldType == typeof(float))
                {
                    float current = (float)field.GetValue(state);
                    if (mod.ModType == ModifierType.Add) current += mod.Value;
                    else if (mod.ModType == ModifierType.Multiply) current *= mod.Value;
                    else if (mod.ModType == ModifierType.Override) current = mod.Value;
                    field.SetValue(state, current);
                }
                else if (field.FieldType == typeof(int))
                {
                    int current = (int)field.GetValue(state);
                    if (mod.ModType == ModifierType.Add) current += (int)mod.Value;
                    else if (mod.ModType == ModifierType.Multiply) current = Mathf.RoundToInt(current * mod.Value);
                    else if (mod.ModType == ModifierType.Override) current = (int)mod.Value;
                    field.SetValue(state, current);
                }
            }
            else
            {
                Debug.LogWarning($"[UpgradeData] Stat '{mod.StatName}' not found in GameState!");
            }
        }

        foreach (var mod in FlagModifiers)
        {
            var field = typeof(GameState).GetField(mod.FlagName);
            if (field != null && field.FieldType == typeof(bool))
            {
                field.SetValue(state, mod.Value);
            }
            else
            {
                Debug.LogWarning($"[UpgradeData] Flag '{mod.FlagName}' not found in GameState!");
            }
        }
    }

    public void Deconstruct(out string Title, out string Price, out string Description, out string PropertyName, out string Change, out Sprite Icon)
    {
        Title = TooltipData.Title;
        Price = $"${this.Price}";
        Description = TooltipData.Description;
        PropertyName = GetPropertyName();
        Icon = this.Icon;
        Change = GetChange();
    }

    private string GetChange()
    {
        if (!string.IsNullOrEmpty(TooltipData.Change)) return TooltipData.Change;
        return $"{ComputeCurrentValue()} > {ComputeUpgradedValue()}";
    }

    private string ComputeCurrentValue()
    {
        GameState state = GameManager.Session.State;

        if (StatModifiers.Count > 0)
        {
            StatModifier mod = StatModifiers[0];
            FieldInfo field = typeof(GameState).GetField(mod.StatName);

            if (field == null)
                return "?";

            object value = field.GetValue(state);
            return value?.ToString() ?? "?";
        }

        if (FlagModifiers.Count > 0)
        {
            FlagModifier mod = FlagModifiers[0];
            FieldInfo field = typeof(GameState).GetField(mod.FlagName);

            if (field == null)
                return "?";

            object value = field.GetValue(state);
            return value?.ToString() ?? "?";
        }

        return "?";
    }

    private string ComputeUpgradedValue()
    {
        GameState state = GameManager.Session.State;

        if (StatModifiers.Count > 0)
        {
            StatModifier mod = StatModifiers[0];
            FieldInfo field = typeof(GameState).GetField(mod.StatName);

            if (field == null)
                return "?";

            if (field.FieldType == typeof(float))
            {
                float current = (float)field.GetValue(state);

                switch (mod.ModType)
                {
                    case ModifierType.Add:
                        current += mod.Value;
                        break;

                    case ModifierType.Multiply:
                        current *= mod.Value;
                        break;

                    case ModifierType.Override:
                        current = mod.Value;
                        break;
                }

                return current.ToString();
            }

            if (field.FieldType == typeof(int))
            {
                int current = (int)field.GetValue(state);

                switch (mod.ModType)
                {
                    case ModifierType.Add:
                        current += (int)mod.Value;
                        break;

                    case ModifierType.Multiply:
                        current = Mathf.RoundToInt(current * mod.Value);
                        break;

                    case ModifierType.Override:
                        current = (int)mod.Value;
                        break;
                }

                return current.ToString();
            }
        }

        if (FlagModifiers.Count > 0)
        {
            return FlagModifiers[0].Value.ToString();
        }

        return "?";
    }

    private string GetPropertyName()
    {
        if (!string.IsNullOrEmpty(TooltipData.Property)) return TooltipData.Property;
        return StatModifiers.Count == 0 ? ExtractPropertyName(GameStateFieldType.Flag) : ExtractPropertyName(GameStateFieldType.Stat);
    }

    private string ExtractPropertyName(GameStateFieldType FieldType)
    {
        if (FieldType == GameStateFieldType.Stat)
        {
            StatModifier mod = StatModifiers[0];
            var field = typeof(GameState).GetField(mod.StatName);
            if (field == null) return mod.StatName;
            var attr = field.GetCustomAttribute<UnityEngine.Serialization.FormerlySerializedAsAttribute>();
            return attr != null ? attr.oldName : mod.StatName;
        }

        if (FieldType == GameStateFieldType.Flag)
        {
            if (FlagModifiers.Count == 0) return string.Empty;
            FlagModifier mod = FlagModifiers[0];
            var field = typeof(GameState).GetField(mod.FlagName);
            if (field == null) return mod.FlagName;
            var attr = field.GetCustomAttribute<UnityEngine.Serialization.FormerlySerializedAsAttribute>();
            return attr != null ? attr.oldName : mod.FlagName;
        }

        return string.Empty;
    }
}