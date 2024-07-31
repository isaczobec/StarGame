using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A script that should be attached to the parent of all world objects in the level. Can handle ie destruction of all objects in the level
/// </summary>
public class LevelWorldObjectsHandler : MonoBehaviour
{
    private void Start() {
        LevelHandler.Insance.AddLevelWorldObjectsHandler(this);
    }
}
