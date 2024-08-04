using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenCoverer : MonoBehaviour
{

    /// <summary>
    /// Must have a property called _CoverAmount
    /// </summary>
    private Material covererMaterial;

    [SerializeField] private Image coverImage;
    [SerializeField] private SoundPlayer covererSoundPlayer;
    [SerializeField] private string onCoverSoundName = "cover";
    [SerializeField] private string onUncoverSoundName = "uncover";


    /// <summary>
    /// [0,1], 0 is no cover, 1 is full cover
    /// </summary>
    private float coverAmount = 0;
    private const string coverAmountRef = "_CoverAmount";


    private Coroutine coverCoroutine;


    public static ScreenCoverer instance { get; private set; }

    /// <summary>
    /// Called when the cover is complete. The bool is true if we covered the screen, false if we uncovered it
    /// </summary>
    public event EventHandler<bool> OnCoverComplete;

    void Awake()
    {
        instance = this;
        OnCoverComplete += UpdateImageActive;
    }

    public void Start() {
        coverImage.enabled = false;
        covererMaterial = coverImage.material;
        covererMaterial.SetFloat(coverAmountRef, coverAmount);
    }


    public void BeginCover(float timeToCover)
    {
        coverImage.enabled = true;
        StartCoverCoroutine(1, timeToCover);
        covererSoundPlayer?.PlayOneShot(onCoverSoundName);
    }


    public void EndCover(float timeToUncover)
    {
        StartCoverCoroutine(0, timeToUncover);
        covererSoundPlayer?.PlayOneShot(onUncoverSoundName);
    }

    private void UpdateCoverAmount(float newCoverAmount)
    {
        coverAmount = newCoverAmount;
        covererMaterial.SetFloat(coverAmountRef, coverAmount);
    }


    private void StartCoverCoroutine(float targetCoverAmount, float timeToCover)
    {
        if (coverCoroutine != null)
        {
            StopCoroutine(coverCoroutine);
        }
        coverCoroutine = StartCoroutine(CoverCoroutine(targetCoverAmount, timeToCover));
    }

    private IEnumerator CoverCoroutine(float targetCoverAmount,float timeToCover)
    {
        float startCoverAmount = coverAmount;
        float timeElapsed = 0;
        while (timeElapsed < timeToCover)
        {
            timeElapsed += Time.deltaTime;
            UpdateCoverAmount(Mathf.Lerp(startCoverAmount, targetCoverAmount, timeElapsed / timeToCover));
            yield return null;
        }
        UpdateCoverAmount(targetCoverAmount);
        coverCoroutine = null;
        OnCoverComplete?.Invoke(this, targetCoverAmount > startCoverAmount);

        coverCoroutine = null;
    }
    private void UpdateImageActive(object sender, bool covered)
    {
        if (!covered) coverImage.enabled = false;
    }

}
