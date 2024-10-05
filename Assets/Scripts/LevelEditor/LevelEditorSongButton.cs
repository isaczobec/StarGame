using System;
using UnityEngine;

public class LevelEditorSongButton : MonoBehaviour
{
    private SongSO songSO;
    [SerializeField] private UIButton playButton;
    [SerializeField] private UIButton selectButton;

    [SerializeField] private TMPro.TextMeshProUGUI songNameText;
    [SerializeField] private TMPro.TextMeshProUGUI songArtistText;



    public void Setup(SongSO songSO)
    {
        this.songSO = songSO;
        playButton.OnUIButtonReleased += OnPlayButtonReleased;
        selectButton.OnUIButtonReleased += OnSelectButtonReleased;
        songNameText.text = songSO.songName;
        songArtistText.text = songSO.artist;
    }

    private void OnSelectButtonReleased(object sender, EventArgs e)
    {
        LevelEditorSongSelector.instance.SelectSong(songSO);
    }

    private void OnPlayButtonReleased(object sender, EventArgs e)
    {
        MusicManager.insance.PlaySong(songSO);
    }
}