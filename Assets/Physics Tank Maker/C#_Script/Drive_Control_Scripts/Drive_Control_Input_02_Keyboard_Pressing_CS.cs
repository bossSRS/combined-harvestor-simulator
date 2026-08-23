using CHS;
using CHS.Input;
using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{

    public class Drive_Control_Input_02_Keyboard_Pressing_CS : Drive_Control_Input_01_Keyboard_Stepwise_CS
    {

        public override void Drive_Input()
        {
            if (chs_Input == null) chs_Input = Object.FindAnyObjectByType<CHS_Input>();
            if (chs_Controller == null) chs_Controller = Object.FindAnyObjectByType<CHS_Controller>();

            if (chs_Controller != null && chs_Controller.harvestorState == HarvestorState.Off)
            {
                vertical = 0f;
                horizontal = 0f;
                controlScript.Apply_Brake = true;
                Set_Values();
                return;
            }

            bool fwdKey = Input.GetKey(General_Settings_CS.Drive_Up_Key) || Input.GetKey(KeyCode.UpArrow);
            bool backKey = Input.GetKey(General_Settings_CS.Drive_Down_Key) || Input.GetKey(KeyCode.DownArrow);

            float drivePower = 0f;
            if (fwdKey)
            {
                drivePower = 1.0f;
            }
            else if (backKey)
            {
                drivePower = -1.0f;
            }
            else if (chs_Controller != null && Mathf.Abs(chs_Controller.TempAccleration) > 0.02f)
            {
                drivePower = chs_Controller.TempAccleration;
            }
            else if (chs_Input != null)
            {
                if (chs_Input.mainTransmissionLever == MainTransmissionLever.Increaseing) drivePower = 1.0f;
                else if (chs_Input.mainTransmissionLever == MainTransmissionLever.Decreaseing) drivePower = -1.0f;
            }

            if (chs_Input != null && chs_Input.subTransmissionLever == SubTransmissionLever.Neutral && !fwdKey && !backKey)
            {
                drivePower = 0f;
            }

            vertical = drivePower;

            bool leftKey = Input.GetKey(General_Settings_CS.Drive_Left_Key) || Input.GetKey(KeyCode.LeftArrow) || (chs_Input != null && chs_Input.steering_UpDownLeftRight == SteeringUpDownLeftRight.Left);
            bool rightKey = Input.GetKey(General_Settings_CS.Drive_Right_Key) || Input.GetKey(KeyCode.RightArrow) || (chs_Input != null && chs_Input.steering_UpDownLeftRight == SteeringUpDownLeftRight.Right);

            if (leftKey)
            {
                horizontal = -1.0f;
            }
            else if (rightKey)
            {
                horizontal = 1.0f;
            }
            else
            {
                horizontal = 0.0f;
            }

            // Control the brake.
            controlScript.Apply_Brake = Input.GetKey(General_Settings_CS.Drive_Brake_Key);

            // Set the "Stop_Flag", "L_Input_Rate", "R_Input_Rate" and "Turn_Brake_Rate".
            Set_Values();
        }

        bool getLeftOrRight()
        {
            if (chs_Input == null) return Input.GetKey(General_Settings_CS.Drive_Left_Key) || Input.GetKey(General_Settings_CS.Drive_Right_Key);
            return chs_Input.steering_UpDownLeftRight == SteeringUpDownLeftRight.Left || chs_Input.steering_UpDownLeftRight == SteeringUpDownLeftRight.Right || Input.GetKey(General_Settings_CS.Drive_Left_Key) || Input.GetKey(General_Settings_CS.Drive_Right_Key);
        }

        protected override void Set_Values()
        {
            // In case of stopping.
            if (vertical == 0.0f && horizontal == 0.0f)
            { // The tank should stop.
                controlScript.Stop_Flag = true;
                controlScript.L_Input_Rate = 0.0f;
                controlScript.R_Input_Rate = 0.0f;
                controlScript.Turn_Brake_Rate = 0.0f;
                controlScript.Pivot_Turn_Flag = false;
                return;
            }
            else
            { // The tank should be driving.
                controlScript.Stop_Flag = false;
            }

            // In case of going straight.
            if (horizontal == 0.0f)
            { // The tank should be going straight.
                controlScript.L_Input_Rate = -vertical;
                controlScript.R_Input_Rate = vertical;
                controlScript.Turn_Brake_Rate = 0.0f;
                controlScript.Pivot_Turn_Flag = false;
                return;
            }

            // In case of pivot-turn.
            if (controlScript.Allow_Pivot_Turn)
            { // Pivot-turn is allowed.
                if (vertical == 0.0f && controlScript.Speed_Rate <= controlScript.Pivot_Turn_Rate)
                { // The tank should be doing pivot-turn.
                    horizontal *= controlScript.Pivot_Turn_Rate;
                    controlScript.L_Input_Rate = -horizontal;
                    controlScript.R_Input_Rate = -horizontal;
                    controlScript.Turn_Brake_Rate = 0.0f;
                    controlScript.Pivot_Turn_Flag = true;
                    return;
                }
            }
            else
            { // Pivot-turn is not allowed.
                if (vertical == 0.0f)
                {
                    vertical = 1.0f;
                }
            }

            // In case of brake-turn.
            controlScript.Pivot_Turn_Flag = false;
            Brake_Turn();
        }

    }

}
