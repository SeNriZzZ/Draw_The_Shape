using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InputManager : MonoBehaviour
{
    private Shape shape;
    private Path path;
    private Vector3 clickPosition;
    private Vector2 direction;
    private float angle, angleOffset, fillAmount;
    private float clockWiseSign;
    private Image pathFillImage;
    private RaycastHit2D hit2d;

    private Camera camera;
    private Rect rect;

    private const string START_COLLIDER_TAG = "Start";
    private const float RECT_HIGHT_DIVIDER = 2f;
    private const float CIRCLE_ANGLE_MAX = 360f;
    

    private void Start()
    {
        camera = Camera.main;
    }

    public void SetShape(Shape newShape)
    {
        shape = newShape;
    }

    public void OnExitImage()
    {
        if(path == null) return;
        path.Clear();
        path = null;
    }

    private void Update ()
    {
        OnClick();
        OnRelease();
        FillPath();
    }
    
    private void OnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (shape == null)return;
            if (shape.isComplete) return;
            hit2d = Physics2D.Raycast (GetCurrentPlatformClickPosition (camera), Vector2.zero);
            if (hit2d.collider != null)
            {
                if (hit2d.transform.CompareTag(START_COLLIDER_TAG))
                {
                    var newPath = hit2d.transform.GetComponentInParent<Path>();
                    if (shape.CurrentPath == newPath)
                    {
                        path = newPath;
                        pathFillImage = path.GetFillImage();
                        rect = path.GetRect();
                    }
                }
            }
        }
    }

    private void OnRelease()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if(path == null) return;
            path.Clear();
            path = null;
        }
    }

    private void FillPath()
    {
        if (path == null) return;
        if (path.FillMethod == FillMethod.Radial) 
        {
            RadialFill ();
        } 
        else if (path.FillMethod == FillMethod.Linear)
        {
            LinearFill();
        }
    }
    
    private Vector3 GetCurrentPlatformClickPosition (Camera camera)
    {
        var clickPos = Vector3.zero;

        if (Application.isMobilePlatform) 
        {
            if (Input.touchCount != 0) 
            {
                var touch = Input.GetTouch (0);
                clickPos = touch.position;
            }
        } 
        else clickPos = Input.mousePosition;
        
        clickPos = camera.ScreenToWorldPoint (clickPos);
        clickPos.z = 0;
        return clickPos;
    }
    
    
    
    private void RadialFill ()
    {
        clickPosition = camera.ScreenToWorldPoint (Input.mousePosition);
    
        direction = clickPosition - path.transform.position;
    
        angleOffset = 0;
        clockWiseSign = (pathFillImage.fillClockwise ? 1 : -1);
    
        switch (pathFillImage.fillOrigin)
        {
            case 0:
                angleOffset = 0;
                break;
            case 1:
                angleOffset = clockWiseSign * 90;
                break;
            case 2:
                angleOffset = -180;
                break;
            case 3:
                angleOffset = -clockWiseSign * 90;
                break;
        }
    
        angle = Mathf.Atan2 (-clockWiseSign * direction.x, -direction.y) * Mathf.Rad2Deg + angleOffset;
    
        if (angle < 0)
            angle += CIRCLE_ANGLE_MAX;
    
        angle = Mathf.Clamp (angle, 0, CIRCLE_ANGLE_MAX);
        angle -= path.RadialAngleOffset;
    
        fillAmount = Mathf.Abs (angle / CIRCLE_ANGLE_MAX);
        pathFillImage.fillAmount = fillAmount;
        CheckPathComplete();
    }
    
    private void LinearFill ()
    {
        clickPosition = camera.ScreenToWorldPoint(Input.mousePosition);

        var rotation = path.transform.eulerAngles;
        rotation.z -= path.Offset;
        
        Vector3 pos1 = Vector3.zero, pos2 = Vector3.zero;
        
        pos1.x = path.transform.position.x - Mathf.Cos (rotation.z * Mathf.Deg2Rad) * rect.height / RECT_HIGHT_DIVIDER;
        pos1.y = path.transform.position.y - Mathf.Sin (rotation.z * Mathf.Deg2Rad) * rect.height / RECT_HIGHT_DIVIDER;

        pos2.x = path.transform.position.x + Mathf.Cos (rotation.z * Mathf.Deg2Rad) * rect.height / RECT_HIGHT_DIVIDER;
        pos2.y = path.transform.position.y + Mathf.Sin (rotation.z * Mathf.Deg2Rad) * rect.height / RECT_HIGHT_DIVIDER;
        
        pos1.z = path.transform.position.z;
        pos2.z = path.transform.position.z;

        if (path.Flip) (pos2, pos1) = (pos1, pos2);
        
        clickPosition.x = Mathf.Clamp (clickPosition.x, Mathf.Min (pos1.x, pos2.x), Mathf.Max (pos1.x, pos2.x));
        clickPosition.y = Mathf.Clamp (clickPosition.y, Mathf.Min (pos1.y, pos2.y), Mathf.Max (pos1.y, pos2.y));
        fillAmount = Vector2.Distance (clickPosition, pos1) / Vector2.Distance (pos1, pos2);
        pathFillImage.fillAmount = fillAmount;
        CheckPathComplete ();
    }
    
    private void CheckPathComplete()
    {
        if (pathFillImage.fillAmount < path.CompleteOffset) return;
        path.Complete();
        path = null;
        shape.CompletePath();
        
    }
}


