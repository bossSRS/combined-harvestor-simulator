using CHS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrainBehavior : MonoBehaviour
{
    public enum GrainType
    {
        None,
        Paddy,
        Wheat
    }

    public GrainType type;
    public bool isHarvested;
    public bool isFound;

    public GameObject FullBody;
    public Rigidbody FullBodyrb;
    public Rigidbody JointBodyrb;

    public GameObject LowerHalf;

    private void Start()
    {
        FullBodyrb.Sleep();
        JointBodyrb.Sleep();
    }
    public void HarvestedGrain()
    {
        isHarvested = true;
        CHS_Controller.instance.HarvestedCropCount++;
        JointBodyrb.gameObject.SetActive(false);
        FullBody.gameObject.SetActive(false);
        if (LowerHalf != null)
        {
            LowerHalf.SetActive(true);
        }
    }
}
