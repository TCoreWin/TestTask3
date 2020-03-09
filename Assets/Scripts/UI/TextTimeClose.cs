using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.UI;

public class TextTimeClose : MonoBehaviour
{
    private Text text;
    
    void Awake()
    {
        text = GetComponent<Text>();
        
        text.CrossFadeAlpha(0, 1f, true);
        StartCoroutine(DeleteObject());
    }

    IEnumerator DeleteObject()
    {
        yield return new WaitForSeconds(1.1f);
        Destroy(gameObject);
    }
}
