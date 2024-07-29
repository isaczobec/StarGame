using UnityEngine;

public class MusicManager : MonoBehaviour {

    public static MusicManager insance { get; private set; }

    [SerializeField] private SoundPlayer soundPlayer;
    [SerializeField] private string audioName = "testMusic";

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
        soundPlayer.PlayLoop(audioName);
    }



}