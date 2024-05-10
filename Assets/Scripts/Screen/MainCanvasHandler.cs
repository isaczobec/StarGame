using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvasHandler : MonoBehaviour
{
    

    [SerializeField] private GameObject mainCanvasObject;
    private void Awake() {
        mainCanvasObject.SetActive(true);
    }

}