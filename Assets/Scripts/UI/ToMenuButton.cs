
using System;
using UnityEngine;
using UnityEngine.UI;

public class ToMenuButton : MonoBehaviour
{
    [SerializeField] private Button button;

    private void Start()
    {
        button.onClick.AddListener(ToMenu);
    }

    private void ToMenu()
    {
        GameController.Instance.LoadMenu();
    }
}
