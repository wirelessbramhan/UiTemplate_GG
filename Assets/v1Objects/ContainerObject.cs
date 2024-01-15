using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ContainerObject : TemplateObject
{
    public Container _viewData = new();
    private RawImage _image;
    public override void Configure()
    { 
        gameObject.name = _viewData.name + "_" + _viewData.type;
    }

    public override void Setup()
    {
        if(TryGetComponent<RawImage>(out _image))
        {
            _image.color = _viewData.backgroundColor;
        }
    }

    public void AddChild()
    {
        _viewData.childs.Clear();

        foreach(Transform obj in transform)    
        {
            if(obj != this)
            {
                var i = obj.GetComponent<TemplateObject>();
                _viewData.childs.Add(i._loadedData);
            }
        }
    }
}
