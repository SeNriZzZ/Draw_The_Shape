using System;

public interface ISaveLoad
{
    void Load();
    void Save();
}


[Serializable]
public class SaveLoadData
{
    public GameData GameData;

    public SaveLoadData()
    {
        GameData = new GameData();
    }
    public void Load()
    {
        GameData.Load();
    }
}