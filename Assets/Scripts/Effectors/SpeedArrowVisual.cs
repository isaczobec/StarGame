using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedArrowVisual : MonoBehaviour
{

    [SerializeField] private float cycleOffset = 0.17f; // Offset for each arrow in the animation, seconds
    [SerializeField] private Animator[] arrowAnimators; // Animators for the arrows
    [SerializeField] private string cycleOffsetString = "CycleOffset";
    [SerializeField] private string speedArrowTriggeredString = "ArrowTriggered";

    [SerializeField] private SpeedArrowEffector speedArrowEffector; // The effector that this visual is linked to



    // Start is called before the first frame update
    void Start()
    {
        SetupArrowAnimators();

        speedArrowEffector.OnSpeedArrowVisualTriggered += SpeedArrowEffector_OnEffectorTriggered; // subscribe to the effector event
    }


    private void SetupArrowAnimators()
    {
        int counter = 0;
        float firstOffset = arrowAnimators.Length * cycleOffset;
        foreach (Animator animator in arrowAnimators)
        {
            animator.SetFloat(cycleOffsetString, firstOffset - cycleOffset * counter);
            counter++;
        }
    }

    private void SpeedArrowEffector_OnEffectorTriggered(object sender, EventArgs e)
    {
        foreach (Animator animator in arrowAnimators)
        {
            animator.SetTrigger(speedArrowTriggeredString);
        }
    }
}
