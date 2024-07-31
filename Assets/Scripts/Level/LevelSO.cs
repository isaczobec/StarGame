using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LevelSO", menuName = "Level/LevelSO", order = 1)]
public class LevelSO : ScriptableObject
{

    [Header("Level Info")]
    public string levelName;
    public string levelDifficulty;
    public string author;
    public int secondsPlayed;
    public int timesDied;

    public bool completed;

    [Header("Level Scene")]
    public string sceneToLoadRefString;
}
