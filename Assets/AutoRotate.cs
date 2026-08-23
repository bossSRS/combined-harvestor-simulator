using ChobiAssets.PTM;
using CHS.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public string LeverName;
    [Range(0f, 1f)]
    public float torque;
    public bool startHarvesting;
    public Drive_Control_CS drive_Control;
    public static bool isReverse;

    // Update is called once per frame
    void Update()
    {
        if (startHarvesting)
        {
            if (isReverse)
            {
                transform.Rotate(-torque * Time.deltaTime * drive_Control.RPM, 0, 0);
            }
            else
            {
                transform.Rotate(torque * Time.deltaTime * drive_Control.RPM, 0, 0);
            }
        }
    }
}
