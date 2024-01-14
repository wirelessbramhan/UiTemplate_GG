using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TemplateData Container", menuName = "UI/Template Holder", order = 1)]
public class TemplatePrefabHolderSO : ScriptableObject
{
    [SerializeField] private GameObject Root;
    [SerializeField] private TemplateObject container;
    [SerializeField] private List<TemplateObject> templateObjects;

    public static event Action OnInstantiateRaised;

    public static void RaiseInstantitateEvent()
    {
        OnInstantiateRaised?.Invoke();
    }
}
