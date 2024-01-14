using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class AssetHandler
{
    [OnOpenAsset]
    public static bool OpenEditor(int instanceId, int line)
    {
        TemplateDataSO obj = EditorUtility.InstanceIDToObject(instanceId) as TemplateDataSO;

        if (obj != null) 
        {
            TemplateCreatorWindow.ShowWindow(obj);
            return true;
        }

        return false;
    }
}

[CustomEditor(typeof(TemplateDataSO))]
public class TemplateDataEditor : Editor
{
    private void OnEnable()
    {
        TemplateDataSO.OnLoadEvent += RefreshWindow;
        TemplateDataSO.OnSaveEvent += RefreshWindow;
    }

    private void OnDisable()
    {
        TemplateDataSO.OnLoadEvent -= RefreshWindow;
        TemplateDataSO.OnSaveEvent -= RefreshWindow;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(EditorGUIUtility.singleLineHeight * 2);

        if (GUILayout.Button("Open Editor"))
        {
            TemplateCreatorWindow.ShowWindow((TemplateDataSO)target);
        }
    }

    private void RefreshWindow()
    {
        TemplateCreatorWindow.ShowWindow((TemplateDataSO)target);
    }
}
