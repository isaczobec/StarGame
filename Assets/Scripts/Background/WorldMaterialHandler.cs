using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class WorldMaterialHandler : MonoBehaviour
{
    public Material backgroundMaterial;
    public Material[] tileMaterials;
    
    private const string color1Ref = "_Color1";
    private const string color2Ref = "_Color2";


    private const string tileMainColorRef = "_MainColor";


    private Dictionary<int, string> backgroundColorRefDict = new Dictionary<int, string>() {
        {0, color1Ref},
        {1, color2Ref}
    };

    private Dictionary<ColorType, Coroutine[]> changeColorCoroutinesDict = new Dictionary<ColorType, Coroutine[]>();

    private Coroutine[] changeColorCoroutines;
    private Coroutine[] changeTileColorCoroutines;

    public static WorldMaterialHandler instance {get; private set;}

    private void Awake() {
        changeColorCoroutines = new Coroutine[backgroundColorRefDict.Count];
        changeTileColorCoroutines = new Coroutine[tileMaterials.Length];
        // populate the dict
        changeColorCoroutinesDict.Add(ColorType.BackgroundColor, changeColorCoroutines);
        changeColorCoroutinesDict.Add(ColorType.TileColor, changeTileColorCoroutines);
        instance = this;
    }

    public void SetBackgroundColorAtIndex(int index, Color color, float transitionTime) {
        StartCoroutine(ChangeColorCoroutine(index, backgroundMaterial.GetColor(backgroundColorRefDict[index]), color, backgroundColorRefDict[index], transitionTime, backgroundMaterial, ColorType.BackgroundColor));
    }

    public void SetTileColorAtIndex(int index, Color color,float transitionTime) {
        StartCoroutine(ChangeColorCoroutine(index, tileMaterials[index].GetColor(tileMainColorRef), color, tileMainColorRef, transitionTime, tileMaterials[index], ColorType.TileColor));
    }

    private IEnumerator ChangeColorCoroutine(int index, Color startColor, Color color, string colorReferenceString, float transitionTime, Material material, ColorType colorType) {
        if (changeColorCoroutinesDict[colorType][index] != null) {
            StopCoroutine(changeColorCoroutines[index]);
        }
        float passedTime = 0f;
        while (passedTime < transitionTime) {
            passedTime += Time.deltaTime;
            material.SetColor(colorReferenceString, Color.Lerp(startColor, color, passedTime / transitionTime));
            yield return null;
        }
        material.SetColor(backgroundColorRefDict[index], color);
        changeColorCoroutinesDict[colorType][index] = null;
    }

}

public enum ColorType {
    BackgroundColor,
    TileColor
}
