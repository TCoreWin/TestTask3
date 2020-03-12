using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPanel : Singleton<LoadPanel>
{
    [SerializeField] private GameObject prefab;

    private void Awake()
    {
        for (int i = 0; i < SaveLoadSystem.Instance.SaveListP.saveIndex.Length; i++)
        {
            if (SaveLoadSystem.Instance.SaveListP.saveIndex[i])
            {
                LoadPanelElement script = Instantiate(prefab, transform, true).GetComponent<LoadPanelElement>();    
                script.SetText(SaveLoadSystem.Instance.SaveListP.time[i], i);
            }
        }
    }
}
