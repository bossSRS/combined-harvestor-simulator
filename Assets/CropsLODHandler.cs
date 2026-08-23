using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropsLODHandler : MonoBehaviour
{
    public Transform FollowTarget;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector3(FollowTarget.position.x + offset.x,transform.position.y, FollowTarget.position.z + offset.z);
        //transform.localEulerAngles = FollowTarget.localEulerAngles;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            var grainAreaBehavior = other.gameObject.GetComponent<GrainAreaBehavior>();
            if(grainAreaBehavior != null)
            {
                if (grainAreaBehavior.isReveled) return;
                else
                {
                    grainAreaBehavior.RevelArea();
                    grainAreaBehavior.isReveled = true;
                }
            }
        }
        /*if(other.gameObject.tag == "Wheat")
        {
            var grainCtr = other.gameObject.GetComponentInParent<GrainBehavior>();
            if (grainCtr != null)
            {
                if (!grainCtr.isHarvested)
                {
                    if (!grainCtr.isFound)
                    {
                        grainCtr.isFound = true;
                        var CardOBJ = other.gameObject.transform.parent.parent.GetChild(15);
                        if (CardOBJ != null)
                        {
                            CardOBJ.gameObject.SetActive(false);
                        }
                        var GrainModel = other.gameObject.GetComponent<MeshRenderer>();
                        var GrainBody = other.gameObject.GetComponent<Rigidbody>();
                        var GrainJointBody = other.gameObject.transform.parent.GetChild(0).GetComponent<Rigidbody>();
                        if (GrainModel != null)
                        {
                            GrainModel.enabled = true;
                        }
                        if (GrainBody != null)
                        {
                            GrainBody.WakeUp();
                        }
                        if (GrainJointBody != null)
                        {
                            GrainJointBody.WakeUp();
                        }
                    }
                }
            }
        }*/
    }
}
