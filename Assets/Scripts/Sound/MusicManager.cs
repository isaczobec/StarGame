using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour {

    public static MusicManager insance { get; private set; }

    [Header("References")]
    [SerializeField] private string audioName = "testMusic";
    [SerializeField] private SongSO menuSong;

    [Header("Settings")]

    [SerializeField] private float baseFadeTime = 0.5f;
    [SerializeField] private bool autoPlayMenuMusic = true;

    // mixer settings
    private AudioMixerGroup musicGroup;

    private List<SongAndAudioSource> songAndAudioSources = new List<SongAndAudioSource>();

    [Header("Song Scriptable Objects")]
    /// <summary>
    /// All the song scriptable objects in the game.
    /// </summary>
    [SerializeField] private SongSO[] songSOs;


    private void Awake()
    {
        if (insance == null)
        {
            insance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        musicGroup = MainMixer.instance.GetMusicGroup();
        if (autoPlayMenuMusic) PlaySong(menuSong);
    }


    private SongAndAudioSource TryGetSongAndAudioSource(SongSO song) {
        foreach (var item in songAndAudioSources) {
            if (item.song == song) {
                return item;
            }
        }
        return null;
    }

    /// <summary>
    /// Plays a song if it is not already playing.
    /// </summary>
    /// <param name="song"></param>
    public void PlaySong(SongSO song, bool stopOthers = true) {
        SongAndAudioSource songAndAudioSource = TryGetSongAndAudioSource(song);

            if (stopOthers) {
                foreach (var item in songAndAudioSources) {
                    item.audioSource.Stop();
                }
            }

            if (songAndAudioSource == null) CreateSongAndAudioSource(song);
            songAndAudioSource = TryGetSongAndAudioSource(song);
            songAndAudioSource.audioSource.Play();

    }

    /// <summary>
    /// Plays a song from an ID if it is not already playing.
    /// </summary>
    /// <param name="song"></param>
    public void PlaySong(string songID,bool stopOthers = true) {
        SongSO song = GetSongSOFromID(songID);
        if (song != null) {
            PlaySong(song,stopOthers);
        }
    }

    public void StopSong(SongSO song, float fadeOutTime) {
        FadeToVolume(song, 0f, fadeOutTime, destroyAudioSourceObjectOnFadeOut: true);
    }

    public void StopAllMusic(float fadeOutTime) {
        foreach (var item in songAndAudioSources) {
            StopSong(item.song, fadeOutTime);
        }
    }

    public void FadeToVolume(SongSO song, float targetVolume, float fadeTime, bool destroyAudioSourceObjectOnFadeOut = false) {
        SongAndAudioSource songAndAudioSource = TryGetSongAndAudioSource(song);
        if (songAndAudioSource == null) {
            return;
        }
        if (songAndAudioSource.fadeCoroutine != null) {
            StopCoroutine(songAndAudioSource.fadeCoroutine);
        }
        songAndAudioSource.fadeCoroutine = StartCoroutine(FadeToVolumeCoroutine(song, targetVolume, fadeTime, destroyAudioSourceObjectOnFadeOut));
    }

    public void PlayMenuMusic() {
        PlaySong(menuSong);
    }

    private IEnumerator FadeToVolumeCoroutine(SongSO song, float targetVolume, float fadeTime, bool destroyAudioSourceObjectOnFadeOut = false) {
        SongAndAudioSource songAndAudioSource = TryGetSongAndAudioSource(song);
        if (songAndAudioSource == null) {
            yield break;
        }
        float startVolume = songAndAudioSource.audioSource.volume;
        float startTime = Time.time;
        while (Time.time < startTime + fadeTime) {
            songAndAudioSource.audioSource.volume = Mathf.Lerp(startVolume, targetVolume, (Time.time - startTime) / fadeTime);
            yield return null;
        }
        songAndAudioSource.audioSource.volume = targetVolume;
        songAndAudioSource.fadeCoroutine = null;

        if (destroyAudioSourceObjectOnFadeOut && targetVolume <= 0f)
        {
            DestroySongAndAudioSource(songAndAudioSource);
        }
    }


    private void CreateSongAndAudioSource(SongSO song)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = song.clip;
        audioSource.outputAudioMixerGroup = musicGroup;
        audioSource.loop = true;
        SongAndAudioSource songAndAudioSource = new SongAndAudioSource
        {
            song = song,
            audioSource = audioSource
        };
        songAndAudioSources.Add(songAndAudioSource);
    }

    private void DestroySongAndAudioSource(SongAndAudioSource songAndAudioSource)
    {
        Destroy(songAndAudioSource.audioSource);
        songAndAudioSources.Remove(songAndAudioSource);
    }

    /// <summary>
    /// Get the SongSO from the songID. Returns null if not found.
    /// </summary>
    /// <param name="songID"></param>
    /// <returns></returns>
    private SongSO GetSongSOFromID(string songID) {
        for (int i = 0; i < songSOs.Length; i++) {
            if (songSOs[i].songID == songID) {
                return songSOs[i];
            }
        }
        return null;
    }


}


public class SongAndAudioSource {
    public SongSO song;
    public AudioSource audioSource;

    public Coroutine fadeCoroutine;

}