using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[CustomPropertyDrawer(typeof(GameStateSelectorAttribute))]
public class GameStateSelectorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.String)
        {
            EditorGUI.PropertyField(position, property, label);
            return;
        }

        GameStateSelectorAttribute attr = (GameStateSelectorAttribute)attribute;

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Display current value, or "Select..." if empty
        string displayString = string.IsNullOrEmpty(property.stringValue) ? "Select..." : property.stringValue;

        // Draw the dropdown button
        if (EditorGUI.DropdownButton(position, new GUIContent(displayString), FocusType.Keyboard))
        {
            ShowDropdown(property, attr.FieldType, position);
        }
    }

    private void ShowDropdown(SerializedProperty property, GameStateFieldType type, Rect buttonRect)
    {
        // 1. Use reflection to get all public fields from GameState
        FieldInfo[] allFields = typeof(GameState).GetFields(BindingFlags.Public | BindingFlags.Instance);
        IEnumerable<FieldInfo> filteredFields;

        // 2. Filter fields based on what we are looking for
        if (type == GameStateFieldType.Flag)
        {
            filteredFields = allFields.Where(f => f.FieldType == typeof(bool));
        }
        else
        {
            filteredFields = allFields.Where(f => f.FieldType == typeof(float) || f.FieldType == typeof(int));
        }

        string[] options = filteredFields.Select(f => f.Name).ToArray();

        // 3. Create and show the Searchable Dropdown
        var dropdown = new FieldSearchDropdown(new AdvancedDropdownState(), options, selected =>
        {
            // Apply the selected string back to the property
            property.serializedObject.Update();
            property.stringValue = selected;
            property.serializedObject.ApplyModifiedProperties();
        });

        dropdown.Show(buttonRect);
    }
}

// --------------------------------------------------
// Internal class handling the actual search window UI
// --------------------------------------------------
public class FieldSearchDropdown : AdvancedDropdown
{
    private readonly string[] _options;
    private readonly Action<string> _onItemSelected;

    public FieldSearchDropdown(AdvancedDropdownState state, string[] options, Action<string> onItemSelected) : base(state)
    {
        _options = options;
        _onItemSelected = onItemSelected;
        
        // Define dropdown size
        minimumSize = new Vector2(250, 300);
    }

    protected override AdvancedDropdownItem BuildRoot()
    {
        var root = new AdvancedDropdownItem("Game State Fields");

        // Add all filtered fields as clickable children
        foreach (var option in _options)
        {
            root.AddChild(new AdvancedDropdownItem(option));
        }

        return root;
    }

    protected override void ItemSelected(AdvancedDropdownItem item)
    {
        base.ItemSelected(item);
        _onItemSelected?.Invoke(item.name);
    }
}