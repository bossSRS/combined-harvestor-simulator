using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHS.Input
{
    public enum IgnitionSwitch
    {
        Neutral = 1,
        Started = 2
    }
    public enum RPM
    {
        Increasing = 1,
        Neutral = 2,
        Decreasing = 3
    }
    public enum SubTransmissionLever
    {
        Harvesting = 1,
        Neutral = 2,
        Travel = 3,
        Loading = 4
    }
    public enum MainTransmissionLever
    {
        Increaseing = 1,
        Neutral = 2,
        Decreaseing = 3
    }

    public enum SteeringUpDownLeftRight
    {
        Neutral = 0,
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4
    }

    public enum ReelControlLever
    {
        Neutral = 0,
        Up = 1,
        Down = 2
    }
    public enum Thrashing_Lever
    {
        Off = 0,
        On = 1
    }
    public enum Cutting_Lever
    {
        Off = 0,
        On = 1
    }
    public enum Unloading_Cylinder_Lever
    {
        Neutral = 0,
        Up = 1,
        Down = 2
    }
    public enum Unloading_Pipe_Control_Switch
    {
        Neutral = 0,
        Left = 1,
        Right = 2
    }
    public enum Grain_Main_Clutch_Lever
    {
        On = 1,
        Off = 2
    }
    public enum Horn_Light_Switch
    {
        On = 1,
        Off = 2
    }
    public enum Reverse_Feeder_Chain_Lever
    {
        Forward = 1,
        Backward = 2,
    }
    public enum Stop_Cable
    {
        On = 0,
        Off = 1
    }
    public enum Neutral_Switch
    {
        On = 0,
        Off = 1
    }
    public enum Clutch_Paddle
    {
        Up = 1, 
        Down = 2
    }

    
    [RequireComponent(typeof(CHS_Simulator_Comms))]
    public class CHS_Input : MonoBehaviour
    {
        public CHS_Simulator_Comms Simulator_Comms;

        public IgnitionSwitch startSwitch;
        public RPM rpmLever;
        public SubTransmissionLever subTransmissionLever;
        public MainTransmissionLever mainTransmissionLever;
        public SteeringUpDownLeftRight steering_UpDownLeftRight;
        public Thrashing_Lever thrashing_Lever;
        public Cutting_Lever cutting_Lever;
        public ReelControlLever reelControlLever;
        public Grain_Main_Clutch_Lever grainLever;
        public Horn_Light_Switch hornLightSwitch;
        public Unloading_Cylinder_Lever unloadingCylinderLever;
        public Unloading_Pipe_Control_Switch unloadingPipeControlLever;
        public Reverse_Feeder_Chain_Lever reverse_Feeder_Chain_Lever;
        public Stop_Cable stopCable;
        public Neutral_Switch neutralSwitch;
        public Clutch_Paddle clutch_Paddle;

        [Header("Keyboard Controls Configuration")]
        public bool enableKeyboardInput = true;

        [Header("Keyboard Controls: Engine & Power")]
        public KeyCode keyIgnitionStart = KeyCode.Alpha1;
        public KeyCode keyStopCable = KeyCode.Escape;
        public KeyCode keyNeutralSwitch = KeyCode.N;
        public KeyCode keyClutchPaddle = KeyCode.Space;

        [Header("Keyboard Controls: RPM / Throttle")]
        public KeyCode keyRPMIncrease = KeyCode.PageUp;
        public KeyCode keyRPMDecrease = KeyCode.PageDown;
        public KeyCode keyRPMIncreaseAlt = KeyCode.LeftShift;
        public KeyCode keyRPMDecreaseAlt = KeyCode.LeftControl;
        public KeyCode keyRPMFull = KeyCode.Home;
        public KeyCode keyRPMIdle = KeyCode.End;
        public float rpmKeyboardChangeSpeed = 150f;

        [Header("Keyboard Controls: Main Transmission (Speed Lever)")]
        public KeyCode keyTransmissionForward = KeyCode.W;
        public KeyCode keyTransmissionForwardAlt = KeyCode.UpArrow;
        public KeyCode keyTransmissionBackward = KeyCode.S;
        public KeyCode keyTransmissionBackwardAlt = KeyCode.DownArrow;
        public KeyCode keyTransmissionNeutral = KeyCode.X;
        public float transmissionKeyboardChangeSpeed = 30f;

        [Header("Keyboard Controls: Sub-Transmission (Gears)")]
        public KeyCode keyGearHarvesting = KeyCode.Alpha2;
        public KeyCode keyGearTravel = KeyCode.Alpha3;
        public KeyCode keyGearLoading = KeyCode.Alpha4;
        public KeyCode keyGearCycle = KeyCode.G;

        [Header("Keyboard Controls: Cutter & Threshing")]
        public KeyCode keyCutterHeightUp = KeyCode.I;
        public KeyCode keyCutterHeightDown = KeyCode.K;
        public KeyCode keyReelHeightUp = KeyCode.O;
        public KeyCode keyReelHeightDown = KeyCode.L;
        public KeyCode keyThrashingToggle = KeyCode.T;
        public KeyCode keyThrashingToggleAlt = KeyCode.F;
        public KeyCode keyCuttingToggle = KeyCode.C;
        public KeyCode keyCuttingToggleAlt = KeyCode.E;
        public KeyCode keyReverseFeederToggle = KeyCode.R;

        [Header("Keyboard Controls: Steering")]
        public KeyCode keySteerLeft = KeyCode.A;
        public KeyCode keySteerLeftAlt = KeyCode.LeftArrow;
        public KeyCode keySteerRight = KeyCode.D;
        public KeyCode keySteerRightAlt = KeyCode.RightArrow;

        [Header("Keyboard Controls: Unloader Auger")]
        public KeyCode keyUnloadArmUp = KeyCode.U;
        public KeyCode keyUnloadArmDown = KeyCode.N;
        public KeyCode keyUnloadArmDownAlt = KeyCode.M;
        public KeyCode keyUnloadSwivelLeft = KeyCode.H;
        public KeyCode keyUnloadSwivelRight = KeyCode.J;
        public KeyCode keyGrainDischargeToggle = KeyCode.Y;
        public KeyCode keyGrainDischargeToggleAlt = KeyCode.P;

        [Header("Keyboard Controls: Auxiliary & System")]
        public KeyCode keyHornLightToggle = KeyCode.V;
        public KeyCode keyResetAllNeutral = KeyCode.Z;
        public KeyCode keyResetAllNeutralAlt = KeyCode.F2;

        [Header("Keyboard Controls HUD (Top-Left)")]
        public bool showKeyboardHUD = true;
        public bool isHUDCollapsed = false;
        public KeyCode toggleHUDKey = KeyCode.F1;

        public int DefRPMValue;
        [HideInInspector] public int MinRPMValue;
        [HideInInspector] public int MaxRPMValue;

        public int DefMainTransmissionValue;
        [HideInInspector] public int DefTransmissionInputMin;
        [HideInInspector] public int DefTransmissionInputMax;

        [HideInInspector] public int TransmissionInputMin;
        [HideInInspector] public int TransmissionInputMax;

        private float currentMainTransmissionFloat = 155f;
        private float currentRPMFloat = 305f;

        // Start is called before the first frame update
        void Start()
        {
            InitializeInput();
        }
        private void InitializeInput()
        {
            Simulator_Comms = GetComponent<CHS_Simulator_Comms>();
            MinRPMValue = 305;  // stables values from Simulator Controller
            MaxRPMValue = 0;  // stables values from Simulator Controller
            DefTransmissionInputMin = 150;
            DefTransmissionInputMax = 160;
            TransmissionInputMin = 130;
            TransmissionInputMax = 170;
            ResetAllToNeutral();
        }

        public void ResetAllToNeutral()
        {
            startSwitch = IgnitionSwitch.Neutral;
            rpmLever = RPM.Neutral;
            currentRPMFloat = MinRPMValue > 0 ? MinRPMValue : 305f;
            DefRPMValue = Mathf.RoundToInt(currentRPMFloat);
            subTransmissionLever = SubTransmissionLever.Neutral;
            mainTransmissionLever = MainTransmissionLever.Neutral;
            currentMainTransmissionFloat = 155f;
            DefMainTransmissionValue = 155;
            steering_UpDownLeftRight = SteeringUpDownLeftRight.Neutral;
            reelControlLever = ReelControlLever.Neutral;
            thrashing_Lever = Thrashing_Lever.Off;
            cutting_Lever = Cutting_Lever.Off;
            unloadingCylinderLever = Unloading_Cylinder_Lever.Neutral;
            unloadingPipeControlLever = Unloading_Pipe_Control_Switch.Neutral;
            grainLever = Grain_Main_Clutch_Lever.Off;
            hornLightSwitch = Horn_Light_Switch.Off;
            reverse_Feeder_Chain_Lever = Reverse_Feeder_Chain_Lever.Forward;
            stopCable = Stop_Cable.Off;
            neutralSwitch = Neutral_Switch.Off;
            clutch_Paddle = Clutch_Paddle.Up;
        }
        public bool CheckNetural()
        {
            if (neutralSwitch == Neutral_Switch.On)
            {
                bool CheckAll = (startSwitch == IgnitionSwitch.Started &&
                                rpmLever == RPM.Neutral &&
                                subTransmissionLever == SubTransmissionLever.Neutral &&
                                mainTransmissionLever == MainTransmissionLever.Neutral &&
                                steering_UpDownLeftRight == SteeringUpDownLeftRight.Neutral &&
                                reelControlLever == ReelControlLever.Neutral &&
                                grainLever == Grain_Main_Clutch_Lever.Off &&
                                hornLightSwitch == Horn_Light_Switch.Off &&
                                reverse_Feeder_Chain_Lever == Reverse_Feeder_Chain_Lever.Forward &&
                                stopCable == Stop_Cable.Off &&
                                neutralSwitch == Neutral_Switch.On &&
                                clutch_Paddle == Clutch_Paddle.Up);
                return CheckAll;
            }
            else return false;
        }
        public bool CheckNetural(bool isCheckingForTruck)
        {
            bool CheckAll = (startSwitch == IgnitionSwitch.Neutral &&
                                DefRPMValue > 295f &&
                                subTransmissionLever == SubTransmissionLever.Neutral &&
                                mainTransmissionLever == MainTransmissionLever.Neutral &&
                                steering_UpDownLeftRight == SteeringUpDownLeftRight.Neutral &&
                                reelControlLever == ReelControlLever.Neutral &&
                                grainLever == Grain_Main_Clutch_Lever.Off &&
                                hornLightSwitch == Horn_Light_Switch.Off &&
                                reverse_Feeder_Chain_Lever == Reverse_Feeder_Chain_Lever.Forward &&
                                stopCable == Stop_Cable.On &&
                                neutralSwitch == Neutral_Switch.Off &&
                                clutch_Paddle == Clutch_Paddle.Up);
            return CheckAll;
        }
        public bool CheckNetural(string tutorial)
        {
            bool CheckAll = (startSwitch == IgnitionSwitch.Neutral &&
                                DefRPMValue > 295f &&
                                subTransmissionLever == SubTransmissionLever.Neutral &&
                                mainTransmissionLever == MainTransmissionLever.Neutral &&
                                steering_UpDownLeftRight == SteeringUpDownLeftRight.Neutral &&
                                reelControlLever == ReelControlLever.Neutral &&
                                grainLever == Grain_Main_Clutch_Lever.Off &&
                                hornLightSwitch == Horn_Light_Switch.Off &&
                                reverse_Feeder_Chain_Lever == Reverse_Feeder_Chain_Lever.Forward &&
                                stopCable == Stop_Cable.Off &&
                                neutralSwitch == Neutral_Switch.Off &&
                                clutch_Paddle == Clutch_Paddle.Up);
            return CheckAll;
        }
        private void Update()
        {
            bool serialActive = (Simulator_Comms != null && Simulator_Comms.state == State.Active);

            if (serialActive)
            {
                if (int.TryParse(Simulator_Comms.GetRPMData(), out int parsedRPM))
                    DefRPMValue = parsedRPM;
                if (int.TryParse(Simulator_Comms.GetMainTransmissonInput(), out int parsedTrans))
                    DefMainTransmissionValue = parsedTrans;

                GetSteeringUDLRValue();
                GetReelControlWheelUDValue();
                GetThrashingValue();
                GetCuttingValue();
                GetUnloadControllSwitchValue();
                GetUnloadPipeValue();
                GetSubTransmissionLeverSwitch();

                GetIgnitionSwitchInput();
                GetStopCableInput();
                GetReverseFeedleChainInput();
                GrainMainClutchInput();
                GetClutchPaddle();
                GetNeutralSwitch();
            }

            if (enableKeyboardInput)
            {
                ProcessKeyboardInput(serialActive);
            }
        }

        private void ProcessKeyboardInput(bool serialActive)
        {
            // 0. Reset All Levers To Neutral Function Key
            if (UnityEngine.Input.GetKeyDown(keyResetAllNeutral) || UnityEngine.Input.GetKeyDown(keyResetAllNeutralAlt))
            {
                ResetAllToNeutral();
                CHS_Controller controller = GetComponent<CHS_Controller>() ?? FindAnyObjectByType<CHS_Controller>();
                if (controller != null && controller.harvestorState == HarvestorState.Off)
                {
                    controller.ResetOff();
                }
            }

            // 1. Ignition Start & Stop Cable
            if (UnityEngine.Input.GetKeyDown(keyIgnitionStart) || UnityEngine.Input.GetKeyDown(KeyCode.Return))
            {
                if (startSwitch == IgnitionSwitch.Neutral)
                {
                    startSwitch = IgnitionSwitch.Started;
                    stopCable = Stop_Cable.Off;
                    neutralSwitch = Neutral_Switch.On;
                    clutch_Paddle = Clutch_Paddle.Up;
                }
                else
                {
                    startSwitch = IgnitionSwitch.Neutral;
                }
            }

            if (UnityEngine.Input.GetKeyDown(keyStopCable) || UnityEngine.Input.GetKeyDown(KeyCode.Backspace))
            {
                stopCable = Stop_Cable.On;
                startSwitch = IgnitionSwitch.Neutral;
            }

            // 2. Safety Interlocks: Neutral Switch & Clutch Pedal
            if (UnityEngine.Input.GetKeyDown(keyNeutralSwitch))
            {
                neutralSwitch = (neutralSwitch == Neutral_Switch.On) ? Neutral_Switch.Off : Neutral_Switch.On;
            }

            if (UnityEngine.Input.GetKeyDown(keyClutchPaddle) || UnityEngine.Input.GetKeyDown(KeyCode.LeftAlt))
            {
                clutch_Paddle = (clutch_Paddle == Clutch_Paddle.Down) ? Clutch_Paddle.Up : Clutch_Paddle.Down;
            }

            // 3. Sub-Transmission Gear Selection
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad1))
            {
                if (startSwitch == IgnitionSwitch.Started)
                    subTransmissionLever = SubTransmissionLever.Neutral;
            }
            if (UnityEngine.Input.GetKeyDown(keyGearHarvesting) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad2))
            {
                subTransmissionLever = SubTransmissionLever.Harvesting;
            }
            if (UnityEngine.Input.GetKeyDown(keyGearTravel) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad3))
            {
                subTransmissionLever = SubTransmissionLever.Travel;
            }
            if (UnityEngine.Input.GetKeyDown(keyGearLoading) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad4))
            {
                subTransmissionLever = SubTransmissionLever.Loading;
            }
            if (UnityEngine.Input.GetKeyDown(keyGearCycle))
            {
                if (subTransmissionLever == SubTransmissionLever.Neutral) subTransmissionLever = SubTransmissionLever.Harvesting;
                else if (subTransmissionLever == SubTransmissionLever.Harvesting) subTransmissionLever = SubTransmissionLever.Travel;
                else if (subTransmissionLever == SubTransmissionLever.Travel) subTransmissionLever = SubTransmissionLever.Loading;
                else subTransmissionLever = SubTransmissionLever.Neutral;
            }

            // 4. Engine RPM Lever
            bool rpmIncreasingKey = UnityEngine.Input.GetKey(keyRPMIncrease) || UnityEngine.Input.GetKey(keyRPMIncreaseAlt);
            bool rpmDecreasingKey = UnityEngine.Input.GetKey(keyRPMDecrease) || UnityEngine.Input.GetKey(keyRPMDecreaseAlt);

            if (rpmIncreasingKey)
            {
                currentRPMFloat = Mathf.Clamp(currentRPMFloat - rpmKeyboardChangeSpeed * Time.deltaTime, MaxRPMValue, MinRPMValue);
                DefRPMValue = Mathf.RoundToInt(currentRPMFloat);
                rpmLever = RPM.Increasing;
            }
            else if (rpmDecreasingKey)
            {
                currentRPMFloat = Mathf.Clamp(currentRPMFloat + rpmKeyboardChangeSpeed * Time.deltaTime, MaxRPMValue, MinRPMValue);
                DefRPMValue = Mathf.RoundToInt(currentRPMFloat);
                rpmLever = RPM.Decreasing;
            }
            else if (UnityEngine.Input.GetKeyDown(keyRPMFull))
            {
                currentRPMFloat = MaxRPMValue;
                DefRPMValue = MaxRPMValue;
                rpmLever = RPM.Increasing;
            }
            else if (UnityEngine.Input.GetKeyDown(keyRPMIdle))
            {
                currentRPMFloat = MinRPMValue;
                DefRPMValue = MinRPMValue;
                rpmLever = RPM.Decreasing;
            }
            else if (!serialActive)
            {
                rpmLever = RPM.Neutral;
            }

            // 5. Main Transmission Lever (Speed & Throttle)
            bool transFwd = UnityEngine.Input.GetKey(keyTransmissionForward) || UnityEngine.Input.GetKey(keyTransmissionForwardAlt);
            bool transBack = UnityEngine.Input.GetKey(keyTransmissionBackward) || UnityEngine.Input.GetKey(keyTransmissionBackwardAlt);

            if (transFwd)
            {
                currentMainTransmissionFloat = Mathf.Clamp(currentMainTransmissionFloat - transmissionKeyboardChangeSpeed * Time.deltaTime, TransmissionInputMin, TransmissionInputMax);
                DefMainTransmissionValue = Mathf.RoundToInt(currentMainTransmissionFloat);
                mainTransmissionLever = MainTransmissionLever.Increaseing;
            }
            else if (transBack)
            {
                currentMainTransmissionFloat = Mathf.Clamp(currentMainTransmissionFloat + transmissionKeyboardChangeSpeed * Time.deltaTime, TransmissionInputMin, TransmissionInputMax);
                DefMainTransmissionValue = Mathf.RoundToInt(currentMainTransmissionFloat);
                mainTransmissionLever = MainTransmissionLever.Decreaseing;
            }
            else if (UnityEngine.Input.GetKeyDown(keyTransmissionNeutral))
            {
                currentMainTransmissionFloat = 155f;
                DefMainTransmissionValue = 155;
                mainTransmissionLever = MainTransmissionLever.Neutral;
            }
            else if (!serialActive)
            {
                if (DefMainTransmissionValue >= DefTransmissionInputMin && DefMainTransmissionValue <= DefTransmissionInputMax)
                    mainTransmissionLever = MainTransmissionLever.Neutral;
                else if (DefMainTransmissionValue < DefTransmissionInputMin)
                    mainTransmissionLever = MainTransmissionLever.Increaseing;
                else
                    mainTransmissionLever = MainTransmissionLever.Decreaseing;
            }

            // 6. Steering & Cutter Header Height
            if (UnityEngine.Input.GetKey(keyCutterHeightUp))
            {
                steering_UpDownLeftRight = SteeringUpDownLeftRight.Up;
            }
            else if (UnityEngine.Input.GetKey(keyCutterHeightDown))
            {
                steering_UpDownLeftRight = SteeringUpDownLeftRight.Down;
            }
            else if (UnityEngine.Input.GetKey(keySteerLeft) || UnityEngine.Input.GetKey(keySteerLeftAlt))
            {
                steering_UpDownLeftRight = SteeringUpDownLeftRight.Left;
            }
            else if (UnityEngine.Input.GetKey(keySteerRight) || UnityEngine.Input.GetKey(keySteerRightAlt))
            {
                steering_UpDownLeftRight = SteeringUpDownLeftRight.Right;
            }
            else if (!serialActive)
            {
                steering_UpDownLeftRight = SteeringUpDownLeftRight.Neutral;
            }

            // 7. Reel Height (Cutter Outer Joint)
            if (UnityEngine.Input.GetKey(keyReelHeightUp))
            {
                reelControlLever = ReelControlLever.Up;
            }
            else if (UnityEngine.Input.GetKey(keyReelHeightDown))
            {
                reelControlLever = ReelControlLever.Down;
            }
            else if (!serialActive)
            {
                reelControlLever = ReelControlLever.Neutral;
            }

            // 8. Thrashing Lever
            if (UnityEngine.Input.GetKeyDown(keyThrashingToggle) || UnityEngine.Input.GetKeyDown(keyThrashingToggleAlt))
            {
                thrashing_Lever = (thrashing_Lever == Thrashing_Lever.On) ? Thrashing_Lever.Off : Thrashing_Lever.On;
            }

            // 9. Cutting Lever
            if (UnityEngine.Input.GetKeyDown(keyCuttingToggle) || UnityEngine.Input.GetKeyDown(keyCuttingToggleAlt))
            {
                cutting_Lever = (cutting_Lever == Cutting_Lever.On) ? Cutting_Lever.Off : Cutting_Lever.On;
            }

            // 10. Reverse Feeder Chain
            if (UnityEngine.Input.GetKeyDown(keyReverseFeederToggle))
            {
                reverse_Feeder_Chain_Lever = (reverse_Feeder_Chain_Lever == Reverse_Feeder_Chain_Lever.Forward) ? Reverse_Feeder_Chain_Lever.Backward : Reverse_Feeder_Chain_Lever.Forward;
            }

            // 11. Unloading Pipe Cylinder (Arm Up / Down)
            if (UnityEngine.Input.GetKey(keyUnloadArmUp))
            {
                unloadingCylinderLever = Unloading_Cylinder_Lever.Up;
            }
            else if (UnityEngine.Input.GetKey(keyUnloadArmDown) || UnityEngine.Input.GetKey(keyUnloadArmDownAlt))
            {
                unloadingCylinderLever = Unloading_Cylinder_Lever.Down;
            }
            else if (!serialActive)
            {
                unloadingCylinderLever = Unloading_Cylinder_Lever.Neutral;
            }

            // 12. Unloading Pipe Swivel (Left / Right)
            if (UnityEngine.Input.GetKey(keyUnloadSwivelLeft))
            {
                unloadingPipeControlLever = Unloading_Pipe_Control_Switch.Left;
            }
            else if (UnityEngine.Input.GetKey(keyUnloadSwivelRight))
            {
                unloadingPipeControlLever = Unloading_Pipe_Control_Switch.Right;
            }
            else if (!serialActive)
            {
                unloadingPipeControlLever = Unloading_Pipe_Control_Switch.Neutral;
            }

            // 13. Grain Main Clutch (Discharge)
            if (UnityEngine.Input.GetKeyDown(keyGrainDischargeToggle) || UnityEngine.Input.GetKeyDown(keyGrainDischargeToggleAlt))
            {
                grainLever = (grainLever == Grain_Main_Clutch_Lever.On) ? Grain_Main_Clutch_Lever.Off : Grain_Main_Clutch_Lever.On;
            }

            // 14. Horn & Lights
            if (UnityEngine.Input.GetKeyDown(keyHornLightToggle))
            {
                hornLightSwitch = (hornLightSwitch == Horn_Light_Switch.On) ? Horn_Light_Switch.Off : Horn_Light_Switch.On;
            }
        }

        public int CtrRPMLever()
        {
            int CurrentRPM = DefRPMValue;
            if (Simulator_Comms != null && Simulator_Comms.state == State.Active)
            {
                if (int.TryParse(Simulator_Comms.GetRPMData(), out int parsedRPM))
                    CurrentRPM = parsedRPM;

                if (CurrentRPM == DefRPMValue) rpmLever = RPM.Neutral;
                else if (CurrentRPM > DefRPMValue) rpmLever = RPM.Decreasing;
                else if (CurrentRPM < DefRPMValue) rpmLever = RPM.Increasing;
            }
            return CurrentRPM;
        }

        public int CtrMainTransMissionLever()
        {
            int CurrentTransmissionInput = DefMainTransmissionValue;
            if (Simulator_Comms != null && Simulator_Comms.state == State.Active)
            {
                if (int.TryParse(Simulator_Comms.GetMainTransmissonInput(), out int parsedTrans))
                    CurrentTransmissionInput = parsedTrans;
            }

            if (CurrentTransmissionInput <= DefTransmissionInputMax && CurrentTransmissionInput >= DefTransmissionInputMin)
                mainTransmissionLever = MainTransmissionLever.Neutral;
            else if (CurrentTransmissionInput > DefTransmissionInputMax)
                mainTransmissionLever = MainTransmissionLever.Decreaseing;
            else if (CurrentTransmissionInput < DefTransmissionInputMin)
                mainTransmissionLever = MainTransmissionLever.Increaseing;

            return CurrentTransmissionInput;
        }
        public void GetSteeringUDLRValue()
        {
            string SteeringInput = Simulator_Comms.GetSteeringUDLRInput();
            if( SteeringInput != null)
            {
                if( SteeringInput.Length > 0 /*&& steering_UpDown == SteeringUpDown.Neutral*/)
                {
                    if (SteeringInput == "1000")
                    {
                        steering_UpDownLeftRight = SteeringUpDownLeftRight.Up;
                    }
                    else if (SteeringInput == "0100")
                    {
                        steering_UpDownLeftRight = SteeringUpDownLeftRight.Down;
                    }
                    else if (SteeringInput == "0010")
                    {
                        steering_UpDownLeftRight = SteeringUpDownLeftRight.Left;
                    }
                    else if (SteeringInput == "0001")
                    {
                        steering_UpDownLeftRight = SteeringUpDownLeftRight.Right;
                    }
                    else if (SteeringInput == "0000")
                    {
                        steering_UpDownLeftRight = SteeringUpDownLeftRight.Neutral;
                    }
                }
            }
        }
        public void GetReelControlWheelUDValue()
        {
            string SteeringInput = Simulator_Comms.GetReelUDInput();
            if (SteeringInput != null)
            {
                if (SteeringInput.Length > 0)
                {
                    if (SteeringInput == "10")
                    {
                        reelControlLever = ReelControlLever.Up;
                    }
                    else if (SteeringInput == "01")
                    {
                        reelControlLever = ReelControlLever.Down;
                    }
                    else if (SteeringInput == "00")
                    {
                        reelControlLever = ReelControlLever.Neutral;
                    }
                }
            }
        }
        public void GetThrashingValue()
        {
            string TharashingInput = Simulator_Comms.GetThrashingInput();
            {
                if(TharashingInput != null)
                {
                    if(TharashingInput.Length > 0)
                    {
                        if(TharashingInput == "1")
                        {
                            thrashing_Lever = Thrashing_Lever.Off;
                        }
                        else if(TharashingInput == "0") 
                        {
                            thrashing_Lever = Thrashing_Lever.On;
                        }
                    }
                }
            }
        }
        public void GetCuttingValue()
        {
            string CuttingInput = Simulator_Comms.GetCuttingInput();
            {
                if (CuttingInput != null)
                {
                    if (CuttingInput.Length > 0)
                    {
                        if (CuttingInput == "0")
                        {
                            cutting_Lever = Cutting_Lever.Off;
                        }
                        else if (CuttingInput == "1")
                        {
                            cutting_Lever = Cutting_Lever.On;
                        }
                    }
                }
            }
        }
        public void GetUnloadPipeValue()
        {
            string UnloadPipeUDValue = Simulator_Comms.GetUnloadPipeSwitchUD();
            { 
                if(UnloadPipeUDValue != null)
                {
                    if(UnloadPipeUDValue.Length > 0)
                    {
                        if (UnloadPipeUDValue == "10")
                        {
                            unloadingCylinderLever = Unloading_Cylinder_Lever.Down;
                        }
                        else if (UnloadPipeUDValue == "01")
                        {
                            unloadingCylinderLever = Unloading_Cylinder_Lever.Up;
                        }
                        else if (UnloadPipeUDValue == "00")
                        {
                            unloadingCylinderLever = Unloading_Cylinder_Lever.Neutral;
                        }
                    }
                }
            }
        }
        public void GetUnloadControllSwitchValue()
        {
            string UnloadControlSwitch = Simulator_Comms.GetUnloadedPipeSwitchLR();
            {
                if (UnloadControlSwitch != null)
                {
                    if (UnloadControlSwitch.Length > 0)
                    {
                        if (UnloadControlSwitch == "10")
                        {
                            unloadingPipeControlLever = Unloading_Pipe_Control_Switch.Left;
                        }
                        else if (UnloadControlSwitch == "01")
                        {
                            unloadingPipeControlLever = Unloading_Pipe_Control_Switch.Right;
                        }
                        else if (UnloadControlSwitch == "00")
                        {
                            unloadingPipeControlLever = Unloading_Pipe_Control_Switch.Neutral;
                        }
                    }
                }
            }
        }
        public void GetSubTransmissionLeverSwitch()
        {
            string TransmissionLeverSwitch = Simulator_Comms.GetSubTransmissionInputs();
            if(TransmissionLeverSwitch != null)
            {
                if(TransmissionLeverSwitch.Length > 0)
                {
                    if(TransmissionLeverSwitch == "001")
                    {
                        subTransmissionLever = SubTransmissionLever.Harvesting;
                    }
                    else if (TransmissionLeverSwitch == "010" || TransmissionLeverSwitch == "100")
                    {
                        subTransmissionLever = SubTransmissionLever.Neutral;
                    }
                    else if (TransmissionLeverSwitch == "110")
                    {
                        subTransmissionLever = SubTransmissionLever.Travel;
                    }
                    else if (TransmissionLeverSwitch == "000")
                    {
                        subTransmissionLever = SubTransmissionLever.Loading;
                    }
                }
            }
        }
        //new input
        public void GetIgnitionSwitchInput()
        {
            string TransmissionLeverSwitch = Simulator_Comms.GetIgnitionSwitchInput();
            {
                if (TransmissionLeverSwitch != null)
                {
                    if (TransmissionLeverSwitch.Length > 0)
                    {
                        if (TransmissionLeverSwitch == "0")
                        {
                            startSwitch = IgnitionSwitch.Neutral;
                        }
                        else if (TransmissionLeverSwitch == "1")
                        {
                            startSwitch = IgnitionSwitch.Started;
                        }
                    }
                }
            }
        }
        public void GetStopCableInput()
        {
            string StopCableInput = Simulator_Comms.GetStopCableInput();
            {
                if (StopCableInput != null)
                {
                    if (StopCableInput.Length > 0)
                    {
                        if (StopCableInput == "0")
                        {
                            stopCable = Stop_Cable.Off;
                        }
                        else if (StopCableInput == "1")
                        {
                            stopCable = Stop_Cable.On;
                        }
                    }
                }
            }
        }
        public void GetReverseFeedleChainInput()
        {
            string ReverseFeedleChainSwitch = Simulator_Comms.GetReverseFeedleChainInput();
            {
                if (ReverseFeedleChainSwitch != null)
                {
                    if (ReverseFeedleChainSwitch.Length > 0)
                    {
                        if (ReverseFeedleChainSwitch == "0")
                        {
                            reverse_Feeder_Chain_Lever = Reverse_Feeder_Chain_Lever.Forward;
                        }
                        else if (ReverseFeedleChainSwitch == "1")
                        {
                            reverse_Feeder_Chain_Lever = Reverse_Feeder_Chain_Lever.Backward;
                        }
                    }
                }
            }
        }
        public void GrainMainClutchInput()
        {
            string GrainMainClutchSwitch = Simulator_Comms.GrainMainClutchInput();
            {
                if (GrainMainClutchSwitch != null)
                {
                    if (GrainMainClutchSwitch.Length > 0)
                    {
                        if (GrainMainClutchSwitch == "0")
                        {
                            grainLever = Grain_Main_Clutch_Lever.Off;
                        }
                        else if (GrainMainClutchSwitch == "1")
                        {
                            grainLever = Grain_Main_Clutch_Lever.On;
                        }
                    }
                }
            }
        }
        public void GetClutchPaddle()
        {
            string ClutchPaddleSwitch = Simulator_Comms.ClutchPaddleInput();
            {
                if (ClutchPaddleSwitch != null)
                {
                    if (ClutchPaddleSwitch.Length > 0)
                    {
                        if (ClutchPaddleSwitch == "0")
                        {
                            clutch_Paddle = Clutch_Paddle.Up;
                        }
                        else if (ClutchPaddleSwitch == "1")
                        {
                            clutch_Paddle = Clutch_Paddle.Down;
                        }
                    }
                }
            }
        }
        public void GetNeutralSwitch()
        {
            string NeutralSwitch = Simulator_Comms.NeutralInput();
            {
                if (NeutralSwitch != null)
                {
                    if (NeutralSwitch.Length > 0)
                    {
                        if (NeutralSwitch == "0")
                        {
                            neutralSwitch = Neutral_Switch.Off;
                        }
                        else if (NeutralSwitch == "1")
                        {
                            neutralSwitch = Neutral_Switch.On;
                        }
                    }
                }
            }
        }

        private void OnGUI()
        {
            if (!enableKeyboardInput || !showKeyboardHUD) return;

            if (UnityEngine.Input.GetKeyDown(toggleHUDKey))
            {
                isHUDCollapsed = !isHUDCollapsed;
            }

            int panelWidth = 320;
            int margin = 12;
            float xPos = margin;
            float yPos = margin;

            if (isHUDCollapsed)
            {
                float btnWidth = 145;
                float btnHeight = 28;
                float btnX = margin;

                GUI.backgroundColor = new Color(0.08f, 0.1f, 0.14f, 0.9f);
                if (GUI.Button(new Rect(btnX, yPos, btnWidth, btnHeight), "⌨ Controls [F1]"))
                {
                    isHUDCollapsed = false;
                }
                return;
            }

            int panelHeight = 465;
            GUI.backgroundColor = new Color(0.05f, 0.07f, 0.11f, 0.94f);
            GUI.Box(new Rect(xPos, yPos, panelWidth, panelHeight), GUIContent.none);

            GUIStyle titleStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft
            };
            titleStyle.normal.textColor = new Color(1f, 0.85f, 0.25f);

            GUIStyle sectionHeaderStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 11,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft
            };
            sectionHeaderStyle.normal.textColor = new Color(0.35f, 0.82f, 1f);

            GUIStyle keyStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 11,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft
            };
            keyStyle.normal.textColor = new Color(1f, 0.95f, 0.65f);

            GUIStyle descStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 11,
                fontStyle = FontStyle.Normal,
                alignment = TextAnchor.MiddleLeft
            };
            descStyle.normal.textColor = new Color(0.92f, 0.94f, 0.97f);

            GUIStyle miniBtnStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 11,
                alignment = TextAnchor.MiddleCenter
            };

            // Title bar
            GUI.Label(new Rect(xPos + 10, yPos + 6, panelWidth - 45, 20), "⌨ KEYBOARD CONTROLS", titleStyle);
            if (GUI.Button(new Rect(xPos + panelWidth - 28, yPos + 6, 20, 18), "−", miniBtnStyle))
            {
                isHUDCollapsed = true;
            }

            float curY = yPos + 28;
            float rowHeight = 17;
            float keyColWidth = 115;
            float descColWidth = panelWidth - keyColWidth - 20;

            void DrawRow(string keys, string description)
            {
                GUI.Label(new Rect(xPos + 10, curY, keyColWidth, rowHeight), keys, keyStyle);
                GUI.Label(new Rect(xPos + 10 + keyColWidth, curY, descColWidth, rowHeight), description, descStyle);
                curY += rowHeight;
            }

            void DrawSection(string title)
            {
                curY += 2;
                GUI.Label(new Rect(xPos + 8, curY, panelWidth - 16, rowHeight), title, sectionHeaderStyle);
                curY += rowHeight;
            }

            DrawSection("ENGINE & SAFETY");
            DrawRow("[ 1 ] / [ Enter ]", "Start Engine");
            DrawRow("[ Esc ] / [ Bksp ]", "Stop Cable (Kill)");
            DrawRow("[ Z ] / [ F2 ]", "Reset ALL Levers to Neutral");
            DrawRow("[ N ]", "Neutral Switch");
            DrawRow("[ Space ] / [ Alt ]", "Clutch Pedal");

            DrawSection("TRANSMISSION & RPM");
            DrawRow("[ W ] / [ S ]", "Speed Lever (Fwd/Rev)");
            DrawRow("[ X ]", "Speed Neutral");
            DrawRow("[ 1 - 4 ] / [ G ]", "Gear (1:N, 2:H, 3:T, 4:L)");
            DrawRow("[ PgUp / PgDn ]", "RPM (+ / -)");

            DrawSection("STEERING & HEADER");
            DrawRow("[ A ] / [ D ]", "Steering (Left / Right)");
            DrawRow("[ I ] / [ K ]", "Cutter Height (Up / Down)");
            DrawRow("[ O ] / [ L ]", "Reel Height (Up / Down)");

            DrawSection("HARVESTING & AUGER");
            DrawRow("[ T ] / [ F ]", "Threshing Drum (Toggle)");
            DrawRow("[ C ] / [ E ]", "Cutter Blades (Toggle)");
            DrawRow("[ R ]", "Feeder Chain (Toggle)");
            DrawRow("[ U ] / [ N ]", "Auger Pipe (Up / Down)");
            DrawRow("[ H ] / [ J ]", "Auger Swivel (Left / Right)");
            DrawRow("[ Y ] / [ P ]", "Grain Discharge Clutch");
            DrawRow("[ V ]", "Horn & Lights");

            // Footer hint
            GUIStyle footerStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 9,
                fontStyle = FontStyle.Italic,
                alignment = TextAnchor.MiddleCenter
            };
            footerStyle.normal.textColor = new Color(0.7f, 0.75f, 0.8f);
            GUI.Label(new Rect(xPos + 5, yPos + panelHeight - 17, panelWidth - 10, 15), "Press [F1] to Minimize/Expand", footerStyle);
        }
    }
}
