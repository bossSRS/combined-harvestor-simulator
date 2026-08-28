using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObj : MonoBehaviour
{
    public float Angle;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Angle * Time.deltaTime, 0); 
    }
}
