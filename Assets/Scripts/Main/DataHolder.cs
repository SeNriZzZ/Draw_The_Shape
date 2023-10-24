
using UnityEngine;

public class DataHolder : MonoSingleton<DataHolder>
{
    [SerializeField] private LevelColorData levelColorData;

    public LevelColorData LevelColorData => levelColorData;
}
