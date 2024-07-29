using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    
    [Header("Animation curves")]
    [SerializeField] private AnimationCurve sizeAnimationCurve;

    [Header("Materials")]
    [SerializeField] private Material shockWaveSourceMaterial;


    /// <summary>
    /// The minimum size of the shock wave it will start at.
    /// </summary>
    [SerializeField] private float minSize;

    /// <summary>
    /// The maximum size of the shock wave it will reach at the end of its lifetime.
    /// </summary>
    [SerializeField] private float maxSize;

    private float size;

    /// <summary>
    /// The max lifetime of the shock wave.
    /// </summary>
    [SerializeField] private float lifeTime;
    private float passedTime;
    
    private Material shockWaveMaterial;

    private bool isOn = false;

    private const string Size = "_Size";

    private void Start() {
        // instantiate material and set its initial values

        shockWaveMaterial = new Material(shockWaveSourceMaterial);
        GetComponent<SpriteRenderer>().material = shockWaveMaterial;
        shockWaveMaterial.SetFloat(Size, 0);

        
    }

    private void Update() {
        if (isOn) UpdateShockWave();
    }


    /// <summary>
    /// Starts the shock wave with the given lifetime and max size.
    /// </summary>
    /// <param name="lifeTime"></param>
    /// <param name="maxSize"></param>
    public void InitiateShockWave() {
        this.passedTime = 0;
        size = 0;

        GetComponent<DeleteGameObject>().DeleteGameObjectAfterTime(lifeTime);

        isOn = true;

    }

    /// <summary>
    /// Ran every frame to update the shock wave, ie increase size etc.
    /// </summary>
    private void UpdateShockWave()
    {
        passedTime += Time.deltaTime;
        size = Mathf.Lerp(minSize,maxSize,sizeAnimationCurve.Evaluate(passedTime / lifeTime));
        shockWaveMaterial.SetFloat(Size, size);
        
        if (passedTime >= lifeTime) {
            isOn = false;
            Destroy(gameObject);
        }
    }
}
