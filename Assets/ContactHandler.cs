using CHS.Tutorial;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHS 
{
    public class ContactHandler : MonoBehaviour
    {
        public CHS_CameraController chs_Controller;
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "WorkingZone")
            {
                if (chs_Controller.tutorialManager != null)
                {
                    if (!chs_Controller.tutorialManager.isZoneWarningDone)
                    {
                        chs_Controller.tutorialManager.ZoneWarningMessage();
                    }
                }
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "RoadTrigger")
            {
                if (chs_Controller.tutorialManager != null)
                {
                    if (!chs_Controller.tutorialManager.isRoadWarningDone)
                    {
                        chs_Controller.tutorialManager.RoadWarningMessage();
                    }
                }
            }
            if (other.gameObject.tag == "BigAile")
            {
                if (chs_Controller.tutorialManager != null)
                {
                    if (!chs_Controller.tutorialManager.isBigAileWarningDone)
                    {
                        chs_Controller.tutorialManager.BigAileWarningMessage();
                    }
                }
            }
            if (other.gameObject.tag == "WaterField")
            {
                if (chs_Controller.tutorialManager != null)
                {
                    if (!chs_Controller.tutorialManager.isWaterWarningDone)
                    {
                        chs_Controller.tutorialManager.WaterWarningMessage();
                    }
                }
            }
        }

    }
}
