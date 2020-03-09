using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

public class SelectedBox : MonoBehaviour
{
    private RectTransform border;
    private Vector2 startPos;
    private Vector2 endPos;
    private RectTransform m_canvas_rect;
    
    public Vector2 StartPos
    {
        get { return startPos; }
        set { startPos = value; }
    }

    public Vector2 EndPos
    {
        get { return endPos; }
        set { endPos = value; }
    }

    private void Awake()
    {
        border = GameObject.Find("Border").GetComponent<RectTransform>();
        
        border.gameObject.SetActive(false);
    }

    public void UpdateRectBox()
    {
        
    }

    
    //TODO: ПЕРЕДЕЛАТЬ
    public List<UIElemt> CheckContainsUIElements(List<UIElemt> elements)
    {
        List<UIElemt> uiElemts;
        Vector2 endPosMouse;
        Rect selectedRect = new Rect(startPos.x, startPos.y, startPos.x - endPos.x, startPos.y - endPos.y);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_canvas_rect, endPos, Camera.main,
            out endPosMouse);
        
        for (int i = 0; i < elements.Count; i++)
        {
            if (selectedRect.Contains(border.worldToLocalMatrix.MultiplyPoint(Camera.main.ScreenToWorldPoint(elements[i].RectTransformUIElement.localPosition))));
        }
        
        return null;
    }
}
