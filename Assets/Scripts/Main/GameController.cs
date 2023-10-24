using System;
using UnityEngine;

public class GameController : MonoSingleton<GameController>
{
    public event Action<int> OnShowFadeUI;
    public event Action<int> OnLevelLoaded;

    private const int MENU_LEVEL = 1;
    private const int GAME_LEVEL = 2;

    private LevelType currentLevelType;
    private int currentLevelNumber;

    public LevelType CurrentLevelType => currentLevelType;
    public int CurrentLevelNumber => currentLevelNumber;
    

    private void Start()
    {
        LevelsController.Instance.OnLevelLoaded += LevelsController_LevelLoaded;
        Application.targetFrameRate = 60;
        LateStart();
    }

    private void LateStart()
    {
        this.DoAfterNextFrameCoroutine(() =>
        {
            LoadLevel(1);
        });
    }

    private void LevelsController_LevelLoaded(int levelNumber)
    {
        OnLevelLoaded?.Invoke(levelNumber);
    }

    public void InvokeOnStartLevelLoading(int levelNumber)
    {
        LevelsController.Instance.LoadLevel(levelNumber);
    }
    
    private void LoadLevel(int level)
    {
        OnShowFadeUI?.Invoke(level);
    }

    public void LoadMenu()
    {
        LoadLevel(MENU_LEVEL);
    }

    public void LoadGame()
    {
        LoadLevel(GAME_LEVEL);
    }

    public void SetLevelData(LevelType levelType, int levelNumber)
    {
        currentLevelType = levelType;
        currentLevelNumber = levelNumber;
    }
    
}