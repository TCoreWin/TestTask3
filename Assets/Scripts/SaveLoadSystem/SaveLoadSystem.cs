using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class SaveLoadSystem : Singleton<SaveLoadSystem>
{
    [SerializeField] SaveData saveData = new SaveData();
    [SerializeField] SaveList saveList = new SaveList();

    private UIContent[] uiContent;
    private string saveListKey = "SaveList";
    private string saveDataKey = "SaveData";

    private Action onLoad;

    public SaveList SaveListP
    {
        get { return saveList; }
    }

    private void Awake()
    {
        uiContent = FindObjectsOfType<UIContent>();
        
        if (PlayerPrefs.HasKey(saveListKey))
        {
            saveList = JsonUtility.FromJson<SaveList>(PlayerPrefs.GetString(saveListKey));
        }
    }

    public void Save()
    {
        for (int i = 0; i < saveList.saveIndex.Length; i++)
        {
            if (saveList.saveIndex[i] == false)
            {
                saveList.saveIndex[i] = true;
                saveList.time[i] = System.DateTime.Now.ToString(CultureInfo.CurrentCulture);
                
                FillingSaveData(i);
                
                PlayerPrefs.SetString(saveListKey, JsonUtility.ToJson(saveList));
                
                return;
            }
        }

        List<bool> saveListTempBools = saveList.saveIndex.ToList();
        saveListTempBools.Add(true);

        List<string> saveListTempTime = saveList.time.ToList();
        saveListTempTime.Add(System.DateTime.Now.ToString(CultureInfo.CurrentCulture));
        
        saveList.saveIndex = saveListTempBools.ToArray();
        saveList.time = saveListTempTime.ToArray();
        
        int index = saveList.saveIndex.Length - 1;
        
        FillingSaveData(index);
        
        PlayerPrefs.SetString(saveListKey, JsonUtility.ToJson(saveList)); 
    }

    void FillingSaveData(int index)
    {
        SaveData saveDataTemp = new SaveData();
        
        UIElemt[] uiElemts = UIPanelContent.Instance.UiElementOnField.ToArray();

        saveDataTemp.type = new int[uiElemts.Length];
        saveDataTemp.position = new float[uiElemts.Length * 3];

        for (int i = 0; i < uiElemts.Length; i++)
        {
            saveDataTemp.type[i] = (int) uiElemts[i].Type;
            saveDataTemp.position[i * 3] = uiElemts[i].RectTransformUIElement.localPosition.x;
            saveDataTemp.position[i * 3 + 1] = uiElemts[i].RectTransformUIElement.localPosition.y;
            saveDataTemp.position[i * 3 + 2] = uiElemts[i].RectTransformUIElement.localPosition.z;
        }
        
        PlayerPrefs.SetString(saveDataKey + index, JsonUtility.ToJson(saveDataTemp));
    }

    public void DeleteSaveData(int index)
    {
        saveList.saveIndex[index] = false;
        saveList.time[index] = "";
        
        PlayerPrefs.DeleteKey(saveDataKey + index);
        PlayerPrefs.SetString(saveListKey, JsonUtility.ToJson(saveList)); 
    }
    
    public void Load(int index)
    {
        UIPanelContent.Instance.ClearField();
        
        saveData = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString(saveDataKey + index));

        int posIndex = 0;
        for (int i = 0; i < uiContent.Length; i++)
        {
            for (int j = 0; j < saveData.type.Length; j++)
            {
                if ((int) uiContent[i].UiPoolType == saveData.type[j])
                {
                    //TODO: uiContent -> Spawn object with saveDataPos posx => j*3
                    Vector3 spawnPosition = new Vector3(saveData.position[j*3],saveData.position[j*3+1], saveData.position[j*3+2]);
                    uiContent[i].SpawnUIElements(spawnPosition);
                }
            }
        }
    }
}
