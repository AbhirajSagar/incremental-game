using UnityEngine;

public enum GameStateFieldType { Stat, Flag }

public class GameStateSelectorAttribute : PropertyAttribute
{
    public GameStateFieldType FieldType;

    public GameStateSelectorAttribute(GameStateFieldType type)
    {
        FieldType = type;
    }
}