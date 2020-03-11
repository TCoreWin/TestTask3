using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIContent : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private PoolType poolType;
    [SerializeField] private GameObject prefab;
    [Space(5)]
    [SerializeField] private PoolType uiPoolType;
    [SerializeField] private GameObject uiPrefab;
    
    private GameObject viewDragPoolObj;

    public PoolType UiPoolType
    {
        get { return uiPoolType; }
    }

    private void Start()
    {
        PoolManager.Instance.AddPool(poolType).Populate(prefab, 1);
        PoolManager.Instance.AddPool(UiPoolType).Populate(uiPrefab, 10);
    }

    public void OnDrag(PointerEventData eventData)
    {   
        RectTransformUtility.ScreenPointToLocalPointInRectangle(UIPanelContent.Instance.MCanvasRect, eventData.position, eventData.enterEventCamera, out var pos);
        viewDragPoolObj.transform.localPosition = pos;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        viewDragPoolObj = PoolManager.Instance.Spawn(poolType, prefab);
        viewDragPoolObj.transform.SetParent(UIPanelContent.Instance.MCanvas);
        viewDragPoolObj.transform.localScale = Vector3.one;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        PoolManager.Instance.Despawn(poolType, viewDragPoolObj);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(UIPanelContent.Instance.MCanvasRect, eventData.position, eventData.enterEventCamera, out var pos);
        bool onField = OnField(eventData.position, UIPanelContent.Instance.UIField, eventData.enterEventCamera);
        
        if (onField)
        {
            var uiGo = PoolManager.Instance.Spawn(UiPoolType, uiPrefab);
            uiGo.transform.SetParent(UIPanelContent.Instance.MCanvas);
            uiGo.transform.localScale = Vector3.one;
            uiGo.transform.localPosition = pos;
            
            UIElemt scrips = uiGo.GetComponent<UIElemt>();
            scrips.UpdatePosData();
            scrips.Type = UiPoolType;
        }
    }

    bool OnField(Vector2 cursorPos, RectTransform field, Camera cam)
    {   
        if (field.rect.Contains(field.worldToLocalMatrix.MultiplyPoint(cam.ScreenToWorldPoint(cursorPos))))
            return true;
        return false;
    }
}
