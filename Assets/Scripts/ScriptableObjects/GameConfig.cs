using UnityEngine;

/// <summary>
/// Designer-facing ScriptableObject that holds baseline values for every stat and flag
/// in the upgrade system. Serves as the immutable template read at session start.
///
/// ARCHITECTURE RULE: UpgradeManager must NEVER modify this asset at runtime.
///     1. GameSession copies relevant values from GameConfig on Initialize().
///     2. UpgradeManager.ApplyNodeOnSession() then mutates the GameSession copy.
///     3. This asset always reflects "zero upgrades purchased" defaults.
/// </summary>
[CreateAssetMenu(fileName = "GameConfig", menuName = "Settings/Game Config")]
public class GameConfig : ScriptableObject
{
    [Header("Initial Default State")]
    public GameState InitialState;
}