using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelColorData", menuName = "SO/LevelColorData", order = 1)]
public class LevelColorData : ScriptableObject
{
    public List<LevelColor> levelColors = new List<LevelColor>();
}

[Serializable]
public class LevelColor
{
    public int Level;
    public Color Color;
}
