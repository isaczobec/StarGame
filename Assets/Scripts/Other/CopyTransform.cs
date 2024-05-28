using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyTransform : MonoBehaviour
{

    [SerializeField] private Transform sourceTransform;

    [Header("Settings to copy")]
    [SerializeField] private bool copyPosition = true;
    [SerializeField] private bool copyRotation = true;
    [SerializeField] private bool copyScale = true;

    // Update is called once per frame
    void Update()
    {
        UpdateTransform();
    }

    private void UpdateTransform() {
        if (copyPosition) {transform.position = sourceTransform.position;}
        if (copyRotation) {transform.rotation = sourceTransform.rotation;}
        if (copyScale) {transform.localScale = sourceTransform.localScale;}
    }
}
