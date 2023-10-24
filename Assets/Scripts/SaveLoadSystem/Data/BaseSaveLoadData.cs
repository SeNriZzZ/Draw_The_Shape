using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseSaveLoadData : ISaveLoad
{
    private readonly string DataKey;

    public BaseSaveLoadData()
    {
        DataKey = GetType().Name;
    }

    public void Load()
    {
        string ppData = PlayerPrefs.GetString(DataKey, "");
        if (string.IsNullOrEmpty(ppData))
        {
            LoadInternal();
        }
        else
        {
            LoadFromJson();
        }
    }

    protected virtual void LoadInternal()
    {

    }

    protected virtual void LoadFromJson()
    {

    }

    protected T GetData<T>() where T : BaseSaveLoadData
    {
        string ppData = PlayerPrefs.GetString(DataKey, "");
        var data = JsonUtility.FromJson<T>(ppData);
        return data;
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(this);
        PlayerPrefs.SetString(DataKey, json);
    }
}