using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("references")]
    [SerializeField] private Cinemachine.CinemachineVirtualCamera virtualCamera;
    [Header("Settings")]
    [SerializeField] private float defaultShakeFrequency = 6f; 
    [Header("Death shake Settings")]
    [SerializeField] private float deathShakeFrequency = 6f; 
    [SerializeField] private float deathShakeTime = 6f; 
    [SerializeField] private float deathShakeIntensity = 6f; 
    private Coroutine shakeCoroutine;

    public static CameraShake Instance {get; private set;}

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetShakeFrequency(defaultShakeFrequency);
        Player.Instance.OnPlayerDeath += Player_OnPlayerDeath;
    }




    // ----- METHODS FOR SHAKING THE SCREEN ----- 

    public void StartCameraShake(float intensity, float time, float frequency = 0)
    {
        Debug.Log("Shake");
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }

        shakeCoroutine = StartCoroutine(Shake(intensity, time, frequency));
    }

    private IEnumerator Shake(float intensity, float time, float frequency = 0)
    {
        Cinemachine.CinemachineBasicMultiChannelPerlin noise = virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();

        float passedTime = 0f;

        noise.m_AmplitudeGain = intensity;
        if (frequency == 0) {noise.m_FrequencyGain = defaultShakeFrequency;} else {noise.m_FrequencyGain = frequency;}

        while (passedTime < time)
        {
            noise.m_AmplitudeGain = Mathf.Lerp(intensity, 0f, passedTime/time);
            passedTime += Time.deltaTime;
            yield return null;
        }

        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;

        shakeCoroutine = null;
    }

    private void SetShakeFrequency(float frequency)
    {
        Cinemachine.CinemachineBasicMultiChannelPerlin noise = virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        noise.m_FrequencyGain = frequency;
    }

    // ----- SHAKE THE SCREEN ON CERTAIN EVENTS -----
    private void Player_OnPlayerDeath(object sender, PlayerDeathEventArgs e)
    {
        StartCameraShake(deathShakeIntensity, deathShakeTime, deathShakeFrequency);
    }

}
