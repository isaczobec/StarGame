using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionButton : UIButton
{

    [SerializeField] private Vector2 direction;



    public override void OnClick()
    {
        if (Player.Instance.GetPracticeModeEnabled() != true) return; // donbt allow the player to change direction if practice mode is not enabled
        Player.Instance.SetPracticeModeSpawnDirection(direction);
    }
}

