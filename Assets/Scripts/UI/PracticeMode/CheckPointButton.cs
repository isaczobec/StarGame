using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointButton : UIButton
{

    [SerializeField] private GameObject checkPointPrefab;

    private GameObject checkPoint;

    public override void OnClick()
    {
        if (Player.Instance.GetPracticeModeEnabled() != true) return; // donbt allow the player to set a checkpoint if practice mode is not enabled
        PlaceCheckPoint();
    }

    private void PlaceCheckPoint()
    {
        if (checkPoint != null) Destroy(checkPoint);
        checkPoint = Instantiate(checkPointPrefab, Player.Instance.transform.position, Quaternion.identity);
        Player.Instance.SetPracticeModeSpawnPoint(Player.Instance.transform.position);
    }

}
