using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHS_LocomotionController : MonoBehaviour
{
    public Transform Locomotion;
    public CoordinateMapper CordMapper;
    // Start is called before the first frame update
    void Awake()
    {
        CordMapper = FindAnyObjectByType<CoordinateMapper>();
    }

    public void FormatXYDataToSend()
    {
        float AngleX = (Locomotion.transform.eulerAngles.z < 180f) ? Locomotion.transform.eulerAngles.z : Locomotion.transform.eulerAngles.z - 360;
        float AngleY = (Locomotion.transform.eulerAngles.x < 180f) ? Locomotion.transform.eulerAngles.x : Locomotion.transform.eulerAngles.x - 360;

        AngleX = Mathf.Clamp(AngleX, -10f, 10f);
        AngleY = Mathf.Clamp(AngleY, -10f, 10f);

        CordMapper.SetXYRotationValues(AngleX, AngleY);
    }
    // Update is called once per frame
    void Update()
    {
        FormatXYDataToSend();
    }
}
