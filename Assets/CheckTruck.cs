using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTruck : MonoBehaviour
{
    public bool isInTruck;
    public Transform CHS;
    public Vector3 targetPosition;
    public Vector3 targetRotation;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "CHS" && !isInTruck)
        {
            isInTruck = true;
            CHS = other.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "CHS" && isInTruck)
        {
            isInTruck = false;
        }
    }
    public void FixPosition()
    {
        if (CHS != null)
        {
            CHS.transform.localPosition = targetPosition;
            CHS.transform.DOLocalRotate(targetRotation,5f);
        }
    }
}
