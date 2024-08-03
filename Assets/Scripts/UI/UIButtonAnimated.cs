using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonAnimated : UIButton {

    [Header("Setiings")]
    [SerializeField] private bool showByDefault = false;

    [SerializeField] private TMPro.TextMeshProUGUI buttonText;

    [Header("Visuals")]
    [Header("Colors")]

    [SerializeField] private Color baseColor = Color.white;
    [SerializeField] private Color socketColor = Color.gray;
    [SerializeField] private Color borderColor = Color.green;

    [Header("Animator")]
    [SerializeField] private Animator buttonAnimator;
    [SerializeField] private float cycleOffsetRandomMax = 3f;
    
    [Header("Shader and sprite")]
    [SerializeField] private Shader buttonShader;
    [SerializeField] private Image buttonImage;

    [Header("MoveToNewPosition")]
    [SerializeField] private AnimationCurve moveToNewPositionCurve;



    private const string isHoveredRef = "_IsHovered";
    private const string mainColorRef = "_MainColor";
    private const string socketColorRef = "_SocketColor";
    private const string borderColorRef = "_BorderColor";

    private const string begingHoverRef = "BeginHover";
    private const string endHoverRef = "EndHover";
    private const string appearRef = "Appear";
    private const string disappearRef = "Disappear";
    private const string cycleOffsetRef = "CycleOffset";

    private const string appearSpeedRef = "AppearSpeed";


    // member variables

    private bool isVisible;

    // coroutines

    private Coroutine appearCoroutine;
    private Coroutine moveToNewPositionCoroutine;

    public override void InitButton()
    {
        buttonAnimator?.SetFloat(cycleOffsetRef, UnityEngine.Random.Range(0, cycleOffsetRandomMax));

        // set materials and colors
        if (buttonShader != null) {
            buttonImage.material = new Material(buttonShader);
            // set colors
            buttonImage.material.SetColor(mainColorRef, baseColor);
            buttonImage.material.SetColor(socketColorRef, socketColor);
            buttonImage.material.SetColor(borderColorRef, borderColor);
        }


        ChangeButtonVisualsVisible(showByDefault);


        InitializeAnimatedButton();
    }

    public virtual void InitializeAnimatedButton() {

    }

    public void SetText(string text) {
        buttonText.text = text;
    }


    public override void OnClickEvent()
    {
        if (!isVisible) return;
        soundPlayer?.PlayOneShot(onClickSoundName);
        InvokeOnClickEvent();
    }


    // --------- VISUALS

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (!isVisible) return;
        soundPlayer?.PlayOneShot(onHoverSoundName);
        buttonAnimator?.SetTrigger(begingHoverRef);
        if (buttonShader != null) buttonImage.material.SetFloat(isHoveredRef, 1f);
        InvokeOnHoverChangedEnterEvent(true);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (!isVisible) return;
        soundPlayer?.PlayOneShot(onHoverExitSoundName);
        buttonAnimator?.SetTrigger(endHoverRef);
        if (buttonShader!= null) buttonImage.material.SetFloat(isHoveredRef, 0f);
        InvokeOnHoverChangedEnterEvent(false);
    }


    /// <summary>
    /// Make the button appear. This is called by the IEnumerator WaitToAppearCoroutine coroutine.
    /// </summary>
    private void ChangeButtonVisualsVisible(bool visible, float speedMultiplier = 1f) {
        buttonAnimator.SetFloat(appearSpeedRef, speedMultiplier);
        if (visible) {
            buttonAnimator.SetTrigger(appearRef);
        } else {
            buttonAnimator.SetTrigger(disappearRef);
        }
        isVisible = visible;
    }

    private IEnumerator WaitToAppearCoroutine(bool visible, float timeUntilAppearStart, float speedMultiplier = 1f) {
        yield return new WaitForSeconds(timeUntilAppearStart);
        ChangeButtonVisualsVisible(visible, speedMultiplier);
    }

    /// <summary>
    /// Begin the wait for the button to appear. After the timeUntilAppearStart, the button will appear
    /// </summary>
    /// <param name="timeUntilAppearStart"></param>
    public void ChangeVisible(bool visible, float timeUntilStart = 0f, float speedMultiplier = 1f) {
        if (isVisible == visible) return;
        if (appearCoroutine != null) {
            StopCoroutine(appearCoroutine);
        }
        if (timeUntilStart <= 0) {
            ChangeButtonVisualsVisible(visible, speedMultiplier);
            return;
        }
        appearCoroutine = StartCoroutine(WaitToAppearCoroutine(visible,timeUntilStart));
    }


    public void MoveToNewPosition(Vector3 targetPosition, float timeToMove) {
        if (moveToNewPositionCoroutine != null) {
            StopCoroutine(moveToNewPositionCoroutine);
        }
        moveToNewPositionCoroutine = StartCoroutine(MoveToNewPositionCoroutine(targetPosition, timeToMove));
    }

    private IEnumerator MoveToNewPositionCoroutine(Vector3 targetPosition, float timeToMove) {
        Vector3 startPosition = transform.position;
        float timeElapsed = 0;
        while (timeElapsed < timeToMove) {
            timeElapsed += Time.deltaTime;
            float t = moveToNewPositionCurve.Evaluate(timeElapsed / timeToMove);
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }
        transform.position = targetPosition;
    }

}