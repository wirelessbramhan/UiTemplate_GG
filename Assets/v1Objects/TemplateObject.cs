using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class TemplateObject : MonoBehaviour
{
    public List<TemplateObject> childs = new();
    protected RectTransform _rectTransform;
    [HideInInspector] public ViewTemplateData _data;
    protected bool _loaded;
    public List<TemplateObject> Children;
    private void Awake()
    {
        Init();
    }

    private void OnValidate()
    {
        Init();
    }
    private void Init()
    {
        AddChildren();
        Setup();
        Configure();
    }

    public abstract void Configure();

    public abstract void Setup();

    [ContextMenu("add children")]
    private void AddChildren()
    {
        Children.Clear();
        _data.childs.Clear();

        if (transform.childCount > 0)
        {
            foreach (Transform t in transform.GetChild(0))
            {
                if (t.TryGetComponent<TemplateButton>(out var obj))
                {
                    Children.Add(obj);
                    _data.childs.Add(obj._viewData);
                }
            }
        }
    }
}