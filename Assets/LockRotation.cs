using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
    public float MinValue;
    public float MaxValue;
    private Vector3 DefPosition;
    // Start is called before the first frame update
    void Start()
    {
        DefPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = DefPosition;
        transform.localEulerAngles = new Vector3(Mathf.Clamp(transform.localEulerAngles.x,MinValue,MaxValue),
            transform.localEulerAngles.y,
            transform.localEulerAngles.z
            );
    }
}
