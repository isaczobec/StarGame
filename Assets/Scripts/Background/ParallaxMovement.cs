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

    private void Update() {
        lastCameraPosition = targetCamera.position;
    }

    /// <summary>
    /// Get the movement of the target camera since the last frame
    /// </summary>
    public Vector3 GetParallaxMovement(
        bool negative = true
    ) {
        if (negative) return -1 * (targetCamera.position - lastCameraPosition); else return lastCameraPosition - targetCamera.position;
    }

}
