using UnityEngine;

[CreateAssetMenu(fileName = "New Song", menuName = "Song")]
public class SongSO : ScriptableObject {

    public string songName;
    public string artist;
    public AudioClip clip;

}