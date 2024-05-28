using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BackgroundParallax : MonoBehaviour
{

    [SerializeField] private VisualEffect[] parallaxEffectsToMove;
    [SerializeField] private string parallaxMovementReference = "ParallaxMovement";
    [SerializeField] private ParallaxMovement parallaxMovement;

    // Update is called once per frame
    void Update()
    {
        foreach (VisualEffect effect in parallaxEffectsToMove)
        {
            effect.SetVector3(parallaxMovementReference, parallaxMovement.GetParallaxMovement());
        }
    }
}
