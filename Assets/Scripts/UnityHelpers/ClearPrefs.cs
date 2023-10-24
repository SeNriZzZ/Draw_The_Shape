#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class ClearPrefs : MonoBehaviour
{
    [MenuItem("Tools/ClearPrefs")]
    public static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("PLAYER PREFS CLEARED");
    }
    
}
#endif
