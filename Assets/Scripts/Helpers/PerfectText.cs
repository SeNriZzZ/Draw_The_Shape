
using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PerfectText : MonoSingleton<PerfectText>
{
    [SerializeField] private List<string> perfectPhrases;
    [SerializeField] private TextMeshProUGUI perfectText;
    [SerializeField] private float showTime = 1f;
    [SerializeField] private float jumpScale = 1.5f;
    [SerializeField] private float jumpTime = 0.3f;
    [SerializeField] private Vector2 leftZRotate;
    [SerializeField] private Vector2 rightZRotate;
    [SerializeField] private float rotateTime = 0.1f;
    [SerializeField] private bool unscaledTime;

    private string oldPerfectPhase;
    private string oldOrdinaryPhase;
    private Sequence jumpSequence;
    private Sequence rotateSequence;


    private void Start()
    {
        perfectText.text = string.Empty;
    }

    public void ShowPerfect(Action callback)
    {
        ShowPerfectText(callback, perfectPhrases, ref oldPerfectPhase);
    }
    

    private void ShowPerfectText( Action callback,List<string> phrases, ref string oldPhrase)
    {
        perfectText.text = GetRandomPhase(phrases, ref oldPhrase);
        perfectText.color = perfectText.color.WithAlpha(1f);
        perfectText.SetActive();
        JumpAnimation(callback);
        RotateAnimation();
    }

    private string GetRandomPhase(List<string> phrases, ref string oldPhrase)
    {
        if (phrases.Count == 0)
            phrases.Add("Amazing!");

        if (phrases.Count == 1)
            return phrases.First();

        string randomPhase;
        do
        {
            randomPhase = phrases.GetRandom();
        }
        while (randomPhase == oldPhrase);
        oldPhrase = randomPhase;
        return randomPhase;
    }

    private void JumpAnimation(Action callback)
    {
        jumpSequence?.Kill();
        jumpSequence = DOTween.Sequence();
        jumpSequence.SetUpdate(unscaledTime);
        jumpSequence.Append(perfectText.transform.DOScale(Vector3.one * jumpScale, jumpTime));
        jumpSequence.Append(perfectText.transform.DOScale(Vector3.one, jumpTime));
        jumpSequence.Append(perfectText.DOFade(0f, showTime / 2f).SetDelay(showTime));
        jumpSequence.OnComplete(() =>
        {
            callback?.Invoke();
            perfectText.SetInactive();
        });
    }

    private void RotateAnimation()
    {
        rotateSequence?.Kill();
        rotateSequence = DOTween.Sequence();
        rotateSequence.SetUpdate(unscaledTime);
        rotateSequence.Append(perfectText.transform.DORotate(Vector3.forward * GetRotateAngle(leftZRotate), rotateTime));
        rotateSequence.Append(perfectText.transform.DORotate(Vector3.forward * GetRotateAngle(rightZRotate), rotateTime));
        rotateSequence.Append(perfectText.transform.DORotate(Vector3.zero, rotateTime));
    }

    private float GetRotateAngle(Vector2 angleRange)
    {
        return Random.Range(angleRange.x, angleRange.y);
    }

}
