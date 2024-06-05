using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public Button button;
    public void Start()
    {
        button.onClick.AddListener(OnClick);
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHoverEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnHoverExit();
    }
}
