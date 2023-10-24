using System;

public interface ILevelProvider
{
    event Action<int> OnLevelLoaded;
    int LevelsCount { get; }
    void LoadLevel(int number);
}