using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLevelsProvider : MonoBehaviour, ILevelProvider
{
    [SerializeField] private bool activateNewScene;

    private int currentScene = -1;

    public int LevelsCount => SceneManager.sceneCountInBuildSettings - 1;

    public event Action<int> OnLevelLoaded;

    public void LoadLevel(int levelNumber)
    {
        StartCoroutine(LoadLevelRoutine(levelNumber));
    }

    private IEnumerator LoadLevelRoutine(int levelNumber)
    {
#if UNITY_EDITOR
        if (SceneTester.Enabled)
        {
            yield return StartCoroutine(LoadTestSceneRoutine());
            currentScene = SceneManager.GetSceneAt(1).buildIndex;
        }
        else
#endif
        {
            yield return UnloadScenes(1);
            currentScene = levelNumber;
            yield return StartCoroutine(LoadSceneRoutine());
        }

        yield return null;

        OnLevelLoaded?.Invoke(currentScene);
    }

    private IEnumerator LoadTestSceneRoutine()
    {
        yield return UnloadScenes(2);
        yield return ReloadTestSceneRoutine();
    }

    private IEnumerator ReloadTestSceneRoutine()
    {
        var scene = SceneManager.GetSceneAt(1);
        var path = scene.path;

        yield return SceneManager.UnloadSceneAsync(scene);
        yield return SceneManager.LoadSceneAsync(path, LoadSceneMode.Additive);

        if (activateNewScene)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
        }

        yield return null;
    }

    private IEnumerator UnloadSceneRoutine()
    {
        if (currentScene > 0)
        {
            yield return SceneManager.UnloadSceneAsync(currentScene);
        }
    }

    private IEnumerator LoadSceneRoutine()
    {
        yield return SceneManager.LoadSceneAsync(currentScene, LoadSceneMode.Additive);
        if (activateNewScene)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
        }
    }

    private IEnumerator UnloadScenes(int startFrom)
    {
        for (var i = startFrom; i < SceneManager.sceneCount; i++)
        {
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
        }
    }
}