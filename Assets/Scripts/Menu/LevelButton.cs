
using UnityEngine;
using UnityEngine.UI;

public enum LevelType
{
    Letter,
    Number,
    Circle
}
public class LevelButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private int levelNumber;
    [SerializeField] private LevelType levelType;

    private void Start()
    {
        button.onClick.AddListener(StartLevel);
        Init();
    }

    private void Init()
    {
        if (SLS.Data.GameData.LevelsData.ContainsKey(levelType))
        {
            button.interactable = SLS.Data.GameData.LevelsData[levelType] >= levelNumber - 1;
        }
        else button.interactable = levelNumber == 1;
    }

    private void StartLevel()
    {
        GameController.Instance.SetLevelData(levelType, levelNumber);
        GameController.Instance.LoadGame();
    }
    
}
