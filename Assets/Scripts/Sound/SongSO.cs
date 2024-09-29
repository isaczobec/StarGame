using UnityEngine;

[CreateAssetMenu(fileName = "New Song", menuName = "Song")]
public class SongSO : ScriptableObject {

    public string songID;
    public string songName;
    public string artist;
    public AudioClip clip;

}