using System;

[Serializable]
public class GameData : BaseSaveLoadData
{
    public StoredDictionary<LevelType, int> LevelsData;
    
    
    public GameData() : base()
    {
        
    }
    
    protected override void LoadInternal()
    {
        LevelsData = new StoredDictionary<LevelType, int>(this);
    }

    protected override void LoadFromJson()
    {
        var data = GetData<GameData>();
        LevelsData = data.LevelsData;
        
        LevelsData.SetSaveLoad(this);
    }
    
    public void CompleteLevel(LevelType type, int levelNumber)
    {
        if (LevelsData.ContainsKey(type))
        {
            if (LevelsData[type] < levelNumber) LevelsData[type] = levelNumber;
        }
        else
        {
            LevelsData.Add(type, levelNumber);
        }
    }
}