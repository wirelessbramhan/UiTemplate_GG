using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Serialiser : MonoBehaviour
{
    [SerializeField]
    private LayoutObject rootGO;
    public Root _templateRoot, _returnedRoot;
    [TextArea, SerializeField]
    private string jsonString;
    private void Start()
    {
        SetData();

        jsonString = JsonUtility.ToJson(_templateRoot, true);

        if(jsonString != null || jsonString != string.Empty)
        {
            _returnedRoot = JsonUtility.FromJson<Root>(jsonString);
        }
    }

    private void SetData()
    {
        if(rootGO)
        {
            _templateRoot = rootGO._viewData;
        }
    }
}
