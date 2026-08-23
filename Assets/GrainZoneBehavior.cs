using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrainZoneBehavior : MonoBehaviour
{
    public GameObject Board;
    public List<GrainBehavior> grainBehaviors;

    [Button]
    public void UnveilZone()
    {
        Board.SetActive(false);
        foreach (GrainBehavior grainBehavior in grainBehaviors)
        {
            if (!grainBehavior.isFound)
            {
                if (grainBehavior.FullBody != null)
                {
                    grainBehavior.gameObject.SetActive(true);
                }
                grainBehavior.isFound = true;
            }
        }
    }
    [Button]
    public void VeilZone()
    {
        Board.SetActive(true);
        foreach (GrainBehavior grainBehavior in grainBehaviors)
        {
            grainBehavior.gameObject.SetActive(false);
            grainBehavior.isFound = false;
        }
    }
}
