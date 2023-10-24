using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using TMPro;


    public class FaderBlackScreenAnimation : BaseSceneFaderAnimation
    {
        [Header("Settings")]
        [SerializeField] private float fadeTime = 0.5f;
        [SerializeField] private Ease fadeEase;

        [Header("Objects")]
        [SerializeField] private Image fadeImage;

        [Header("Other")]
        [SerializeField] private CanvasGroup loadingTextCanvas;
        [SerializeField] private TextMeshProUGUI loadingText;
        [SerializeField] private string loadingTextStr = "Loading";
        [SerializeField] private float dotsSpeed = 1;

        public override void Show(Action callback)
        {
            loadingTextCanvas.alpha = 1;
            fadeImage.DOFade(1f, fadeTime).SetEase(fadeEase).OnComplete(() => callback?.Invoke()).OnUpdate(LoadingText);
        }
        public override void Hide(Action callback)
        {
            loadingTextCanvas.DOFade(0, fadeTime);
            fadeImage.DOFade(0f, fadeTime).SetEase(fadeEase).OnComplete(() => callback?.Invoke());
        }
        public override void ShowImmediately()
        {
            gameObject.SetActive();
            fadeImage.color = fadeImage.color.WithAlpha(1);
            loadingTextCanvas.alpha = 1;
        }
        public override void HideImmediately()
        {
            fadeImage.color = fadeImage.color.WithAlpha(0);
            loadingTextCanvas.alpha = 0;
        }

        private void LoadingText()
        {
            loadingText.text = loadingTextStr;
            for (int i = 0; i < Mathf.Floor((Time.time * dotsSpeed) % 4); i++)
                loadingText.text += ".";
        }
    }

