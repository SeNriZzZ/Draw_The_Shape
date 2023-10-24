using System;
using UnityEngine;

public class LevelsController : MonoSingleton<LevelsController>
{
    [SerializeField]
    private int levelOffset;

    private ILevelProvider levelProvider;

    public int LevelsCount => levelProvider != null ? levelProvider.LevelsCount : 0;

    public event Action<int> OnLevelLoaded;

    private void OnValidate()
    {
        if (levelProvider == null)
            levelProvider = GetComponent<ILevelProvider>();

        levelOffset = Mathf.Clamp(levelOffset, 0, levelProvider.LevelsCount - 1);
    }

    protected override void Awake()
    {
        base.Awake();

        levelProvider = GetComponent<ILevelProvider>();

        if (levelProvider == null)
            Debug.LogWarning("LevelProvider is not selected!!!", this);

        levelProvider.OnLevelLoaded += LevelProvider_OnLevelLoaded;
    }

    public void LoadLevel(int levelNumber)
    {
        levelProvider?.LoadLevel(AdjustLevelNumber(levelNumber));
    }

    private void LevelProvider_OnLevelLoaded(int sceneId)
    {
        OnLevelLoaded?.Invoke(sceneId);
    }

    private int AdjustLevelNumber(int levelNumber)
    {
        int levelIndex = levelNumber - 1;
        if (levelNumber > levelProvider.LevelsCount && levelOffset > 0)
        {
            levelIndex = (levelIndex - levelOffset) % (levelProvider.LevelsCount - levelOffset) + levelOffset;
        }
        else
        {
            levelIndex = levelIndex % levelProvider.LevelsCount;
        }
        return levelIndex + 1;
    }
}