using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPanelContent : Singleton<UIPanelContent>, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    private RectTransform uiField;

    public RectTransform UIField
    {
        get { return uiField; }
    }
    
    private void Awake()
    {
        uiField = GameObject.Find("UIField").GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log(gameObject);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }
}
