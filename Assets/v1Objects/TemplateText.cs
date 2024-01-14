using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TemplateText : TemplateObject
{
    public ViewTemplateData _viewData;
    [TextArea]
    private TextMeshProUGUI _text;
    private Image _outline, _bg;
    public override void Configure()
    {
        if (_text != null)
        {
            _text.text = _viewData.text;
            _text.fontSize = _viewData.textSize;
        }

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
    }

    public override void Setup()
    {
        _viewData.type = "text";
        gameObject.name = _viewData.name + "_" + _viewData.type;
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _bg = GetComponentInChildren<Image>();

        if (_bg != null)
        {
            _bg.color = _viewData.background;

            if (_bg.TryGetComponent<RectTransform>(out var rect))
            {
                rect.offsetMax = new Vector2(-_viewData.padding, -_viewData.padding);
                rect.offsetMin = new Vector2(_viewData.padding, _viewData.padding);
            }

            else
            {
                Debug.Log("text bg Rect not found");
            }
        }
    }
}
