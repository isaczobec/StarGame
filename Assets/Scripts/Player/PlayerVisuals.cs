using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{

    [SerializeField] private float RotationSpeed = 5f;

    [SerializeField] private Material playerMaterial;
    [SerializeField] private string playerColorName = "_PlayerColor";
    


    private Coroutine fadeToPlayerColorCoroutine;
    private bool isFadingToPlayerColor = false;
    
    public static PlayerVisuals Instance { get; private set; }
    private void Awake() {
        // Singleton pattern
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }
    }


    // Update is called once per frame
    void Update()
    {
        Rotate();
    }

    private void Rotate() {
        transform.Rotate(Vector3.forward * RotationSpeed * Time.deltaTime);
    }

    private IEnumerator FadePlayerColorCoroutine(Color newColor, float duration) {
        isFadingToPlayerColor = true;

        float passedTime = 0;

        while (passedTime < duration) {
            playerMaterial.SetColor(playerColorName, Color.Lerp(playerMaterial.GetColor(playerColorName), newColor, passedTime / duration));
            passedTime += Time.deltaTime;
            yield return null;
        }
        playerMaterial.SetColor(playerColorName, newColor);
        isFadingToPlayerColor = false;
    
        yield break;

    }

    public void FadePlayerColor(Color newColor, float duration) {
        newColor.a = 1;
        if (isFadingToPlayerColor) {
            StopCoroutine(fadeToPlayerColorCoroutine);
        }
        fadeToPlayerColorCoroutine = StartCoroutine(FadePlayerColorCoroutine(newColor, duration));
    }

}
