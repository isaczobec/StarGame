using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public Button button;

    [Header("Visuals")]
    [SerializeField] public Image buttonImage;
    [Header("Sound")]
    [SerializeField] public SoundPlayer soundPlayer;
    [SerializeField] public string onClickSoundName = "buttonClick";
    [SerializeField] public string onHoverSoundName = "buttonHover";
    [SerializeField] public string onHoverExitSoundName = "buttonHoverExit";

    /// <summary>
    /// Called when the button is clicked. Add your custom functionality to this event.
    /// </summary>
    public event EventHandler<EventArgs> OnUIButtonClicked;

    /// <summary>
    /// Called when the button is hovered. Add your custom functionality to this event. THe bool parameter is true if the button is hovered, false if it is not.
    /// </summary>
    public event EventHandler<bool> OnUIButtonHoveredChanged;

    public void Start()
    {
        button.onClick.AddListener(OnClickEvent);
        InitButton();
    }

    /// <summary>
    /// Initializes the button. Called in base class in Start(). Override this method instead of defining Start().
    /// </summary>
    public virtual void InitButton()
    {
    }


    /// <summary>
    /// Called when the button is clicked. Override this method to add custom functionality.
    /// </summary>
    public virtual void OnClick() {
    }

    /// <summary>
    /// Called when the button is hovered. Override this method to add custom functionality.
    /// </summary>
    public virtual void OnHoverEnter() {
    }

    /// <summary>
    /// Called when the button stops being hovered. Override this method to add custom functionality.
    /// </summary>
    public virtual void OnHoverExit() {
    }


    // METHODS NOT CALLED BY THIS CLASS
    public virtual void OnClickEvent()
    {
        soundPlayer?.PlayOneShot(onClickSoundName);
        OnClick();
        OnUIButtonClicked?.Invoke(this, EventArgs.Empty);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        soundPlayer?.PlayOneShot(onHoverSoundName);
        OnHoverEnter();
        OnUIButtonHoveredChanged?.Invoke(this, true);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        soundPlayer?.PlayOneShot(onHoverExitSoundName);
        OnHoverExit();
        OnUIButtonHoveredChanged?.Invoke(this, false);
    }

    public void InvokeOnClickEvent()
    {
        OnUIButtonClicked?.Invoke(this, EventArgs.Empty);
    }

    public void InvokeOnHoverChangedEnterEvent(bool hovered)
    {
        OnUIButtonHoveredChanged?.Invoke(this, hovered);
    }

    public void SetColor(Color color) {
        if (buttonImage != null) buttonImage.color = color;
    }
}
