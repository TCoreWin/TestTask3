using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIElemt : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler, IPoolable
{
    private PoolType type;
    
    private RectTransform rectTransform;
    private Image selectImage;
    
    private Vector2 minPoint;
    private Vector2 maxPoint;
    private Vector3 offset;

    public Vector3 Offset
    {
        get { return offset; }
        set { offset = value; }
    }

    public RectTransform RectTransformUIElement
    {
        get { return rectTransform; }
    }

    public PoolType Type
    {
        get { return type; }
        set { type = value; }
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        selectImage = GetComponent<Image>();
    }

    private void Start()
    {
        selectImage.color = new Color(selectImage.color.r,selectImage.color.g,selectImage.color.b,0);
    }

    public void UpdatePosData()
    {
        Vector2 offset = new Vector2(RectTransformUIElement.rect.width *.5f, RectTransformUIElement.rect.height *.5f);
        
        minPoint = (Vector2) RectTransformUIElement.localPosition - offset;
        maxPoint = (Vector2) RectTransformUIElement.localPosition + offset;
        
        //Debug.Log(minPoint + " / " + maxPoint);
        
        OverAreaCheck();
    }

    void OverAreaCheck()
    {
        var localPosition = RectTransformUIElement.localPosition;
        
        if (minPoint.x < UIPanelContent.Instance.MinUiFieldPpoint.x)
        {
            localPosition = new Vector3(UIPanelContent.Instance.MinUiFieldPpoint.x + RectTransformUIElement.rect.width * .5f, localPosition.y, localPosition.z);
            RectTransformUIElement.localPosition = localPosition;
        }
        if (maxPoint.x > UIPanelContent.Instance.MaxUiFieldPpoint.x)
        {
            localPosition = new Vector3(UIPanelContent.Instance.MaxUiFieldPpoint.x - RectTransformUIElement.rect.width * .5f, localPosition.y, localPosition.z);
            RectTransformUIElement.localPosition = localPosition;
        }
        if (minPoint.y < UIPanelContent.Instance.MinUiFieldPpoint.y)
        {
            localPosition = new Vector3(localPosition.x, UIPanelContent.Instance.MinUiFieldPpoint.y + RectTransformUIElement.rect.height * .5f, localPosition.z);
            RectTransformUIElement.localPosition = localPosition;
        }
        if (maxPoint.y > UIPanelContent.Instance.MaxUiFieldPpoint.y)
        {
            localPosition = new Vector3(localPosition.x, UIPanelContent.Instance.MaxUiFieldPpoint.y - RectTransformUIElement.rect.height * .5f, localPosition.z);
            RectTransformUIElement.localPosition = localPosition;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {   
        Vector2 pos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(UIPanelContent.Instance.MCanvasRect, eventData.position,
            eventData.enterEventCamera, out pos);

        RectTransformUIElement.localPosition = (Vector3)pos - offset;
        
        if(UIPanelContent.Instance.CountSelectedElements() > 0) UIPanelContent.Instance.MoveSelectedObjects(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector2 mouseLocalPos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(UIPanelContent.Instance.MCanvasRect, eventData.position,
            eventData.enterEventCamera, out mouseLocalPos);

        Offset = (Vector3) mouseLocalPos - RectTransformUIElement.localPosition;
        
        UIPanelContent.Instance.RecountOffset(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        UIPanelContent.Instance.UpdatePosDataAllElements();
        UpdatePosData();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            UIPanelContent.Instance.AddSelectedElement(this);
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            UIPanelContent.Instance.DeleteSpawnElement(this);
        }
    }

    public void Despawn()
    {
        PoolManager.Instance.Despawn(type, gameObject);
    }

    public void OnSpawn()
    {
        UIPanelContent.Instance.AddSpawnElement(this);
    }

    public void OnDespawn()
    {
        selectImage.color = new Color(selectImage.color.r,selectImage.color.g,selectImage.color.b,0);
    }

    public void SetSelect(bool value)
    {
        if(value) selectImage.color = new Color(selectImage.color.r,selectImage.color.g,selectImage.color.b,1);
        else selectImage.color = new Color(selectImage.color.r,selectImage.color.g,selectImage.color.b,0);
    }
}
