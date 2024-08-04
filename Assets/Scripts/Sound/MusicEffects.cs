using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class MusicEffects : MonoBehaviour {

    private AudioMixer mainMixer;
    
    [Header("Mixer references")]
    [SerializeField] private string musicReverbDryParameter;
    [SerializeField] private string musicReverbWetParameter;
    [SerializeField] private string musicLowPassFrequencyParameter;

    [Header("DeathMusicEffect")]
    [Header("Animationcurves")]
    [Header("Start and end at y = 1 or 0, same value.")]
    [Header("Start at x = 0 and end at x = 1")]
    [SerializeField] private AnimationCurve deathMusicReverbWetCurve;
    [SerializeField] private AnimationCurve deathMusicReverbDryCurve;
    [SerializeField] private AnimationCurve deathMusicLowPassCurve;	

    [SerializeField] private float defaultReverbWetLevel = -10000f;
    [SerializeField] private float defaultReverbDryLevel = 0f;
    [SerializeField] private float defaultLowPassLevel = 22000f;


    // coroutines
    private Coroutine deathMusicEffectCoroutine;

    private void Start()
    {
        mainMixer = MainMixer.instance.GetMainAudioMixer();
        Player.Instance.OnPlayerDeath += StartDeathMusicEffect;
    }

    private void StartDeathMusicEffect(object sender, PlayerDeathEventArgs e)
    {
        if (deathMusicEffectCoroutine != null)
        {
            StopCoroutine(deathMusicEffectCoroutine);
        }
        deathMusicEffectCoroutine = StartCoroutine(DeathMusicEffectCoroutine(e.respawnTime));
    }

    private IEnumerator DeathMusicEffectCoroutine(float duration) {
        float passedTime = 0f;
        float lowPassMultipleir = defaultLowPassLevel;
        float reverbMultiplier = -defaultReverbWetLevel;

        while (passedTime < duration) {
            float lowPassLevel = deathMusicLowPassCurve.Evaluate(passedTime / duration) * lowPassMultipleir;
            float reverbWetLevel = (deathMusicReverbWetCurve.Evaluate(passedTime / duration)-1) * reverbMultiplier;
            float reverbDryLevel = (deathMusicReverbDryCurve.Evaluate(passedTime / duration)-1) * reverbMultiplier;

            mainMixer.SetFloat(musicLowPassFrequencyParameter, lowPassLevel);
            mainMixer.SetFloat(musicReverbWetParameter, reverbWetLevel);
            mainMixer.SetFloat(musicReverbDryParameter, reverbDryLevel);

            passedTime += Time.deltaTime;
            yield return null;
        }
        deathMusicEffectCoroutine = null;
    }
}