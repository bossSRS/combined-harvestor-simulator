using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class localPositionLocker : MonoBehaviour
{

    Vector3 DefLocalPos;

    private void Start()
    {
        DefLocalPos = transform.localPosition;
    }
    // Update is called once per frame
    void Update()
    {
        transform.localPosition = DefLocalPos;
    }
}
