using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRawImage : MonoBehaviour
{

    [SerializeField] private RectTransform rawImageParentRectTransform;
    private float aspectRatio;

    // cAllculate aspect ratio
    private void Awake()
    {
        aspectRatio = rawImageParentRectTransform.rect.width / rawImageParentRectTransform.rect.height;
    }

    // Set screen size
    private void SetScreenSize() {
        rawImageParentRectTransform.sizeDelta = new Vector2(Screen.height * aspectRatio, Screen.height);
    }

    private void Update() {
        SetScreenSize();
    }

}
