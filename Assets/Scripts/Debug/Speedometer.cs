using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speedometer : MonoBehaviour
{

    [SerializeField] private TMPro.TextMeshProUGUI speedText;

    // Update is called once per frame
    void Update()
    {
        speedText.text = Player.Instance.GetVelocity().magnitude.ToString("F2");
    }
}
