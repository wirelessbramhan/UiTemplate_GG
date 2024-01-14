using System;
using System.Collections.Generic;
using UnityEngine;

public enum ElementType
{
    root,
    button,
    icon,
    text,
    heading
}

public class ElementSpawner : MonoBehaviour
{
    [SerializeField]
    private List<TemplateObject> templates = new List<TemplateObject>();
    [SerializeField]
    private TemplateObject _root;
    public static ElementType _type;
    [SerializeField]
    private TemplateDataSO _dataSO;

    public static event Action OnRootSpawnEvent;
    public static event Action OnElementSpawnEvent;

    private void OnEnable()
    {
        OnRootSpawnEvent += SpawnRoot;
        OnElementSpawnEvent += SpawnObjects;
    }

    private void OnDisable()
    {
        OnRootSpawnEvent -= SpawnRoot;
        OnElementSpawnEvent -= SpawnObjects;
    }
    private void Start()
    {
        _dataSO.SetData(_root._data);
    }

    [ContextMenu("spawn root")]
    private void SpawnRoot()
    {
        _root = Instantiate(templates[0], transform);
    }

    [ContextMenu("spawn objects")]
    private void SpawnObjects()
    {
        switch(_type)
        {
            case ElementType.button:
                Instantiate(templates[1], _root.transform); break;
            case ElementType.text:
                Instantiate(templates[2], _root.transform); break;
            case ElementType.icon:
                Instantiate(templates[3], _root.transform); break;
            case ElementType.heading:
                Instantiate(templates[4], _root.transform); break;
        }
    }

    public static void RaiseRootSpawnEvent()
    {
        OnRootSpawnEvent?.Invoke();
    }

    public static void RaiseElementSpawnEvent()
    {
        OnElementSpawnEvent?.Invoke();
    }

    public static void SetType(ElementType type)
    {
        _type = type;
    }
}
