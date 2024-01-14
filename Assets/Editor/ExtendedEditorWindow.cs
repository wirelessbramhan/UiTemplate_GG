using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ExtendedEditorWindow : EditorWindow
{
    protected SerializedObject _serializedObject;
    protected SerializedProperty _serializedProperty;

    protected void DrawProperties(SerializedProperty prop, bool showChildren)
    {
        string lastPropPath = string.Empty;
        
        foreach (SerializedProperty p in prop)
        {
            if (p.isArray && p.propertyType == SerializedPropertyType.Generic)
            {
                EditorGUILayout.BeginHorizontal();
                p.isExpanded = EditorGUILayout.Foldout(p.isExpanded, p.displayName);
                EditorGUILayout.EndHorizontal();

                if (p.isExpanded)
                {
                    EditorGUI.indentLevel++;
                    DrawProperties(p, showChildren);
                    EditorGUI.indentLevel--;
                }
            }

            else
            {
                if (string.IsNullOrEmpty(lastPropPath) && p.propertyPath.Contains(lastPropPath)) { continue; }
                lastPropPath = p.propertyPath;
                EditorGUILayout.PropertyField(p, showChildren);
            }
        }
    }

    protected void DrawPropertiesSimple(string propertyName)
    {
        _serializedProperty = _serializedObject.FindProperty(propertyName);
        EditorGUILayout.PropertyField(_serializedProperty);
    }

    protected void ChangePropertiesSimple()
    {
        if(_serializedObject != null)
        {
            _serializedObject.ApplyModifiedProperties();
        }
    }
}
