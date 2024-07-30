using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour {

    public static MusicManager insance { get; private set; }

    [Header("References")]
    [SerializeField] private SoundPlayer soundPlayer;
    [SerializeField] private string audioName = "testMusic";


    // mixer settings
    private AudioMixerGroup musicGroup;


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
        soundPlayer.PlayLoop(audioName);
    }


    



}