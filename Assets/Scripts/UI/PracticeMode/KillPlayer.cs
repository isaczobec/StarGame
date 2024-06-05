using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : UIButton
{

    public override void OnClick()
    {
        if (Player.Instance.GetPracticeModeEnabled() != true) return; // donbt allow the player to kill themselves if practice mode is not enabled
        Player.Instance.KillPlayer();
    }
}
