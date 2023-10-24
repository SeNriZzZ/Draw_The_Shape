
using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class LevelTextHeader : MonoBehaviour
{
    [SerializeField] private List<LevelTextByType> levelTextByTypes = new List<LevelTextByType>();
    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private float scaleDuration;
    [SerializeField] private float bounceMulti;
    [SerializeField] private int bouncesCount;
    [SerializeField] private float bounceDuration;

    public void Play(LevelType type)
    {
        headerText.text = levelTextByTypes.First(x => x.Type == type).Text;
        headerText.transform.localScale = Vector3.zero;

        var sequence = DOTween.Sequence();
        sequence.Append(headerText.transform.DOScale(Vector3.one, scaleDuration));
        sequence.Append(headerText.transform.DOScale(Vector3.one * bounceMulti, bounceDuration)
            .SetLoops(bouncesCount, LoopType.Yoyo));
        sequence.Append(headerText.transform.DOScale(Vector3.zero, scaleDuration));
    }
}

[Serializable]
public class LevelTextByType
{
    public LevelType Type;
    public string Text;
}
