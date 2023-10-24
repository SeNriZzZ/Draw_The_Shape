using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSceneController : MonoBehaviour
{
    [SerializeField] private Transform menuUI;

    private void Start()
    {
        Instantiate(menuUI);
    }
}
