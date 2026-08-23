using ChobiAssets.PTM;
using CHS.Controller;
using CHS.Input;
using CHS.Tutorial;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace CHS
{
    public enum HarvestorState {
        Off,
        Running,
        Harvesting,
        Unloading,
        GrainStuck
    }
    [System.Serializable]
    public class TransitionMode
    {
        public string Name;
        public float TransmissonOffset;
    }
    public class CHS_Controller : MonoBehaviour
    {
        public static CHS_Controller instance;
        [SerializeField] CHS_Input chs_Input;

        public GameObject HarvestingPosition;
        public GameObject FieldPosition;
        public GameObject UnloadingPosition;
        public HarvestorState harvestorState;
        public CHS_Parts_Controller CutterMainCtr;
        public CHS_Parts_Controller CutterOuterJntCtr;
        public AutoRotate ThrashingCtr;
        public AutoRotate CuttingCtr;
        public AutoMove BladeCtr;
        public GrainUnloaderController GrainUnloaderCtr;
        public Drive_Control_CS drive_Control_CS;

        public List<TransitionMode> TransitionModes;
        public int TransitionModeIndex;
        [Range(1f, 250f)]
        public float RPM_Accelaretion_offset;
        public static bool isStarted;
        public bool isTharashingStarted;

        public int TempMainTransmissionInput;
        public int TempRPMInput;

        [Header("Motion Parameters")]
        public float TempTopSpeed;
        public int TempRPM;
        public float TempAccleration;
        public float TempSpeedRate;


        [Header("UI")]
        public TMP_Text MaxSpeedTxt;
        public TMP_Text RPMTxt;
        public TMP_Text TransmissionTxt;
        public TMP_Text SpeedRateTxt;
        public TMP_Text HarvestingPercentange;
        public CHS_UI_Handler cHS_UI_Handler;

        [SerializeField]
        private List<GrainBehavior> grainAreas;

        public bool ischeckingForHarvestingCompletion;
        public bool isHarvestingComplete;
        public AudioSource BeepSound;
        public AudioSource EngineSound;
        public AudioSource GrainUnloadingSound;
        public TutorialManager tutorialManager;
        public int HarvestedCropCount;
        public float progress;
        // Start is called before the first frame update
        void Awake()
        {
            TempMainTransmissionInput = 150;
            TempRPMInput = 30;
            TempRPM = 500;
            TempTopSpeed = 1.6f;
            EngineSound.volume = 0f;

        }
        private void Start()
        {
            instance = this;
            if (tutorialManager == null)
            {
                tutorialManager = FindAnyObjectByType<TutorialManager>();
            }
            harvestorState = HarvestorState.Off;
            ResetOff();
        }
        [Button]
        private void InitGrainAreas()
        {
            var grains = FindObjectsByType<GrainAreaBehavior>(FindObjectsSortMode.None);
            for (int i = 0; i < grains.Length; i++)
            {
                for (int j = 0; j < grains[i].GrainZoneBehaviors.Count; j++)
                {
                    for (int k = 0; k < grains[i].GrainZoneBehaviors[j].grainBehaviors.Count; k++)
                    {
                        grainAreas.Add(grains[i].GrainZoneBehaviors[j].grainBehaviors[k]);
                    }
                }
            }
        }
        // Update is called once per frame
        void LateUpdate()
        {
            progress = ((float)HarvestedCropCount / (float)grainAreas.Count) * 100;
            HarvestingPercentange.text = "Harvested : " + progress.ToString("F2");
            if (chs_Input != null)
            {
                bool isSimulatorActive = (chs_Input.Simulator_Comms != null && chs_Input.Simulator_Comms.state == State.Active);
                bool isKeyboardActive = chs_Input.enableKeyboardInput;

                if (isSimulatorActive || isKeyboardActive)
                {
                    bool isIntroActive = (tutorialManager != null && tutorialManager.tutorialSteps == CHS.Tutorial.TutorialSteps.Dashboard_Introduction);

                    if (isIntroActive && harvestorState != HarvestorState.Off)
                    {
                        harvestorState = HarvestorState.Off;
                        ResetOff();
                    }

                    if (harvestorState != HarvestorState.Off)
                    {
                        if (chs_Input.thrashing_Lever == Thrashing_Lever.On && chs_Input.cutting_Lever == Cutting_Lever.On && chs_Input.subTransmissionLever == SubTransmissionLever.Harvesting && !ischeckingForHarvestingCompletion) 
                        {
                            ischeckingForHarvestingCompletion = true;
                            InvokeRepeating("CheckForHarvestingCompletion", 2f, 2f);
                        }

                        try
                        {
                            TempRPMInput = chs_Input.CtrRPMLever();
                        }
                        catch (Exception)
                        {
                            TempRPMInput = chs_Input.DefRPMValue;
                        }

                        try
                        {
                            TempMainTransmissionInput = chs_Input.CtrMainTransMissionLever();
                        }
                        catch (Exception)
                        {
                            TempMainTransmissionInput = chs_Input.DefMainTransmissionValue;
                        }
                        if (UnityEngine.Input.GetKey(KeyCode.I) || chs_Input.steering_UpDownLeftRight == SteeringUpDownLeftRight.Up)
                        {
                            CutterMainCtr.moveParts(1);
                        }
                        else if (UnityEngine.Input.GetKey(KeyCode.K) || chs_Input.steering_UpDownLeftRight == SteeringUpDownLeftRight.Down)
                        {
                            CutterMainCtr.moveParts(-1);
                        }
                        if (UnityEngine.Input.GetKey(KeyCode.O) || chs_Input.reelControlLever == ReelControlLever.Up)
                        {
                            CutterOuterJntCtr.moveParts(1);
                        }
                        else if (UnityEngine.Input.GetKey(KeyCode.L) || chs_Input.reelControlLever == ReelControlLever.Down)
                        {
                            CutterOuterJntCtr.moveParts(-1);
                        }
                        if (UnityEngine.Input.GetKey(KeyCode.U) || chs_Input.unloadingCylinderLever == Unloading_Cylinder_Lever.Up)
                        {
                            GrainUnloaderCtr.RotateUpper(-1);
                        }
                        else if (UnityEngine.Input.GetKey(KeyCode.N) || chs_Input.unloadingCylinderLever == Unloading_Cylinder_Lever.Down)
                        {
                            GrainUnloaderCtr.RotateUpper(1);
                        }
                        else if (UnityEngine.Input.GetKey(KeyCode.H) || chs_Input.unloadingPipeControlLever == Unloading_Pipe_Control_Switch.Left)
                        {
                            GrainUnloaderCtr.RotateBase(-1);
                        }
                        else if (UnityEngine.Input.GetKey(KeyCode.J) || chs_Input.unloadingPipeControlLever == Unloading_Pipe_Control_Switch.Right)
                        {
                            GrainUnloaderCtr.RotateBase(1);
                        }
                        if (UnityEngine.Input.GetKeyDown(KeyCode.E))
                        {
                            if (CuttingCtr.startHarvesting) CuttingCtr.startHarvesting = false;
                            else CuttingCtr.startHarvesting = true;
                        }
                        if (chs_Input.reverse_Feeder_Chain_Lever == Reverse_Feeder_Chain_Lever.Forward)
                        {
                            if (chs_Input.cutting_Lever == Cutting_Lever.On)
                            {
                                ThrashingCtr.startHarvesting = true;
                                CuttingCtr.startHarvesting = true;
                                BladeCtr.startHarvensting = true;
                            }
                            else if (chs_Input.cutting_Lever == Cutting_Lever.Off)
                            {
                                CuttingCtr.startHarvesting = false;
                                ThrashingCtr.startHarvesting = false;
                                BladeCtr.startHarvensting = false;
                            }
                        }
                        else if (chs_Input.reverse_Feeder_Chain_Lever == Reverse_Feeder_Chain_Lever.Backward)
                        {
                            if (chs_Input.cutting_Lever == Cutting_Lever.On)
                            {
                                ThrashingCtr.startHarvesting = true;
                                CuttingCtr.startHarvesting = true;
                                BladeCtr.startHarvensting = true;
                            }
                            else if (chs_Input.cutting_Lever == Cutting_Lever.Off)
                            {
                                CuttingCtr.startHarvesting = false;
                                ThrashingCtr.startHarvesting = false;
                                BladeCtr.startHarvensting = false;
                            }
                        }
                        if (UnityEngine.Input.GetKeyDown(KeyCode.F))
                        {
                            if (ThrashingCtr.startHarvesting) ThrashingCtr.startHarvesting = false;
                            else ThrashingCtr.startHarvesting = true;
                        }
                        if (chs_Input.thrashing_Lever == Thrashing_Lever.On)
                        {
                            isTharashingStarted = true;
                        }
                        else if (chs_Input.thrashing_Lever == Thrashing_Lever.Off)
                        {
                            isTharashingStarted = false;
                        }
                        
                        if(chs_Input.startSwitch == IgnitionSwitch.Started)
                        {
                            PlayBeepSound();
                            EngineSound.volume = 1;
                        }

                        MapRPMData();
                        MapMainTransmissionData();
                        MapSpeedRate();
                        GearShifting();

                        if (chs_Input.grainLever == Grain_Main_Clutch_Lever.On)
                        {
                            GrainUnloaderCtr.GrainUnloadingONOFF(true);
                        }
                        else if (chs_Input.grainLever == Grain_Main_Clutch_Lever.Off)
                        {
                            GrainUnloaderCtr.GrainUnloadingONOFF(false);
                        }
                        /// For Debugging
                        if (UnityEngine.Input.GetKeyUp(KeyCode.RightShift))
                        {
                            TransitionModeIndex++;
                            TransitionModeIndex = Mathf.Clamp(TransitionModeIndex, 1, 5);
                        }
                        else if (UnityEngine.Input.GetKeyUp(KeyCode.RightControl))
                        {
                            TransitionModeIndex--;
                            TransitionModeIndex = Mathf.Clamp(TransitionModeIndex, 1, 5);
                        }
                        if (UnityEngine.Input.GetKey(KeyCode.LeftShift))
                        {
                            var TempValue = RPM_Accelaretion_offset;
                            drive_Control_CS.UpdateRPM(drive_Control_CS.RPM + TempValue);
                            TempRPM = (int)drive_Control_CS.RPM;
                        }
                        else if (UnityEngine.Input.GetKey(KeyCode.LeftControl))
                        {
                            var TempValue = RPM_Accelaretion_offset;
                            drive_Control_CS.UpdateRPM(drive_Control_CS.RPM - TempValue);
                            TempRPM = (int)drive_Control_CS.RPM;
                        }
                        if(chs_Input.stopCable == Stop_Cable.On)
                        {
                            harvestorState = HarvestorState.Off;
                            ResetOff();
                        }
                    }
                    else
                    {
                        if (!isIntroActive && chs_Input.startSwitch == IgnitionSwitch.Started && (chs_Input.CheckNetural() || (isKeyboardActive && !isSimulatorActive)))
                        {
                            harvestorState = HarvestorState.Running;
                        }
                        GearShifting();
                    }
                }
            }
        }
        public void PlayBeepSound()
        {
            if (!BeepSound.isPlaying)
            {
                BeepSound.Play();
            }
        }
        public void PlayGrainUnloadingSound(bool value)
        {
            if (value)
            {
                if (!GrainUnloadingSound.isPlaying)
                {
                    GrainUnloadingSound.Play();
                }
            }
            else
            {
                GrainUnloadingSound.Stop();
            }
        }
        public void ResetOff()
        {
            if (chs_Input != null)
            {
                chs_Input.ResetAllToNeutral();
            }
            if (ThrashingCtr != null) ThrashingCtr.startHarvesting = false;
            if (CuttingCtr != null) CuttingCtr.startHarvesting = false;
            if (BladeCtr != null) BladeCtr.startHarvensting = false;
            if (EngineSound != null) EngineSound.volume = 0;
            if (GrainUnloaderCtr != null) GrainUnloaderCtr.GrainUnloadingONOFF(false);
            isTharashingStarted = false;
        }
        public void GearShifting()
        {
            if (chs_Input.subTransmissionLever == SubTransmissionLever.Harvesting)
            {
                TransitionModeIndex = 0;
                TransmissionTxt.text = "Gear: Harvesting";
            }
            else if (chs_Input.subTransmissionLever == SubTransmissionLever.Neutral)
            {
                TransitionModeIndex = 1;
                TransmissionTxt.text = "Gear: Neutral";
            }
            else if (chs_Input.subTransmissionLever == SubTransmissionLever.Travel)
            {
                TransitionModeIndex = 2;
                TransmissionTxt.text = "Gear: Travel";
            }
            else if (chs_Input.subTransmissionLever == SubTransmissionLever.Loading)
            {
                TransitionModeIndex = 3;
                TransmissionTxt.text = "Gear: Loading";
            }
        }
        public void MapSpeedRate()
        {
            TempSpeedRate = drive_Control_CS.Speed_Rate;
        }
        private void MapRPMData()
        {
            if (drive_Control_CS == null) return;
            TempRPM = MapRange(TempRPMInput, chs_Input.MinRPMValue, chs_Input.MaxRPMValue, (int)drive_Control_CS.MinRPM, (int)drive_Control_CS.MaxRPM);
            if (harvestorState != HarvestorState.Off && TempRPM < 800)
            {
                TempRPM = 800;
            }
            TempTopSpeed = MapRange(TempRPMInput, chs_Input.MinRPMValue, chs_Input.MaxRPMValue, drive_Control_CS.Min_MaxSpeed, drive_Control_CS.Max_MaxSpeed);

            RPMTxt.text = "RPM:" + TempRPM;
            drive_Control_CS.UpdateRPM(TempRPM);

            drive_Control_CS.UpdateMaxSpeed(TempTopSpeed * TransitionModes[TransitionModeIndex].TransmissonOffset);
            MaxSpeedTxt.text = "Max Speed:" + (TempTopSpeed * TransitionModes[TransitionModeIndex].TransmissonOffset).ToString("0.00") + " KPH";

            SpeedRateTxt.text = "Speed: " + (TempSpeedRate * TempTopSpeed).ToString("0.00") + " KPH";

            float RPM_UI_Data = MapRange(TempRPM, drive_Control_CS.MaxRPM, drive_Control_CS.MinRPM, -100f, 100f);
            cHS_UI_Handler.UpdateValue("RPM", RPM_UI_Data);
        }

        private void MapMainTransmissionData()
        {
            if (TempMainTransmissionInput > chs_Input.DefTransmissionInputMin && TempMainTransmissionInput < chs_Input.DefTransmissionInputMax)
            {
                TempAccleration = 0;
            }
            else
            {
                TempAccleration = MapRange((float)TempMainTransmissionInput, (float)chs_Input.TransmissionInputMax, (float)chs_Input.TransmissionInputMin, -1f, 1f);
            }
        }
        public static int MapRange(int value, int fromMin, int fromMax, int toMin, int toMax)
        {
            // First, normalize the value from the original range to a [0, 1] range
            float normalizedValue = (float)(value - fromMin) / (fromMax - fromMin);

            // Then map the normalized value to the target range
            int result = (int)(toMin + normalizedValue * (toMax - toMin));

            // Ensure the result is within the target range
            if (result < toMin)
            {
                return toMin;
            }
            else if (result > toMax)
            {
                return toMax;
            }
            else
            {
                return result;
            }
        }
        public static float MapRange(float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            // First, normalize the value from the original range to a [0, 1] range
            float normalizedValue = (value - fromMin) / (fromMax - fromMin);

            // Then map the normalized value to the target range
            float result = (toMin + normalizedValue * (toMax - toMin));

            // Ensure the result is within the target range
            if (result < toMin)
            {
                return toMin;
            }
            else if (result > toMax)
            {
                return toMax;
            }
            else
            {
                return result;
            }
        }
        public void CheckForHarvestingCompletion()
        {
            if (progress > 97f)
                isHarvestingComplete = true;
            else
                isHarvestingComplete = false;
        }
    }
}
