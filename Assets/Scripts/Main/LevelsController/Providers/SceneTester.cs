#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTester
{
    public static bool Enabled { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnBeforeSceneLoadRuntimeMethod()
    {
        if (!Enabled)
            return;

        if (SceneManager.GetSceneAt(0).buildIndex == 0)
        {
            Enabled &= SceneManager.sceneCount > 1;
            return;
        }

        var scene = SceneManager.GetSceneAt(0);
        var path = scene.path;
        SceneManager.UnloadSceneAsync(scene);
        SceneManager.LoadScene(0);
        SceneManager.LoadScene(path, LoadSceneMode.Additive);
    }

    public static void SetEnabled(bool enabled)
    {
        Enabled = enabled;
    }
}
#endif