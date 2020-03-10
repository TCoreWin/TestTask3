using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

public class SelectedBox : Singleton<SelectedBox>
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

    private void Update()
    {
           
    }

    //TODO: ПЕРЕДЕЛАТЬ
    public List<UIElemt> CheckContainsUIElements(List<UIElemt> elements)
    {
        border.gameObject.SetActive(false);
        
        List<UIElemt> uiElemts = new List<UIElemt>();
        Rect selectedRect = new Rect((startPos.x + endPos.x) * .5f, (startPos.y + endPos.y)*.5f, Mathf.Abs(startPos.x - endPos.x), Mathf.Abs(startPos.y - endPos.y));
        
        for (int i = 0; i < elements.Count; i++)
        {
            if (RectContains(selectedRect, elements[i].RectTransformUIElement.localPosition))
            {
                uiElemts.Add(elements[i]);
            }
        }
        
        return uiElemts;
    }

    bool RectContains(Rect rect, Vector2 point)
    {
        return rect.x - rect.width *.5f < point.x && rect.x + rect.width * .5f > point.x && rect.y - rect.height * .5f < point.y && rect.y + rect.height * .5f > point.y;
    }
    
    public void DrawSelectedBox()
    {
        if(!border.gameObject.activeInHierarchy)
            border.gameObject.SetActive(true);
        
        Vector2 centre = (startPos + endPos) * .5f;

        border.localPosition = centre;
        
        float width = Mathf.Abs(startPos.x - endPos.x);
        float height = Mathf.Abs(startPos.y - endPos.y);
        
        border.sizeDelta = new Vector2(width, height);
    }
}
