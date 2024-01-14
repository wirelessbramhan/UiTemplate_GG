using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TemplateIcon : TemplateObject
{
    public ViewTemplateData _viewData;
    private Image _outline, _bg;
    public override void Configure()
    {
        if (TryGetComponent(out _rectTransform))
        {
            _rectTransform.sizeDelta = new Vector2(_viewData.width, _viewData.height);
            _rectTransform.anchoredPosition = _viewData.position;
            _rectTransform.rotation = Quaternion.Euler(_viewData.rotation);
            _rectTransform.localScale = _viewData.scale;
        }

        if (TryGetComponent(out _outline))
        {
            _outline.color = _viewData.outline;
        }

        if (_bg != null)
        {
            _bg.color = _viewData.background;
        }
    }

    public override void Setup()
    {
        _viewData.type = "icon";
        gameObject.name = _viewData.name + "_" + _viewData.type;

        _bg = GetComponentsInChildren<Image>()[1];
    }
}
