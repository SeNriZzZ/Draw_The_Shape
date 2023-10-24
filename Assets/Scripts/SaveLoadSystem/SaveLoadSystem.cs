using System.Collections;
using UnityEngine;

public static class SLS
{
    public static SaveLoadData Data => SaveLoadSystem.Instance.Data;
    
}

public class SaveLoadSystem : MonoSingleton<SaveLoadSystem>
{
    public SaveLoadData Data { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        Data = new SaveLoadData();
        Data.Load();
        
        DontDestroyOnLoad(this);
    }
}