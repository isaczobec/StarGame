using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxMovement : MonoBehaviour
{

    /// <summary>
    /// The camera that the parallax effect will be based on
    /// </summary>
    [SerializeField] private Transform targetCamera;

    private Vector3 lastCameraPosition;

    private Vector3 parallaxMovement;

    private void Update() {
        parallaxMovement = lastCameraPosition - targetCamera.position;
        lastCameraPosition = targetCamera.position;
    }

    /// <summary>
    /// Get the movement of the target camera since the last frame
    /// </summary>
    public Vector3 GetParallaxMovement(
        bool negative = true
    ) {
        if (negative) return -1 * parallaxMovement; else return parallaxMovement;
    }

}
