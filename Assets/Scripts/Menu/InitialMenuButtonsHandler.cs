using System;
using UnityEngine;

/// <summary>
/// Class that handles the initial play and level editor buttons.
/// </summary>
public class InitialMenuButtonsHandler : MonoBehaviour {
    
    [SerializeField] private UIButtonAnimated playButton;
    [SerializeField] private UIButtonAnimated levelEditorButton;
    
    public static InitialMenuButtonsHandler Instance { get; private set; }

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("InitialMenuButtonsHandler already exists in the scene. Deleting this one.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ShowInitialButtons() {
        playButton.ChangeVisible(true);
        levelEditorButton.ChangeVisible(true);
    }

    public void HideInitialButtons() {
        playButton.ChangeVisible(false);
        levelEditorButton.ChangeVisible(false);
    }

    private void Start() {
        // Subscribe to button click events
        playButton.OnUIButtonClicked += OnPlayButtonClicked;
        levelEditorButton.OnUIButtonClicked += OnLevelEditorButtonClicked;

        ShowInitialButtons();
    }

    private void OnLevelEditorButtonClicked(object sender, EventArgs e)
    {
    }

    private void OnPlayButtonClicked(object sender, EventArgs e)
    {
        // hide buttons and show level buttons
        HideInitialButtons();
        LevelButtonHandler.Instance.DisplayLevelButtons(); 
    }
}