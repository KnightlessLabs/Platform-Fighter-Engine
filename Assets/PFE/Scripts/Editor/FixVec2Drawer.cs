using UnityEditor;
using UnityEngine;
using FixedPointy;

// IngredientDrawer
[CustomPropertyDrawer(typeof(FixVec2))]
public class FixVec2Drawer : PropertyDrawer {

    public Vector2 val;
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        FixVec2 te = new FixVec2();
        te._x._raw = property.FindPropertyRelative("_x").FindPropertyRelative("_raw").intValue;
        te._y._raw = property.FindPropertyRelative("_y").FindPropertyRelative("_raw").intValue;
        val = (Vector2)te;

        EditorGUI.BeginProperty(position, label, property);

        // Calculate rects
        var nameRect = new Rect(position.x, position.y, position.width - 90, position.height);
        var xRect = new Rect(position.x+130, position.y, 60, position.height);
        var yRect = new Rect(position.x+185, position.y, 60, position.height);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        //EditorGUI.PropertyField(amountRect, val.x. GUIContent.none);
        //EditorGUI.PropertyField(unitRect, val.y, GUIContent.none);
        //EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("name"), GUIContent.none);
        EditorGUI.LabelField(nameRect, property.name);
        val.x = EditorGUI.FloatField(xRect, val.x);
        val.y = EditorGUI.FloatField(yRect, val.y);

        EditorGUI.EndProperty();

        if (GUI.changed) {
            FixVec2 tem = (FixVec2)val;
            property.FindPropertyRelative("_x").FindPropertyRelative("_raw").intValue = tem._x._raw;
            property.FindPropertyRelative("_y").FindPropertyRelative("_raw").intValue = tem._y._raw;
        }
    }
}