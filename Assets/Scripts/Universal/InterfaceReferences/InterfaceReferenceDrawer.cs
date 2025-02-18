#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(InterfaceReference<>))]
public class InterfaceReferenceDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty referenceProperty = property.FindPropertyRelative("reference");
        EditorGUI.ObjectField(position, referenceProperty, typeof(Object), label);

        EditorGUI.EndProperty();
    }
}

#endif