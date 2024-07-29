using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BackgroundParallax : MonoBehaviour
{

    /// <summary>
    /// An array of visual effects that will be moved according to the parallax movement.
    /// the vector3 is momentary, it is the movement since the last frame.
    /// </summary>
    [SerializeField] private VisualEffect[] parallaxEffectsToMove;
    [SerializeField] private string parallaxMovementReference = "ParallaxMovement";

    /// <summary>
    /// An array of materials that will be moved according to the parallax movement.
    /// The vector3 isnt momentary, it is the total movement since the start of the game.
    /// </summary>
    [SerializeField] private Material[] parallaxMaterialsToMove;
    [SerializeField] private string cameraLocationReference = "_CameraLocation";
    [SerializeField] private ParallaxMovement parallaxMovement;

    // Update is called once per frame
    void Update()
    {
        UpdateMomentaryParrallaxVisualEffects();
        UpdateCameraLocationMaterials();
    }

    private void UpdateMomentaryParrallaxVisualEffects()
    {
        foreach (VisualEffect effect in parallaxEffectsToMove)
        {
            Vector3 parallax = parallaxMovement.GetParallaxMovement();
            effect.SetVector3(parallaxMovementReference, parallax);
        }
    }

    private void UpdateCameraLocationMaterials() {
        Vector3 cameraLocation = parallaxMovement.GetParallaxPosition();
        foreach (Material material in parallaxMaterialsToMove) {
            material.SetVector(cameraLocationReference, cameraLocation);
        }
    }
}
