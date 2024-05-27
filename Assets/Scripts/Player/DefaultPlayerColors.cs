using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DefaultPlayerColors: MonoBehaviour
{
    [SerializeField] private Color normalColor;
    [SerializeField] private Color zapColor;
    [SerializeField] private Color glideColor;
    [SerializeField] private Color hookColor;


    public static Dictionary<PlayerGameModeState, Color> defaultColorsDict {get; private set;} = new Dictionary<PlayerGameModeState, Color>();


    void Awake()
    {
        defaultColorsDict.Add(PlayerGameModeState.Normal, normalColor);
        defaultColorsDict.Add(PlayerGameModeState.Zap, zapColor);
        defaultColorsDict.Add(PlayerGameModeState.Glide, glideColor);
        defaultColorsDict.Add(PlayerGameModeState.Hook, hookColor);
    }

    public static Color GetCurrentPlayerColor() {
        return defaultColorsDict[Player.Instance.GetGameModeState()];
    }

}
