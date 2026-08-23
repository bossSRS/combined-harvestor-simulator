using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fixedRotation : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 StartRotation;
    void Start()
    {
        StartRotation = transform.eulerAngles;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.eulerAngles = new Vector3(StartRotation.x, transform.eulerAngles.y,transform.eulerAngles.z);
    }
}
