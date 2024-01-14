using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TemplateButton : TemplateObject
{
    public string _type = "button";
    public ViewTemplateData _viewData;
    [TextArea]
    private TextMeshProUGUI _text;
    private Image _outline;
    private Button _button;
    public override void Configure()
    {
        if (TryGetComponent(out _rectTransform))
        {
            _rectTransform.sizeDelta = new Vector2(_viewData.width, _viewData.height);
            _rectTransform.anchoredPosition = _viewData.position;
            _rectTransform.rotation = Quaternion.Euler(_viewData.rotation);
            _rectTransform.localScale = _viewData.scale;
        }

        _text = GetComponentInChildren<TextMeshProUGUI>();

        if (_text != null)
        {
            _text.text = _viewData.text;
            _text.fontSize = _viewData.textSize;
        }

        if (_button)
        {
            
        }

        if (TryGetComponent(out _outline))
        {
            _outline.color = _viewData.outline;
        }

        var child = GetComponentsInChildren<Image>();

        if (child != null)
        {
            child[1].color = _viewData.background;

            if (child[1].TryGetComponent<RectTransform>(out var icon))
            {
                child[1].GetComponent<RectTransform>().offsetMax = new Vector2(-_viewData.padding, -_viewData.padding);
                child[1].GetComponent<RectTransform>().offsetMin = new Vector2(_viewData.padding, _viewData.padding);
            }

        }

        gameObject.name = _viewData.name + "_" + _type;
        _viewData.type = _type;

        _data = _viewData;
    }

    public override void Setup()
    {
        _button = GetComponentInChildren<Button>();
        
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }
}
