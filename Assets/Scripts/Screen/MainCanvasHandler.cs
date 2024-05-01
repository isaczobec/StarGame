using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvasHandler : MonoBehaviour
{

    private Canvas canvas;
    private void Awake() {
        canvas = GetComponent<Canvas>();

        canvas.enabled = true;
    }

}
