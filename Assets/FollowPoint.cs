using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPoint : MonoBehaviour
{
    [SerializeField]
    private Transform Target;

    public void Update()
    {
        if(Target == null) return;
        else
        {
            transform.position = Target.position;
            transform.localEulerAngles = Vector3.zero;
        }
    }

}
