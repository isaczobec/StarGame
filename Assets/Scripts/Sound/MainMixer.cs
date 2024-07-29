using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MainMixer : MonoBehaviour
{

    public static MainMixer instance {get; private set;}

    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;
    public AudioMixerGroup GetMusicGroup()
    {
        return musicGroup;
    }
    public AudioMixerGroup GetSfxGroup()
    {
        return sfxGroup;
    }

    private Dictionary<MixerGroupType, AudioMixerGroup> mixerGroupDict;

    void Awake()
    {
        // set instance
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SetupMixerGroupDict();
    }

    public void SetupMixerGroupDict()
    {
        mixerGroupDict = new Dictionary<MixerGroupType, AudioMixerGroup>
        {
            { MixerGroupType.Music, musicGroup },
            { MixerGroupType.Sfx, sfxGroup }
        };
    }

    public AudioMixerGroup GetMixerGroup(MixerGroupType mixerGroupType)
    {
        return mixerGroupDict[mixerGroupType];
    }

}

public enum MixerGroupType
{
    Music,
    Sfx
}