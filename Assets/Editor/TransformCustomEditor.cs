// using UnityEditor;
// using UnityEngine;
// [CustomEditor(typeof(Transform))]
// public class TransformCustomEditor : Editor
// {
//     private SerializedProperty positionProp;
//     private SerializedProperty rotationProp;
//     private SerializedProperty scaleProp;
//     private void OnEnable()
//     {
//         positionProp = serializedObject.FindProperty("m_LocalPosition");
//         rotationProp = serializedObject.FindProperty("m_LocalRotation");
//         scaleProp = serializedObject.FindProperty("m_LocalScale");
//     }
//     public override void OnInspectorGUI()
//     {
//         serializedObject.Update();
//         
//         // 绘制Position
//         DrawPropertyWithReset(positionProp, "Position", Vector3.zero);
//         
//         // 绘制Rotation（使用原生显示方式）
//         DrawPropertyWithReset(rotationProp, "Rotation", Quaternion.identity);
//         
//         // 绘制Scale
//         DrawPropertyWithReset(scaleProp, "Scale", Vector3.one);
//         
//         // 世界坐标显示
//         Transform t = (Transform)target;
//         EditorGUILayout.LabelField("World Position", t.position.ToString("F2"));
//         
//         // 重置所有按钮
//         if (GUILayout.Button("Reset All"))
//         {
//             ResetTransform(t);
//         }
//         
//         serializedObject.ApplyModifiedProperties();
//     }
//     private void DrawPropertyWithReset(SerializedProperty prop, string label, object defaultValue)
//     {
//         EditorGUILayout.BeginHorizontal();
//         EditorGUILayout.PropertyField(prop, new GUIContent(label), true);
//         if (GUILayout.Button("R", GUILayout.Width(20)))
//         {
//             if (prop.propertyType == SerializedPropertyType.Quaternion)
//                 prop.quaternionValue = (Quaternion)defaultValue;
//             else
//                 prop.vector3Value = (Vector3)defaultValue;
//             
//             GUI.FocusControl(""); // 清除输入焦点
//         }
//         EditorGUILayout.EndHorizontal();
//     }
//     private void ResetTransform(Transform t)
//     {
//         Undo.RecordObject(t, "Reset Transform");
//         t.position = Vector3.zero;
//         t.rotation = Quaternion.identity;
//         t.localScale = Vector3.one;
//         serializedObject.Update(); // 强制更新序列化数据
//     }
// }