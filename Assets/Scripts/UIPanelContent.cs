using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.PlayerLoop;

public class UIPanelContent : Singleton<UIPanelContent>, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform uiField;
    private RectTransform m_canvas_rect;
    private Vector2 minUIFieldPpoint;
    private Vector2 maxUIFieldPpoint;
    private Transform m_canvas;
    private SelectedBox selectedBox;
    
    [SerializeField] List<UIElemt> uiSelectedElementList = new List<UIElemt>();
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

    private void Update()
    {
        if (Input.GetKey(KeyCode.Delete))
        {
            for (int i = 0; i < uiSelectedElementList.Count; i++)
            {
                if (uiElementOnField.Contains(uiSelectedElementList[i]))
                {
                    DeleteSpawnElement(uiSelectedElementList[i]);
                }
            }
            
            uiSelectedElementList.Clear();
        }
    }

    private void InitUIFieldMinMaxPoint()
    {
        Vector2 offset = new Vector2(uiField.rect.width * .5f, uiField.rect.height * .5f);

        minUIFieldPpoint = (Vector2) uiField.localPosition - offset;
        maxUIFieldPpoint = (Vector2) uiField.localPosition + offset;
    }


    public void AddSelectedElement(UIElemt element)
    {
        if(uiSelectedElementList.Contains(element)) return;

        uiSelectedElementList.Add(element);
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
        for (int i = 0; i < uiSelectedElementList.Count; i++)
        {
            if(uiSelectedElementList[i] == currentMoveElement) continue;

            uiSelectedElementList[i].RectTransformUIElement.localPosition =
                currentMoveElement.RectTransformUIElement.localPosition - uiSelectedElementList[i].Offset;
        }
    }

    public void RecountOffset(UIElemt currentMoveElement)
    {
        for (int i = 0; i < uiSelectedElementList.Count; i++)
        {
            if(currentMoveElement == uiSelectedElementList[i]) continue;

            uiSelectedElementList[i].Offset = currentMoveElement.RectTransformUIElement.localPosition - uiSelectedElementList[i].RectTransformUIElement.localPosition;
        }
    }

    public void UpdatePosDataAllElements()
    {
        for (int i = 0; i < uiSelectedElementList.Count; i++)
        {
            uiSelectedElementList[i].UpdatePosData();
        }
    }
    
    public int CountSelectedElements()
    {
        return uiSelectedElementList.Count;
    }

    private void ClearSelectedElements()
    {
        for (int i = 0; i < uiSelectedElementList.Count; i++)
        {
            uiSelectedElementList[i].SetSelect(false);
        }
        
        uiSelectedElementList.Clear();
    }

    public void ClearField()
    {
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
    
    void FillingUIElementList()
    {
        List<UIElemt> allSelectedElements = SelectedBox.Instance.CheckContainsUIElements(uiElementOnField);

        for (int i = 0; i < allSelectedElements.Count; i++)
        {
            if (!uiSelectedElementList.Contains(allSelectedElements[i]))
            {
                uiSelectedElementList.Add(allSelectedElements[i]);
                allSelectedElements[i].SetSelect(true);
            }
        }
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

        FillingUIElementList();
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 endPos = MousePos(eventData);
        SelectedBox.Instance.EndPos = endPos;
        
        SelectedBox.Instance.DrawSelectedBox();
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (!Input.GetKey(KeyCode.LeftShift))
            ClearSelectedElements();
    }
}
