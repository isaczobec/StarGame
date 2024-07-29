using System;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{

    [SerializeField] private PlayableSound[] sounds;

    public const string defaultSoundName = "_default";

    // Start is called before the first frame update
    void Start()
    {   
        SetupPlayableSounds();
    }

    private void SetupPlayableSounds() {
        // initialize all the sounds
        foreach (PlayableSound sound in sounds) {
            sound.Setup(gameObject.AddComponent<AudioSource>());
        }
    }

    private PlayableSound GetPlayableSound(string soundName) {
        if (soundName == defaultSoundName) {
            return sounds[0];
        }
        foreach (PlayableSound sound in sounds) {
            if (sound.name == soundName) {
                return sound;
            }
        }
        return null;
    }

    public void PlayOneShot(string soundName = defaultSoundName,float pitch = 1f, float volume = 1f, float randomPitchRange = 0f, float randomVolumeRange = 0f) {
        PlayableSound sound = GetPlayableSound(soundName);
        if (sound != null) {
            sound.PlayOneShot(pitch, volume, randomPitchRange, randomVolumeRange);
        }
    }
    public void PlayLoop(string soundName = defaultSoundName,float pitch = 1f, float volume = 1f, float randomPitchRange = 0f, float randomVolumeRange = 0f) {
        PlayableSound sound = GetPlayableSound(soundName);
        if (sound != null) {
            sound.PlayLoop(pitch, volume, randomPitchRange, randomVolumeRange);
        }
    }

}


[Serializable]
public class PlayableSound {

    /// <summary>
    /// The name that is used to find and play the sound.
    /// </summary>
    public string name;
    public AudioClip audioClip;
    public MixerGroupType mixerGroupType;
    public float masterVolume = 1f;
    public float spatialBlend = 0f;
    public float doplerLevel = 0f;




    

    private AudioSource audioSource;

    public void Setup (AudioSource audioSource) {
        this.audioSource = audioSource;
        audioSource.clip = audioClip;
        audioSource.spatialBlend = spatialBlend;
        audioSource.dopplerLevel = doplerLevel;

        audioSource.outputAudioMixerGroup = MainMixer.instance.GetMixerGroup(mixerGroupType);
    }

    public void PlayOneShot(float pitch = 1f, float volume = 1f, float randomPitchRange = 0f, float randomVolumeRange = 0f) {
        audioSource.pitch = randomPitchRange != 0f? UnityEngine.Random.Range(-randomPitchRange,randomPitchRange) + pitch : pitch;
        audioSource.volume = randomVolumeRange != 0f? UnityEngine.Random.Range(-randomVolumeRange,randomVolumeRange) + volume : volume;
        audioSource.volume *= masterVolume;
        audioSource.PlayOneShot(audioClip);
    }

    public void PlayLoop(float pitch = 1f, float volume = 1f, float randomPitchRange = 0f, float randomVolumeRange = 0f) {
        audioSource.loop = true;
        audioSource.pitch = randomPitchRange != 0f? UnityEngine.Random.Range(-randomPitchRange,randomPitchRange) + pitch : pitch;
        audioSource.volume = randomVolumeRange != 0f? UnityEngine.Random.Range(-randomVolumeRange,randomVolumeRange) + volume : volume;
        audioSource.volume *= masterVolume;
        audioSource.volume = volume;
        audioSource.Play();
    }

    public void StopLoop() {
        audioSource.loop = false;
        audioSource.Stop();
    }

    

}
