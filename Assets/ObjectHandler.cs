using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHandler : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        PosObject();
    }

    public void PosObject()
    {
        var GO = GameObject.Find("Wheat_pos");
        if (GO != null)
        {
            if (GO.activeInHierarchy)
            {
                GO.SetActive(false);
            }
        }
    }
    public void NegObject()
    {
        var GO = GameObject.Find("Wheat_neg");
        if (GO != null)
        {
            if (GO.activeInHierarchy)
            {
                if (GO.GetComponent<Rigidbody>())
                {
                    GO.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    GO.transform.eulerAngles = Vector3.zero;
                    GO.GetComponent<Rigidbody>().Sleep();
                }
            }
        }
    }
}
