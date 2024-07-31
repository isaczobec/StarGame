using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButtonHandler : MonoBehaviour
{

    [SerializeField] private GameObject levelButtonPrefab;

    [SerializeField] private LevelSO[] levelsToDisplay;

    [Header("Level button display settings")]
    [SerializeField] private int maxLevelColumns = 2;
    [SerializeField] private int defaultRowBeginOffset = 2;
    [SerializeField] private float columnSpacing = 23f;
    [SerializeField] private float rowSpacing = 23f;


    

    // Start is called before the first frame update
    void Start()
    {
        SpawnLevelButtons(levelsToDisplay);
    }

    private void SpawnSingleLevelButton(LevelSO levelSO, Vector3 position) {
        GameObject levelButton = Instantiate(levelButtonPrefab, transform);
        levelButton.transform.localPosition = position;
        levelButton.GetComponent<LevelSelectButton>().SetupButton(levelSO);
    }


    private void SpawnLevelButtons(LevelSO[] levelSOs) {

        int currentColumn = 0;
        int currentRow = 0;
        foreach (LevelSO levelSO in levelSOs) {
            if (currentColumn >= maxLevelColumns) {
                currentColumn = 0;
                currentRow++;
            }
            Vector3 pos = new Vector3(((float)currentColumn - ((maxLevelColumns-1)/2f)) * columnSpacing, (currentRow-defaultRowBeginOffset) * -rowSpacing, 0);
            SpawnSingleLevelButton(levelSO,pos);
            currentColumn++;
        }
    }

    

    

}
