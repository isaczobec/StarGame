using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedArrowAudio : MonoBehaviour
{
    [SerializeField] private SpeedArrowEffector speedArrowEffector; // The effector that this visual is linked to

    [SerializeField] private string speedUpRefString = "SpeedUp"; // The reference string for the speed up sound
    [SerializeField] private string speedDownRefString = "SpeedDown"; // The reference string for the speed down sound
    [SerializeField] private SoundPlayer soundPlayer; // The sound player that will play the sounds


    private void Start()
    {
        speedArrowEffector.OnSpeedArrowTriggered += SpeedArrowEffector_OnEffectorTriggered; // subscribe to the effector event
    }

    private void SpeedArrowEffector_OnEffectorTriggered(object sender, OnSpeedArrowTriggeredEventArgs e)
    {
        if (e.spedUp) {
            soundPlayer.PlayOneShot(speedUpRefString);
        } else {
            soundPlayer.PlayOneShot(speedDownRefString);
        }
    }
}
