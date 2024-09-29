using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEditorReturn : MonoBehaviour
{

    private const string MAIN_MENU_SCENE_REF = "_InitScene";
    [SerializeField] private UIButtonAnimated saveAndExitButton;

    private void Start()
    {
        saveAndExitButton.OnUIButtonReleased += OnSaveAndExitButtonClicked;
    }

    private void OnSaveAndExitButtonClicked(object sender, EventArgs e)
    {
        SaveAndExitToMainMenu();
    }

    public void SaveAndExitToMainMenu() {
        LevelEditorDataManager.instance.SaveData();
        SceneManager.LoadScene(MAIN_MENU_SCENE_REF);
    }
}