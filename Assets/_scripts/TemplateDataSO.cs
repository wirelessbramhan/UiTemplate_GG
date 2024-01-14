using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New TemplateData Container", menuName = "UI/Template Data", order = 1)]
public class TemplateDataSO : ScriptableObject
{
    public List<ViewTemplateData> templateData;
    public FileDataHandlerSO fileHandler;
    public static event Action OnSaveEvent;
    public static event Action OnLoadEvent;
    public static int count = 1;
    public List<string> saves;
    private void OnEnable()
    {
        OnSaveEvent += SaveTemplate;
        OnLoadEvent += LoadTemplateLast;
    }

    private void OnDisable()
    {
        OnSaveEvent -= SaveTemplate;
        OnLoadEvent -= LoadTemplateLast;
    }
    private void SaveTemplate()
    {
        fileHandler.dataHandler.Save(templateData[0], "Template_" + saves.Count + ".templatedata");
        saves.Add("Template_" + saves.Count + ".templatedata");
    }

    private void LoadTemplateLast()
    {
        templateData.Add(fileHandler.dataHandler.Load(saves[saves.Count - 1]));
        saves.RemoveAt(saves.Count - 1);
    }

    public static void Save()
    {
        OnSaveEvent?.Invoke();
    }

    public static void Load()
    {
        OnLoadEvent?.Invoke();
    }

    public void SetData(ViewTemplateData data)
    {
        templateData.Insert(0, data);
    }
}
