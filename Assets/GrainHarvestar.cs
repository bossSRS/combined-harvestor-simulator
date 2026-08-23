using CHS;
using CHS.Input;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class GrainHarvestar : MonoBehaviour
{
    public CHS_Grain_Collection_Controller grainUnloaderController;
    public CHS_Input chs_Input;
    //public VisualEffect grain;
    private void OnTriggerEnter(Collider other)
    {
        if (chs_Input.thrashing_Lever == Thrashing_Lever.On && chs_Input.cutting_Lever == Cutting_Lever.On && chs_Input.DefMainTransmissionValue < 150f)
        {
            if (other.gameObject.activeSelf)
            {
                if (other.gameObject.tag == "Wheat")
                {
                    var grainCtr = other.gameObject.GetComponentInParent<GrainBehavior>();
                    if (grainCtr != null)
                    {
                        if (!grainCtr.isHarvested)
                        {
                            grainCtr.HarvestedGrain();
                            grainUnloaderController.GrainTankActive(1);
                            //grain.gameObject.SetActive(true);
                            //grain.SendEvent("OnPlay");
                        }
                    }
                }
            }
        }
    }/*
    private void OnTriggerExit(Collider other)
    {
        if (grain.gameObject.activeInHierarchy)
        {
            grain.SendEvent("OnStop");
            grain.gameObject.SetActive(false);
        }
    }*/
}
