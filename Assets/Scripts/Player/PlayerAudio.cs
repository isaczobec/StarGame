using System;
using UnityEngine;

public class PlayerAudio : MonoBehaviour {
    [SerializeField] private SoundPlayer playerSoundPlayer;
    [SerializeField] private Player player;

    [Header("Audio Settings")]
    [SerializeField] private string deathAudioName = "death";
    [SerializeField] private string audioName = "MomentaryDirectionChanged";
    [SerializeField] private float pitch = 1f;
    [SerializeField] private float volume = 1f;
    [SerializeField] private float randomPitchRange = 0.2f;
    [SerializeField] private float randomVolumeRange = 0f;

    private void Start() {
        player.OnPlayerMomentaryDirectionChanged += Player_OnGameModeStateChanged;
        player.OnPlayerDeath += Player_OnPlayerDeath;
    }


    private void Player_OnGameModeStateChanged(object sender, Vector2 e)
    {
        playerSoundPlayer.PlayOneShot(audioName, pitch, volume, randomPitchRange, randomVolumeRange);
    }

    private void Player_OnPlayerDeath(object sender, PlayerDeathEventArgs e)
    {
        playerSoundPlayer.PlayOneShot(deathAudioName);
    }

}