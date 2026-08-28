using ChobiAssets.PTM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMove : MonoBehaviour
{
    public Transform movement;
    public bool startHarvensting;
    [Range(0f, 1f)]
    public float torque;
    public Drive_Control_CS drive_Control;
    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        if (startHarvensting)
        {
            animator.SetFloat("speed", torque * drive_Control.RPM * Time.deltaTime);
            animator.Play("RotateBlade");
        }
        else
        {
            animator.Play("Idle");
        }
    }
}
