using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Base class for all effectors
/// </summary>
public class Effector : MonoBehaviour
{

    


    /// <summary>
    /// Called when the effector is hit. Can be overrided
    /// </summary>
    /// <param name="hitboxEntity">The hitboxentity that was hit</param>
    public virtual void OnEffectorTriggered(IHitboxEntity hitboxEntity)
    {
        Debug.Log("Effector hit");
    }

}
