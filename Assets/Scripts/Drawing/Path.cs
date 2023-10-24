using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public enum FillMethod
{
    Radial,
    Linear
}


public class Path : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image fillImage;
    [SerializeField] private FillMethod fillMethod;
    [SerializeField] private float completeOffset = 0.85f;
    [SerializeField] private float radialAngleOffset = 0;
    [SerializeField] private float offset = 0;
    [SerializeField] private bool flip;

    [SerializeField] private List<RoutePart> routeParts = new List<RoutePart>();
    [SerializeField] private Transform[] movePointsForFinger;
    [SerializeField] private float fingerMoveTime;
    [SerializeField] private Transform finger;

    private bool isComplete = false;

    public FillMethod FillMethod => fillMethod;
    public float CompleteOffset => completeOffset;
    public float RadialAngleOffset => radialAngleOffset;
    public float Offset => offset;
    public bool Flip => flip;


    public Image GetFillImage()
    {
        return fillImage;
    }

    public Rect GetRect()
    {
        return RectTransformToScreenSpace(rectTransform);
    }

    public void Complete()
    {
        fillImage.fillAmount = 1f;
        isComplete = true;
    }

    public void Clear()
    {
        if (!isComplete)
            fillImage.fillAmount = 0f;
    }

    public void ResetPath()
    {
        isComplete = false;
        fillImage.fillAmount = 0f;
    }
    
    private Rect RectTransformToScreenSpace (RectTransform transform)
    {
        var size = Vector2.Scale (transform.rect.size, transform.lossyScale);
        return new Rect (transform.position.x, Screen.height - transform.position.y, size.x, size.y);
    }

    public void ShowRoute()
    {
        StartCoroutine(RouteRoutine());
    }

    private IEnumerator RouteRoutine()
    {
        foreach (var routePart in routeParts)
        {
            routePart.Show();
            yield return new WaitForSeconds(.15f);
        }
    }

    public void HideRoute()
    {
        StopCoroutine(RouteRoutine());
        foreach (var routePart in routeParts)
        {
            routePart.Hide();
        }
    }

    public void MoveFinger()
    {
        finger.SetActive();
        finger.position = movePointsForFinger[0].position;
        finger.DOScale(Vector3.one, .3f);
        if (fillMethod == FillMethod.Linear)
        {
            finger.DOMove(movePointsForFinger[1].position, fingerMoveTime).SetLoops(-1, LoopType.Restart);
        }
        else
        {
            var path = movePointsForFinger.Select(x=>x.position).ToList();
            path.RemoveAt(0);
            var pathArr = path.ToArray();
            finger.DOPath(pathArr, fingerMoveTime).SetLoops(-1, LoopType.Restart);
        }
    }

    public void HideFinger()
    {
        finger.DOKill();
        finger.DOScale(Vector3.zero, .2f);
    }
}

