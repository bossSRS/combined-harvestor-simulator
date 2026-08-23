using System.Collections;
using UnityEngine;
using CHS;
namespace ChobiAssets.PTM
{

    public class Drive_Control_Input_04_Twin_Sticks_CS : Drive_Control_Input_03_Single_Stick_CS
    {

        public override void Drive_Input()
        {
            
            if (CHS_Controller.isStarted)
            {
                vertical = Input.GetAxis("Vertical");
                horizontal = Input.GetAxis("Horizontal2");
            }
            else
            {
                vertical = 0.0f;
                horizontal = 0.0f;
            }

            Set_Values();
        }

    }

}
