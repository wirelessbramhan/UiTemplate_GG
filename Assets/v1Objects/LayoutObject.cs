using UnityEngine;
using UnityEngine.UI;

public class LayoutObject : TemplateObject
{
    public Root _viewData = new();
    public const string _h = "horizontal", _v = "vertical";
    private VerticalLayoutGroup _verticalLayoutGroup;
    private HorizontalLayoutGroup _horizontalLayoutGroup;

    private void OnValidate()
    {
        Init();
    }

    private void Awake()
    {
        Init();
    }

    private void Start()
    {

    }

    private void OnDisable()
    {

    }

    [ContextMenu("Initialize View Data")]
    private void Init()
    {
        Setup();
        Configure();
    }

    public override void Configure()
    {
        if(_verticalLayoutGroup != null)
        {
            _viewData.type = _v;
            _verticalLayoutGroup.padding = _viewData.vPadding;
            _verticalLayoutGroup.spacing = _viewData.rowSpacing;
        }

        else if(_horizontalLayoutGroup != null) 
        {
            _viewData.type = _h;
            _horizontalLayoutGroup.padding = _viewData.hPadding;
            _horizontalLayoutGroup.spacing = _viewData.cellSpacing;
        }

        if (_rectTransform != null && _horizontalLayoutGroup == null)
        {
            _rectTransform.offsetMin = new Vector2(-_viewData.width, 0);
            _rectTransform.offsetMax = new Vector2(0, _viewData.height);
            _rectTransform.anchoredPosition = _viewData.position;
            _rectTransform.rotation = Quaternion.Euler(_viewData.rotation);
            _rectTransform.localScale = _viewData.scale;
        }

        gameObject.name = _viewData.name  + "_"+ _viewData.type;

        if (TryGetComponent<RawImage>(out var image))
        {
            image.color = _viewData.backgroundColor;
        }
    }

    public override void Setup()
    {
        _verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
        _horizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();

        if (_horizontalLayoutGroup != null)
        {
            AddCells();
        }

        else
        {
            AddLayouts();
        }

        if (transform.GetSiblingIndex() == 0 && transform.parent)
        {
            _rectTransform = transform.parent.GetComponent<RectTransform>();
        }
    }

    public void AddLayouts()
    {
        int count = 0;
        childs.Clear();
        _viewData.childs.Clear();

        foreach (Transform t in transform)
        {
            if (t.TryGetComponent<LayoutObject>(out var h) && t != this.transform)
            {
                childs.Add(h);
                h.AddCells();
                _viewData.childs.Add(h._viewData);
                count++;
            }
        }
    }

    private void AddCells()
    {
        _viewData.cells.Clear();
        childs.Clear();

        int count = 0;

        foreach (Transform t in transform)
        {
            if (t.TryGetComponent<ContainerObject>(out var c) && t != this.transform)
            {
                c.AddChild();
                c._viewData.name = (count + 1).ToString();
                c.name = c._viewData.name + "_" + c._viewData.type;
                childs.Add(c);
                _viewData.cells.Add(c._viewData);
                count++;
            }
        }

        _loaded = true;
    }

    //private void SetSectionAlignment(TemplateObject section, Alignment alignment)
    //{
    //    TextAnchor contentalignment = TextAnchor.MiddleCenter;

    //    switch (alignment)
    //    {
    //        case Alignment.Left:
    //            contentalignment = TextAnchor.MiddleLeft; break;
    //        case Alignment.Right:
    //            contentalignment = TextAnchor.MiddleRight; break;
    //        case Alignment.Center:
    //            contentalignment = TextAnchor.MiddleCenter; break;

    //    }

    //    if (section.TryGetComponent<HorizontalLayoutGroup>(out var horizontalLayoutGroup))
    //    {
    //        horizontalLayoutGroup.childAlignment = contentalignment;
    //    }

    //    else
    //    {
    //        Debug.Log("no horizontal layout found");
    //    }
    //}
}
