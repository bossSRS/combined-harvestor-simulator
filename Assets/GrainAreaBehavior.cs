using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrainAreaBehavior : MonoBehaviour
{
    public List<GrainZoneBehavior> GrainZoneBehaviors;
    public bool isReveled;
    public void RevelArea()
    {
        if (isReveled) return;
        GetComponent<BoxCollider>().enabled = false;
        foreach(var zone in GrainZoneBehaviors)
        {
            zone.UnveilZone();
        }
    }
}
