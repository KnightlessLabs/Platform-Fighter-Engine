using UnityEditor;
using UnityEngine;
using FixedPointy;

// IngredientDrawer
[CustomPropertyDrawer(typeof(Fix))]
public class FixDrawer : PropertyDrawer {

    float fValue;
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        Fix te = new Fix(property.FindPropertyRelative("_raw").intValue);
        fValue = (float)te;

        fValue = EditorGUI.FloatField(position, property.name, fValue);

        if (GUI.changed) {
            Fix tem = (Fix)fValue;
            property.FindPropertyRelative("_raw").intValue = tem._raw;
        }
    }
}