using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class TemplateCreatorWindow : ExtendedEditorWindow
{
    private Vector2 scrollPos;
    private ElementType type;
    private bool h = false, _baseCreated;
    [MenuItem("Window/UI Template Creator")]
    public static void ShowWindow(ScriptableObject dataObject)
    {
        TemplateCreatorWindow window = GetWindow<TemplateCreatorWindow>("Template Creator");
        window._serializedObject = new SerializedObject(dataObject);

        if (window._serializedObject != null)
        {
            Debug.Log("SO serialized");
        }
    }

    private void OnGUI()
    {
        //Auto draw properties of SO if Object serialized successfully

        if (_serializedObject != null)
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUIStyle.none);
            EditorGUILayout.LabelField("Hierarchy", EditorStyles.boldLabel);
            DrawPropertiesSimple("templateData");
            EditorGUILayout.EndScrollView();

            DrawPropertiesSimple("saves");
        }

        //resets bool Check for button name if window not used
        else
        {
            _baseCreated = false;
        }

        EditorGUILayout.Space(10);

        if (GUILayout.Button("Save Layout"))
        {
            FileDataHandlerSO.Save();
        }

        EditorGUILayout.Space(10);

        if (GUILayout.Button("Load Layout"))
        {
            FileDataHandlerSO.Load();
            //DrawPropertiesSimple("templateData");
        }

        EditorGUILayout.Space(10);
        
        //Draws enum foldout for adding elements to Root UI Object
        h = EditorGUILayout.BeginFoldoutHeaderGroup(h, "Element to Add");
        
        if (h) { type = (ElementType)EditorGUILayout.EnumPopup(type, EditorStyles.popup); }
        
        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorGUILayout.Space(10);

        if(!_baseCreated)
        {
            if (GUILayout.Button("Create Template"))
            {
                h = true;
                _baseCreated = true;
                ElementSpawner.RaiseRootSpawnEvent();
            }
        }

        else
        {
            if (GUILayout.Button("Add element"))
            {
                ElementSpawner.SetType(type);
                ElementSpawner.RaiseElementSpawnEvent();
            }
        }

        ChangePropertiesSimple();
    }

    private void OnFocus()
    {
        
    }

    private void OnEnable()
    {
        _baseCreated = false;
    }
}
