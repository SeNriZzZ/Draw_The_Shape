using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shape : MonoBehaviour
{
    [SerializeField] private List<Path> paths = new List<Path>();
    [SerializeField] private List<Image> fillImages = new List<Image>();

    private Path currentPath;
    private int pathIndex = 0;

    public Path CurrentPath => currentPath;
    public bool isComplete => pathIndex >= paths.Count;

    public event Action<Shape> OnShapeComplete;

    private void Start()
    {
        currentPath = paths[pathIndex];
    }
    
    public void CompletePath()
    {
        currentPath.HideRoute();
        currentPath.HideFinger();
        pathIndex++;
        if (pathIndex >= paths.Count)
        {
            OnShapeComplete?.Invoke(this);
        }
        else
        {
            currentPath = paths[pathIndex];
            currentPath.ShowRoute();
        }
    }

    public void SetColor(Color colorToSet)
    {
        foreach (var image in fillImages)
        {
            image.color = colorToSet;
        }
    }

    public void ResetShape()
    {
        pathIndex = 0;
        currentPath = paths[pathIndex];
        currentPath.ShowRoute();
        foreach (var path in paths)
        {
            path.ResetPath();
        }
    }

    public void ShowFinger()
    {
        currentPath.MoveFinger();
    }

    public void HideFinger()
    {
        currentPath.HideFinger();
    }
}

