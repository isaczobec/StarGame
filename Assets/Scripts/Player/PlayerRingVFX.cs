using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerRingVFX : MonoBehaviour
{

    [SerializeField] private GameObject ringVFXObject;

    [SerializeField] private string ringColorReference = "Color";




    public void SpawnRingVFX(Color color) {
        GameObject ring = Instantiate(ringVFXObject, transform.position, Quaternion.identity, null);
        VisualEffect ringVFX = ring.GetComponent<VisualEffect>();
        ringVFX.SetVector4(ringColorReference, color);
    }

}
