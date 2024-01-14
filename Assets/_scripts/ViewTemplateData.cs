using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class Root : ViewData
{
    public Vector2 position;
    public float height, width;
    public RectOffset vPadding, hPadding;
    public float rowSpacing, cellSpacing;
    public Color backgroundColor = Color.white;
    public List<Root> childs;
    public List<Container> cells;
}

[Serializable]
public class Container : ViewData
{
    public Color backgroundColor = Color.white;
    public List<ViewTemplateData> childs;
}

[Serializable]
public class ViewTemplateData : ViewData
{
    public Vector2 position;
    public Color background, outline;
    public string text;
    public int padding;
    public float width, textSize ,height;
    public List<ViewTemplateData> childs;
}

[Serializable]
public abstract class ViewData
{
    public string name, type;
    public Vector3 rotation, scale;
}
