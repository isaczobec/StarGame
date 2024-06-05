using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SpeedLines : MonoBehaviour
{

    [SerializeField] private VisualEffect speedVfx;
    [SerializeField] private string playerVelocityVectorName = "PlayerVelocity";
    [SerializeField] private string playerPositionVectorName = "PlayerPosition";


    void Update()
    {
        UpdatePlayerVelocityVector();   
        UpdatePlayerPositionVector();
    }

    private void UpdatePlayerVelocityVector() {
        Vector3 playerVelocity = Player.Instance.GetVelocity();
        speedVfx.SetVector3(playerVelocityVectorName, playerVelocity);
    }

    private void UpdatePlayerPositionVector() {
        Vector3 playerPosition = Player.Instance.transform.position;
        speedVfx.SetVector3("PlayerPosition", playerPosition);
    }
}
