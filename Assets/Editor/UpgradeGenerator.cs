using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class UpgradeGenerator
{
    [MenuItem("Tools/Generate Sample Upgrades")]
    public static void GenerateUpgrades()
    {
        string folderPath = "Assets/Upgrades";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets", "Upgrades");
        }

        // Helper to create Stat upgrades
        void CreateStatUpgrade(string id, string name, string statName, int price, float value, ModifierType modType, UpgradeData unlockAfter = null)
        {
            UpgradeData upgrade = ScriptableObject.CreateInstance<UpgradeData>();
            upgrade.Id = System.Guid.NewGuid().ToString("N");
            upgrade.Price = price;
            
            string verb = modType == ModifierType.Add ? "Increases" : (modType == ModifierType.Multiply ? "Multiplies" : "Overrides");
            string formattedValue = (modType == ModifierType.Multiply) ? $"x{value}" : $"+{value}";
            
            upgrade.TooltipData = new TooltipDisplayData
            {
                Title = name,
                Description = $"{verb} your {name} by {value}.",
                Property = name,
                Change = ""
            };
            
            upgrade.StatModifiers = new List<StatModifier>
            {
                new StatModifier { StatName = statName, Value = value, ModType = modType }
            };
            
            upgrade.UnlockAfter = unlockAfter;

            string assetPath = $"{folderPath}/{id}.asset";
            AssetDatabase.CreateAsset(upgrade, assetPath);
            AssetDatabase.SaveAssets();
        }

        // Helper to create Flag upgrades
        void CreateFlagUpgrade(string id, string name, string desc, string flagName, int price, UpgradeData unlockAfter = null)
        {
            UpgradeData upgrade = ScriptableObject.CreateInstance<UpgradeData>();
            upgrade.Id = System.Guid.NewGuid().ToString("N");
            upgrade.Price = price;
            
            upgrade.TooltipData = new TooltipDisplayData
            {
                Title = name,
                Description = desc,
                Property = name,
                Change = "Unlocked"
            };
            
            upgrade.FlagModifiers = new List<FlagModifier>
            {
                new FlagModifier { FlagName = flagName, Value = true }
            };
            
            upgrade.UnlockAfter = unlockAfter;

            string assetPath = $"{folderPath}/{id}.asset";
            AssetDatabase.CreateAsset(upgrade, assetPath);
            AssetDatabase.SaveAssets();
        }

        // Generate some sample chains!

        // Base Damage Chain
        CreateStatUpgrade("dmg-1", "Harpoon Sharpening", "BaseDamage", 10, 1f, ModifierType.Add);
        UpgradeData dmg1 = AssetDatabase.LoadAssetAtPath<UpgradeData>($"{folderPath}/dmg-1.asset");
        CreateStatUpgrade("dmg-2", "Advanced Alloys", "BaseDamage", 50, 2f, ModifierType.Add, dmg1);
        UpgradeData dmg2 = AssetDatabase.LoadAssetAtPath<UpgradeData>($"{folderPath}/dmg-2.asset");
        CreateFlagUpgrade("piercing-unlock", "Piercing Harpoon", "Attacks deal full damage to all targets instead of dividing it.", "HasPiercing", 150, dmg2);
        
        // Crit Chain
        CreateStatUpgrade("crit-1", "Precision Sights", "CritChance", 20, 0.05f, ModifierType.Add);
        UpgradeData crit1 = AssetDatabase.LoadAssetAtPath<UpgradeData>($"{folderPath}/crit-1.asset");
        CreateStatUpgrade("crit-2", "Lethal Strikes", "CritMultiplier", 60, 0.5f, ModifierType.Add, crit1);
        UpgradeData crit2 = AssetDatabase.LoadAssetAtPath<UpgradeData>($"{folderPath}/crit-2.asset");
        CreateFlagUpgrade("sniper-unlock", "Sniper Focus", "3x damage when hitting the exact center of a fish.", "HasSniper", 200, crit2);

        // Tycoon Chain
        CreateStatUpgrade("gold-1", "Haggler", "FishSellPriceBonus", 15, 1f, ModifierType.Add);
        UpgradeData gold1 = AssetDatabase.LoadAssetAtPath<UpgradeData>($"{folderPath}/gold-1.asset");
        CreateStatUpgrade("gold-2", "Shiny Lure", "FishSpawnRateMultiplier", 75, 1.2f, ModifierType.Multiply, gold1);
        UpgradeData gold2 = AssetDatabase.LoadAssetAtPath<UpgradeData>($"{folderPath}/gold-2.asset");
        CreateFlagUpgrade("bounty-unlock", "Bounty Hunter", "Enables periodic high-value bounty fish spawns.", "HasBountyHunter", 300, gold2);

        // Deep Diver Chain
        CreateStatUpgrade("oxy-1", "Lung Capacity", "MaxOxygen", 25, 20f, ModifierType.Add);
        UpgradeData oxy1 = AssetDatabase.LoadAssetAtPath<UpgradeData>($"{folderPath}/oxy-1.asset");
        CreateStatUpgrade("oxy-2", "Efficient Breathing", "OxygenDrainPerSecond", 80, 0.8f, ModifierType.Multiply, oxy1); // 20% less drain
        UpgradeData oxy2 = AssetDatabase.LoadAssetAtPath<UpgradeData>($"{folderPath}/oxy-2.asset");
        CreateFlagUpgrade("tank-unlock", "Scuba Tank", "Exposes a UI button to fully restore O2 once per dive.", "HasScubaTank", 250, oxy2);
        
        // Occultist Branch
        CreateStatUpgrade("frenzy-1", "Blood Frenzy", "FrenzyBonusPerStack", 30, 0.02f, ModifierType.Add);
        UpgradeData frenzy1 = AssetDatabase.LoadAssetAtPath<UpgradeData>($"{folderPath}/frenzy-1.asset");
        CreateFlagUpgrade("voidrift-unlock", "Void Rift", "Clicking the same empty spot 5 times rapidly opens a black hole.", "HasVoidRift", 500, frenzy1);

        AssetDatabase.Refresh();
        Debug.Log("[UpgradeGenerator] Successfully generated 14 new UpgradeData assets in Assets/Upgrades!");
    }
}