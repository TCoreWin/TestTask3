using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowScaler : MonoBehaviour
{
    [SerializeField] private RectTransform uiContentTransform;

    private RectTransform rectTransform;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        
        float xSize = Screen.width - uiContentTransform.sizeDelta.x;
        Debug.Log(Screen.width +" "+ xSize + " "+ uiContentTransform.sizeDelta.x);
        Debug.Log(Screen.width);
        //rectTransform.position = new Vector3(0+);
    }
}
