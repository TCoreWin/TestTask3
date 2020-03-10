using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPanelContent : Singleton<UIPanelContent>, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform uiField;
    private RectTransform m_canvas_rect;
    private Vector2 minUIFieldPpoint;
    private Vector2 maxUIFieldPpoint;
    private Transform m_canvas;
    private SelectedBox selectedBox;
    
    [SerializeField] List<UIElemt> uiElementList = new List<UIElemt>();
    [SerializeField] List<UIElemt> uiElementOnField = new List<UIElemt>();
    
    public RectTransform UIField
    {
        get => uiField;
    }

    public Vector2 MinUiFieldPpoint
    {
        get => minUIFieldPpoint;
    }

    public Vector2 MaxUiFieldPpoint
    {
        get => maxUIFieldPpoint;
    }

    public Transform MCanvas
    {
        get { return m_canvas; }
    }

    public RectTransform MCanvasRect
    {
        get { return m_canvas_rect; }
    }

    private void Awake()
    {
        uiField = GameObject.Find("UIField").GetComponent<RectTransform>();

        m_canvas = GameObject.Find("Canvas").transform;
        m_canvas_rect = MCanvas.GetComponent<RectTransform>();
        selectedBox = FindObjectOfType<SelectedBox>();
        
        InitUIFieldMinMaxPoint();
    }

    private void InitUIFieldMinMaxPoint()
    {
        Vector2 offset = new Vector2(uiField.rect.width * .5f, uiField.rect.height * .5f);

        minUIFieldPpoint = (Vector2) uiField.localPosition - offset;
        maxUIFieldPpoint = (Vector2) uiField.localPosition + offset;
    }


    public void AddSelectedElement(UIElemt element)
    {
        if(uiElementList.Contains(element)) return;

        uiElementList.Add(element);
    }

    public void AddSpawnElement(UIElemt element)
    {
        uiElementOnField.Add(element);
    }
    
    public void DeleteSpawnElement(UIElemt element)
    {
        element.Despawn();
        uiElementOnField.Remove(element);
    }

    public void MoveSelectedObjects(UIElemt currentMoveElement)
    {   
        if (uiElementList.Count == 0) return;
        
        for (int i = 0; i < uiElementList.Count; i++)
        {
            if(uiElementList[i] == currentMoveElement) continue;

            uiElementList[i].RectTransformUIElement.localPosition =
                currentMoveElement.RectTransformUIElement.localPosition - uiElementList[i].Offset;
        }
    }

    public void RecountOffset(UIElemt currentMoveElement)
    {
        if(uiElementList.Count == 0) return;
        
        for (int i = 0; i < uiElementList.Count; i++)
        {
            if(currentMoveElement == uiElementList[i]) continue;

            uiElementList[i].Offset = currentMoveElement.RectTransformUIElement.localPosition - uiElementList[i].RectTransformUIElement.localPosition;
        }
    }

    public void UpdatePosDataAllElements()
    {
        if(uiElementList.Count == 0) return;
        
        for (int i = 0; i < uiElementList.Count; i++)
        {
            uiElementList[i].UpdatePosData();
        }
    }
    
    public int CountSelectedElements()
    {
        return uiElementList.Count;
    }

    private void ClearSelectedElements()
    {
        uiElementList.Clear();
    }

    public void CleatField()
    {
        if(uiElementOnField.Count == 0) return;

        int count = uiElementOnField.Count;
        
        for (int i = 0; i < count; i++)
        {
            uiElementOnField[i].Despawn();
        }
        
        uiElementOnField.Clear();
    }
    
    private Vector2 MousePos(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(MCanvasRect, eventData.position,
            eventData.enterEventCamera, out pos);
        return pos;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        //ClearSelectedElements();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector2 startPos = MousePos(eventData);
        SelectedBox.Instance.StartPos = startPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 endPos = MousePos(eventData);
        SelectedBox.Instance.EndPos = endPos;

        uiElementList = SelectedBox.Instance.CheckContainsUIElements(uiElementOnField);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 endPos = MousePos(eventData);
        SelectedBox.Instance.EndPos = endPos;
        
        SelectedBox.Instance.DrawSelectedBox();
    }
    
    
}
