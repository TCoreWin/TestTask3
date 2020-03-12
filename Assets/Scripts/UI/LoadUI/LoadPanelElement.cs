using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadPanelElement : MonoBehaviour
{
    private Text text;
    private Button loadButton;
    private Button deleteButton;
    private int index;
    
    private void Awake()
    {
        text = GetComponentInChildren<Text>();
        loadButton = GetComponentInChildren<Button>();
        deleteButton = GetComponentsInChildren<Button>()[1];
        
        loadButton.onClick.AddListener(LoadSaveData);
        deleteButton.onClick.AddListener(DeleteSaveData);
    }

    public void SetText(string data, int index)
    {
        this.text.text = index.ToString() + "_Дата: " + data;
        this.index = index;
    }

    void DeleteSaveData()
    {
        SaveLoadSystem.Instance.DeleteSaveData(index);
        
        Destroy(gameObject);
    }

    void LoadSaveData()
    {
        SaveLoadSystem.Instance.Load(index);
        
        LoadPanel.Instance.GetComponentInParent<Popup>().Close();
    }
}
