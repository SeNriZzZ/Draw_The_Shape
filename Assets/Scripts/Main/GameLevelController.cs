
using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class GameLevelController : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private LevelTextHeader levelTextHeader;
    [SerializeField] private Timer timer;
    [SerializeField] private List<ShapesByType> shapesByTypes = new List<ShapesByType>();

    private Shape currentShape;
    private int currentLevel;
    private LevelType currentType;

    private void Start()
    {
        currentLevel = GameController.Instance.CurrentLevelNumber;
        currentType = GameController.Instance.CurrentLevelType;
        LoadShape();
        GameController.Instance.OnLevelLoaded += OnLevelLoaded;
        timer.OnSmallDelay += Timer_OnSmallDelay;
        timer.OnBigDelay += Timer_OnBigDelay;
        timer.OnAction += Timer_OnAction;
    }

    private void OnDestroy()
    {
        GameController.Instance.OnLevelLoaded -= OnLevelLoaded;
    }

    private void OnLevelLoaded(int _)
    {
        levelTextHeader.Play(currentType);
    }

    private void Timer_OnSmallDelay()
    {
        levelTextHeader.Play(currentType);
    }
    private void Timer_OnAction()
    {
        currentShape.HideFinger();
    }

    private void Timer_OnBigDelay()
    {
        currentShape.ShowFinger();
    }

    private void OnShapeComplete(Shape shape)
    {
        PerfectText.Instance.ShowPerfect(OnPerfectTextComplete);

        void OnPerfectTextComplete()
        {
            SLS.Data.GameData.CompleteLevel(GameController.Instance.CurrentLevelType, currentLevel);
            currentShape.OnShapeComplete -= OnShapeComplete;
            currentShape = null;
            GetNewShape();
        }
    }

    private void GetNewShape()
    {
        currentLevel++;
        if (currentLevel > DataHolder.Instance.LevelColorData.levelColors.Count)
        {
            currentLevel = 1;
        }
        LoadShape();
    }

    private void LoadShape()
    {
        currentShape = shapesByTypes.First(x => x.Type == currentType).Shape;
        currentShape.SetActive();
        currentShape.ResetShape();
        currentShape.SetColor(DataHolder.Instance.LevelColorData.levelColors.First(x=>x.Level == currentLevel).Color);
        currentShape.OnShapeComplete += OnShapeComplete;
        inputManager.SetShape(currentShape);
    }
}


[Serializable]
public class ShapesByType
{
    public LevelType Type;
    public Shape Shape;
}
