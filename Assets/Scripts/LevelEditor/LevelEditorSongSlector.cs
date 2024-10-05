using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorSongSelector : MonoBehaviour {

    public static LevelEditorSongSelector instance { get; private set; }

    [SerializeField] private UIButtonAnimated songSelectMenuButton;
    [SerializeField] private UIButtonAnimated hideSongSelectMenuButton;

    /// <summary>
    /// All the songs that are available to be selected.
    /// </summary>
    [SerializeField] private SongSO[] songs;
    /// <summary>
    /// All the buttons that are currerntly instantiated.
    /// </summary>
    [SerializeField] private List<LevelEditorSongButton> levelEditorSongButtons;

    [SerializeField] private int maxRows = 20;
    [SerializeField] private int maxColumns = 20;
    [SerializeField] private float buttonSpacingHorizontal = 200;
    [SerializeField] private float buttonSpacingVertical = 100;
    [SerializeField] private Vector2 buttonOffset = new Vector3(0, 0);

    [SerializeField] private GameObject songSelectButtonPefab;
    [SerializeField] private GameObject songButtonParent;
    
    private void Awake() {
        instance = this;
    }

    private void Start() {
        songSelectMenuButton.OnUIButtonReleased += OnSongSelectMenuButtonReleased;
        hideSongSelectMenuButton.OnUIButtonReleased += OnHideSongSelectMenuButtonReleased;
    }

    private void OnHideSongSelectMenuButtonReleased(object sender, EventArgs e)
    {
        HideSongSelectMenu();
        hideSongSelectMenuButton.ChangeVisible(false);
    }

    private void OnSongSelectMenuButtonReleased(object sender, EventArgs e)
    {
        ShowSongSelectMenu();
        hideSongSelectMenuButton.ChangeVisible(true);
    }

    private void ShowSongSelectMenu()
    {
        EditorBuildingManager.instance.SetExternalBlockingPlacing(true);
        int currentColum = 0;
        int currentRow = 0;
        int horizontalSize = Mathf.Min(maxColumns, songs.Length);
        int verticalSize = Mathf.Min(maxRows, songs.Length/maxColumns + 1);

        Vector2 rootOffset = new Vector2(-horizontalSize * buttonSpacingHorizontal / 2f, verticalSize * buttonSpacingVertical / 2f) + buttonOffset;
        for (int i = 0; i < songs.Length; i++)
        {
            SongSO songSO = songs[i];
            Vector2 offset = new Vector2(rootOffset.x + currentColum * buttonSpacingHorizontal, rootOffset.y + currentRow * buttonSpacingVertical);
            CreateSongSelectButton(songSO, offset);
            currentColum++;
            if (currentColum >= maxColumns)
            {
                currentColum = 0;
                currentRow++;
            }
            if (currentRow >= maxRows)
            {
                break;
            }
        }

    }

    private void CreateSongSelectButton(SongSO songSO, Vector2 offset) {
        GameObject songSelectButton = Instantiate(songSelectButtonPefab, songButtonParent.transform);
        songSelectButton.transform.localPosition = new Vector3(offset.x, offset.y, 0);
        LevelEditorSongButton levelEditorSongButton = songSelectButton.GetComponent<LevelEditorSongButton>();
        levelEditorSongButtons.Add(levelEditorSongButton);
        levelEditorSongButton.Setup(songSO);
    }

    private void HideSongSelectMenu()
    {
        hideSongSelectMenuButton.ChangeVisible(false);
        EditorBuildingManager.instance.SetExternalBlockingPlacing(false);
        foreach (LevelEditorSongButton levelEditorSongButton in levelEditorSongButtons)
        {
            Destroy(levelEditorSongButton.gameObject);
        }
        levelEditorSongButtons.Clear();
        MusicManager.insance.StopAllMusic(0.5f);
    }

    public void SelectSong(SongSO songSO) {
        LevelEditorDataManager.instance.editorLevelData.songID = songSO.songID;
        HideSongSelectMenu();
    }
}