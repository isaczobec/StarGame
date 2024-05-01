using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{

    [SerializeField] private float RotationSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }

    private void Rotate() {
        transform.Rotate(Vector3.forward * RotationSpeed * Time.deltaTime);
    }

}
