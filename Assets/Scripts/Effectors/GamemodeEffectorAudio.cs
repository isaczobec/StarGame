using System;
using UnityEngine;

public class GamemodeEffectorAudio : MonoBehaviour
{
    [SerializeField] private SoundPlayer gamemodeSoundPlayer;
    [SerializeField] private GamemodeEffector gamemodeEffector;


    [Header("Audio Settings")]
    [SerializeField] private string audioName = "PortalEntered";
    [SerializeField] private float pitch = 1f;
    [SerializeField] private float volume = 1f;
    [SerializeField] private float randomPitchRange = 0.2f;
    [SerializeField] private float randomVolumeRange = 0f;


    private void Start()
    {
        gamemodeEffector.OnEffectorTriggeredByPlayerEvent += GamemodeEffector_OnEffectorTriggered;
    }

    private void GamemodeEffector_OnEffectorTriggered(object sender, EventArgs e)
    {
        gamemodeSoundPlayer.PlayOneShot(audioName, pitch, volume, randomPitchRange, randomVolumeRange);
    }
}