using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DefaultPlayerColors: MonoBehaviour
{
    [SerializeField] private Color normalColor;
    [SerializeField] private Color zapColor;
    [SerializeField] private Color glideColor;
    [SerializeField] private Color hookColor;
    [SerializeField] private Color practiceColor;


    public static Dictionary<PlayerGameModeState, Color> defaultColorsDict {get; private set;} = new Dictionary<PlayerGameModeState, Color>();


    void Awake()
    {
        if (defaultColorsDict.Count > 0) return;
        defaultColorsDict.Add(PlayerGameModeState.Normal, normalColor);
        defaultColorsDict.Add(PlayerGameModeState.Zap, zapColor);
        defaultColorsDict.Add(PlayerGameModeState.Glide, glideColor);
        defaultColorsDict.Add(PlayerGameModeState.Hook, hookColor);
        defaultColorsDict.Add(PlayerGameModeState.Practice, practiceColor);
    }

    public static Color GetCurrentPlayerColor() {
        return defaultColorsDict[Player.Instance.GetGameModeState()];
    }

}
