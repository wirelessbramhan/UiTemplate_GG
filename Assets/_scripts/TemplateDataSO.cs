using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New TemplateData Container", menuName = "UI/Template Data", order = 1)]
public class TemplateDataSO : ScriptableObject
{
    public List<ViewTemplateData> templateData;    
    public List<string> saves;

    #region Save/Load Event Handling
    private void OnEnable()
    {
       FileDataHandlerSO.OnSaveEvent += SaveTemplate;
       FileDataHandlerSO.OnLoadEvent += LoadTemplateLast;
    }

    private void OnDisable()
    {
        FileDataHandlerSO.OnSaveEvent -= SaveTemplate;
        FileDataHandlerSO.OnLoadEvent -= LoadTemplateLast;
    }
    #endregion

    //Uses serilized list to display current hierarachy
    public void SetData(ViewTemplateData data)
    {
        templateData.Insert(0, data);
    }

    //Displays loaded root as Second entry
    public void SetLoadedData(ViewTemplateData data)
    {
        templateData[1] = data;
    }

    //modifies static name list of saves to keep track of session saves like a stack
    private void SaveTemplate()
    {
        saves.Insert(0, "Template_" + saves.Count + ".templatedata");
    }

    private void LoadTemplateLast()
    {
        saves.RemoveAt(saves.Count - 1);
    }
}
