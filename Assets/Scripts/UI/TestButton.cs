using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{

    

    private Button button;

    [SerializeField] private string levelToLoad;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        LevelHandler.Insance.LoadLevel(levelToLoad);
    }
}
