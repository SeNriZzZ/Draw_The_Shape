
using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private float smallDelay = 7f;
    [SerializeField] private float bigDelay = 14f;
    
    public event Action OnSmallDelay;
    public event Action OnBigDelay;
    public event Action OnAction;

    private float timePassed;

    private bool smallDelayTriggered;
    private bool bigDelayTriggered;
    
    private void Update()
    {
        timePassed += Time.deltaTime;
        OnActionDone();
        CheckForDelays();
    }

    private void CheckForDelays()
    {
        if (timePassed >= smallDelay && timePassed <= bigDelay)
        {
            if(smallDelayTriggered) return;
            OnSmallDelay?.Invoke();
            smallDelayTriggered = true;
        }
        else if (timePassed >= bigDelay)
        {
            if(bigDelayTriggered) return;
            OnBigDelay?.Invoke();
            bigDelayTriggered = true;
        }
    }

    private void OnActionDone()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnAction?.Invoke();
            timePassed = 0f;
            smallDelayTriggered = false;
            bigDelayTriggered = false;
        }
    }
}
