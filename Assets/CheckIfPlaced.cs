using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfPlaced : MonoBehaviour
{
    public GrainUnloaderController controller;
    [SerializeField]
    string ZoneTag;
    [SerializeField]
    string ObjectTag;
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == ZoneTag && !controller.isPlaceOnPosition)
        {
            controller.isPlaceOnPosition = true;
            print("Pipe Placed");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == ZoneTag && controller.isPlaceOnPosition)
        {
            controller.isPlaceOnPosition = false;
            print("Pipe Removed");
        }
    }
}
