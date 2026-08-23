using CHS.Input;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using ChobiAssets.PTM;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;
namespace CHS.Tutorial 
{
    public enum TutorialSteps
    {
        Dashboard_Introduction = 1,
        Igniton_Start = 2,
        SteepRoad_Up = 3,
        GettingIntoTruck = 4,
        Stopping = 5,
        GettingDownFromTruck = 6,
        TravelingTowardsField = 7,
        GettingIntoField = 8,
        Harvesting = 9,
        GrainUnloading = 10,
        CompleteTutorial = 11,
    }
    public enum DashboardSteps
    {
        IgnitionSwitch = 1,
        RPMLever = 2,
        SubTransmissionLever = 3,
        MainTransmissionLever = 4,
        SterringLever = 5,
        ReelControlLever = 6,
        ThrashingLever = 7,
        CuttingLever = 8,
        UnloadingCylinderLever = 9,
        UnloadingPipeControllSwitch = 10,
        GrainMainClutchLever = 11,
        ReverseFeederChainLever = 12,
        StopCable = 13,
        NeutralSwitch = 14,
        ClutchPaddle = 15
    }
    public class TutorialManager : MonoBehaviour
    {
        [Header("MainSteps")]
        public TutorialSteps tutorialSteps;
        public int TutorialIndex = 1;
        public CHS_Simulator_Comms Sim_input;
        public CHS_Input chs_Input;
        public CHS_Controller chs_Controller;
        public Drive_Control_CS drive_Control_CS;
        public TrackHandler trackHandler;
        public CHS_Grain_Collection_Controller chs_Grain_Collection_Controller;
        public GrainUnloaderController grainUnloaderController;

        public List<GameObject> TutorialUI_Objs;
        public GameObject PoleWarning;
        public GameObject HitPeopleWarning;
        public GameObject ZoneWarning;
        public GameObject RoadWarning;
        public GameObject HarvestingIndicator;
        public GameObject BigAileWarning;
        public GameObject WaterWarning;
        public GameObject Traffic;

        public bool isPoleWarningDone;
        public bool isHitWarningDone;
        public bool isZoneWarningDone;
        public bool isRoadWarningDone;
        public bool isBigAileWarningDone;
        public bool isWaterWarningDone;
        public bool isInZone;

        [Header("Dashboard Introduction")]
        public DashboardSteps dashboardSteps;
        public DashboardTutorialBehaviour [] dashBoardTutorialBehaviours;
        public List<string> DashboardPartNames;
        public int DashboardTutorialIndex = 1;
        public TutorialCameraController tutorialCameraController;
        public TMP_Text DashboardPartsTxt;
        public TMP_Text DashboardKeyHintTxt;
        [Header("UI Keyboard Prompts")]
        public bool showKeyboardPromptOnScreen = true;

        [Header("Staring")]
        public List<TutorialBehavior> IgnitionStart;
        [Header("GettingOntruck")]
        public List<TutorialBehavior> StepRoadUp;
        public List<TutorialBehavior> GetOnTruck;
        public List<TutorialBehavior> Stopping;
        [Header("GettingToTheField")]
        public List<TutorialBehavior> GettingDownFromTruck;
        public List<TutorialBehavior> TravellingTowardsField;
        public List<TutorialBehavior> GettingDownIntoFields;
        [Header("Harvesting")]
        public List<TutorialBehavior> Harvesting;
        [Header("Unloading")]
        public List<TutorialBehavior> Unloading;
        [Header("ReturnToShade")]
        public List<TutorialBehavior> ReturnToShade;

        [Header("MarkerZones")]
        public GameObject TruckZone;
        public GameObject FieldZone;
        public GameObject HarvestZone;
        public GameObject GrainTargetZone;
        public GameObject PipePositionZone;
        public GameObject LevelEnd;

        public GameObject AileObj;
        public GameObject WorkerObj;
        public AudioSource clueAudio;
        public Harvester_Reset_Controller harvester_Reseter;

        public AudioClip HighlightedPartsInstruction;
        public List<AudioClip> HighlightedPartsName;

        public AudioSource TutAudioSource;
        // Start is called before the first frame update
        IEnumerator Start()
        {
            if (chs_Controller != null)
            {
                chs_Controller.harvestorState = CHS.HarvestorState.Off;
                chs_Controller.ResetOff();
            }
            tutorialSteps = TutorialSteps.Dashboard_Introduction;
            TutorialUI_Objs[0].SetActive(true);
            yield return new WaitForSeconds(3);
            DashBoardIntroductionStart();
        }
        public void DashBoardIntroductionStart()
        {
            dashboardSteps = DashboardSteps.IgnitionSwitch;
            DashboardTutorialIndex = 1;
            UpdateDashboardIntroductionUI();
            dashBoardTutorialBehaviours[DashboardTutorialIndex].PlayIndicator();
            tutorialCameraController.SwitchCamera(1);
            DOVirtual.DelayedCall(1.5f, () =>
            {
                TutAudioSource.PlayOneShot(HighlightedPartsInstruction);
                DOVirtual.DelayedCall(3f, () =>
                {
                    TutAudioSource.Stop();
                    TutAudioSource.clip = HighlightedPartsName[1];
                    TutAudioSource.loop = true;
                    TutAudioSource.Play();
                });
            });
        }
        public void CheckForShiftingTutorialUpdate() 
        {
            switch (tutorialSteps)
            {
                case TutorialSteps.Igniton_Start:
                    if (CheckCurrentTutorialSteps(IgnitionStart)) ShiftTutorialUpdate();
                    break;
                case TutorialSteps.SteepRoad_Up:
                    if(CheckCurrentTutorialSteps(StepRoadUp)) ShiftTutorialUpdate();
                    break;
                case TutorialSteps.GettingIntoTruck:
                    if (CheckCurrentTutorialSteps(GetOnTruck)) ShiftTutorialUpdate();
                    break;
                case TutorialSteps.Stopping:
                    if(CheckCurrentTutorialSteps(Stopping)) ShiftTutorialUpdate();
                    break;
                case TutorialSteps.GettingDownFromTruck:
                    if(CheckCurrentTutorialSteps(GettingDownFromTruck)) ShiftTutorialUpdate();
                    break;
                case TutorialSteps.TravelingTowardsField:
                    if(CheckCurrentTutorialSteps(TravellingTowardsField)) ShiftTutorialUpdate();
                    break;
                case TutorialSteps.GettingIntoField:
                    if(CheckCurrentTutorialSteps(GettingDownIntoFields)) ShiftTutorialUpdate();
                    break;
                case TutorialSteps.Harvesting:
                    if(CheckCurrentTutorialSteps(Harvesting)) ShiftTutorialUpdate();
                    break;
                case TutorialSteps.GrainUnloading:
                    if(CheckCurrentTutorialSteps(Unloading)) ShiftTutorialUpdate();
                    break;
                case TutorialSteps.CompleteTutorial:
                    break;

            }
        }
        public bool CheckCurrentTutorialSteps(List<TutorialBehavior> tbs)
        {
            foreach(TutorialBehavior tb in tbs)
            {
                if(!tb.isCompleted) return false;
            }
            return true;
        } 
        public void ShiftNext(bool shiftExtra = false)
        {
            if (tutorialSteps == TutorialSteps.Dashboard_Introduction)
            {
                if (dashboardSteps == DashboardSteps.IgnitionSwitch && chs_Input != null)
                {
                    chs_Input.startSwitch = CHS.Input.IgnitionSwitch.Neutral;
                    if (chs_Controller != null)
                    {
                        chs_Controller.harvestorState = CHS.HarvestorState.Off;
                        chs_Controller.ResetOff();
                    }
                }
                if (dashboardSteps == DashboardSteps.MainTransmissionLever && chs_Input != null)
                {
                    chs_Input.DefMainTransmissionValue = 155;
                    chs_Input.mainTransmissionLever = CHS.Input.MainTransmissionLever.Neutral;
                }

                dashBoardTutorialBehaviours[DashboardTutorialIndex].StopIndicator();
                DashboardTutorialIndex++;
                if (DashboardTutorialIndex < dashBoardTutorialBehaviours.Length)
                {
                    UpdateDashboardIntroductionUI();
                    dashBoardTutorialBehaviours[DashboardTutorialIndex].PlayIndicator();
                    dashboardSteps = (DashboardSteps)DashboardTutorialIndex;

                    TutAudioSource.Stop();
                    TutAudioSource.clip = HighlightedPartsName[DashboardTutorialIndex];
                    TutAudioSource.Play();
                }
                else if (DashboardTutorialIndex >= dashBoardTutorialBehaviours.Length)
                {
                    if (chs_Input != null)
                    {
                        chs_Input.ResetAllToNeutral();
                    }
                    if (chs_Controller != null)
                    {
                        chs_Controller.harvestorState = CHS.HarvestorState.Off;
                        chs_Controller.ResetOff();
                    }
                    ShiftTutorialUpdate();
                    TutAudioSource.Stop();
                    IgnitionStart[0].PlayAudio.Invoke();
                }
            }
        }

        public void UpdateDashboardIntroductionUI()
        {
            string partName = (DashboardPartNames != null && DashboardTutorialIndex < DashboardPartNames.Count) 
                ? DashboardPartNames[DashboardTutorialIndex] 
                : ((DashboardSteps)DashboardTutorialIndex).ToString();

            DashboardSteps currentStep = (DashboardSteps)DashboardTutorialIndex;
            string keyHint = GetDashboardKeyboardHint(currentStep);

            if (DashboardKeyHintTxt != null)
            {
                DashboardKeyHintTxt.text = "<b>Keyboard:</b> " + keyHint;
                if (DashboardPartsTxt != null)
                {
                    DashboardPartsTxt.text = partName;
                }
            }
            else if (DashboardPartsTxt != null)
            {
                if (!string.IsNullOrEmpty(keyHint))
                {
                    DashboardPartsTxt.text = $"{partName}\n<size=75%><color=#FFD54F><b>[Key: {keyHint}]</b></color></size>";
                }
                else
                {
                    DashboardPartsTxt.text = partName;
                }
            }
        }

        public string GetDashboardKeyboardHint(DashboardSteps step)
        {
            switch (step)
            {
                case DashboardSteps.IgnitionSwitch:
                    return "Press [ 1 ] or [ Enter ]";
                case DashboardSteps.RPMLever:
                    return "Press [ PageUp / Shift ] (+RPM) or [ PageDown / Ctrl ] (-RPM)";
                case DashboardSteps.SubTransmissionLever:
                    return "Press [ 2 ] Harvesting / [ 3 ] Travel / [ G ] Cycle";
                case DashboardSteps.MainTransmissionLever:
                    return "Press [ W ] / [ UpArrow ] (Forward) or [ S ] / [ DownArrow ] (Backward)";
                case DashboardSteps.SterringLever:
                    return "Press [ I / K ] Header Up/Down, [ A / D ] Steer Left/Right";
                case DashboardSteps.ReelControlLever:
                    return "Press [ O ] Reel Up, [ L ] Reel Down";
                case DashboardSteps.ThrashingLever:
                    return "Press [ T ] or [ F ] to Toggle Thrashing";
                case DashboardSteps.CuttingLever:
                    return "Press [ C ] or [ E ] to Toggle Cutting";
                case DashboardSteps.UnloadingCylinderLever:
                    return "Press [ U ] Auger Up, [ N / M ] Auger Down";
                case DashboardSteps.UnloadingPipeControllSwitch:
                    return "Press [ H ] Swivel Left, [ J ] Swivel Right";
                case DashboardSteps.GrainMainClutchLever:
                    return "Press [ Y ] or [ P ] to Toggle Grain Discharge";
                case DashboardSteps.ReverseFeederChainLever:
                    return "Press [ R ] to Toggle Feeder Direction";
                case DashboardSteps.StopCable:
                    return "Press [ Escape ] or [ Backspace ] (Engine Stop)";
                case DashboardSteps.NeutralSwitch:
                    return "Press [ N ] to Toggle Neutral Safety Switch";
                case DashboardSteps.ClutchPaddle:
                    return "Press [ Space ] or [ LeftAlt ] (Clutch Pedal)";
                default:
                    return "";
            }
        }

        private void OnGUI()
        {
            if (showKeyboardPromptOnScreen && tutorialSteps == TutorialSteps.Dashboard_Introduction)
            {
                string partName = (DashboardPartNames != null && DashboardTutorialIndex < DashboardPartNames.Count)
                    ? DashboardPartNames[DashboardTutorialIndex]
                    : ((DashboardSteps)DashboardTutorialIndex).ToString();

                string keyHint = GetDashboardKeyboardHint((DashboardSteps)DashboardTutorialIndex);

                float width = 560;
                float height = 75;
                float x = (Screen.width - width) / 2;
                float y = Screen.height - height - 35;

                GUI.backgroundColor = new Color(0.08f, 0.1f, 0.14f, 0.92f);
                GUI.Box(new Rect(x, y, width, height), GUIContent.none);

                GUIStyle headerStyle = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 16,
                    fontStyle = FontStyle.Bold
                };
                headerStyle.normal.textColor = new Color(1f, 0.88f, 0.35f);

                GUIStyle hintStyle = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 15,
                    fontStyle = FontStyle.Normal
                };
                hintStyle.normal.textColor = Color.white;

                GUI.Label(new Rect(x, y + 8, width, 26), $"Control: {partName} (Step {DashboardTutorialIndex}/15)", headerStyle);
                GUI.Label(new Rect(x, y + 36, width, 28), $"Keyboard Input: {keyHint}", hintStyle);
            }
        }
        public void ShiftTutorialUpdate()
        {
            TutorialUI_Objs[TutorialIndex].SetActive(false);
            TutorialIndex++;
            tutorialSteps = (TutorialSteps)TutorialIndex;
            TutorialUI_Objs[TutorialIndex].SetActive(true);

        }
        private void FixedUpdate()
        {
            CheckForInput();
            if (tutorialSteps == TutorialSteps.Igniton_Start)
            {
                if (chs_Input.CheckNetural("true") && !IgnitionStart[0].isCompleted)
                {
                    IgnitionStart[0].TutorialComplete.Invoke();
                    IgnitionStart[1].PlayAudio.Invoke();
                }
                else if (IgnitionStart[0].isCompleted && chs_Input.neutralSwitch == CHS.Input.Neutral_Switch.On && !IgnitionStart[1].isCompleted)
                {
                    IgnitionStart[1].TutorialComplete.Invoke();
                    IgnitionStart[2].PlayAudio.Invoke();
                }
                else if (IgnitionStart[0].isCompleted && IgnitionStart[1].isCompleted && chs_Input.neutralSwitch == CHS.Input.Neutral_Switch.On && chs_Input.startSwitch == CHS.Input.IgnitionSwitch.Started)
                {
                    IgnitionStart[2].TutorialComplete.Invoke();
                    if (chs_Controller != null)
                    {
                        chs_Controller.harvestorState = CHS.HarvestorState.Running;
                    }
                    DOVirtual.DelayedCall(3f, () => { 
                        CheckForShiftingTutorialUpdate();
                        StepRoadUp[0].PlayAudio.Invoke();
                    });
                }
            }
            else if (tutorialSteps == TutorialSteps.SteepRoad_Up)
            {
                if (drive_Control_CS.RPM >= 2175 && drive_Control_CS.RPM < 2225 && !StepRoadUp[0].isCompleted)
                {
                    StepRoadUp[0].TutorialComplete.Invoke();
                    StepRoadUp[1].PlayAudio.Invoke();
                }
                else if (StepRoadUp[0].isCompleted && !StepRoadUp[1].isCompleted && chs_Controller.CutterMainCtr.transform.localEulerAngles.x < 7.5f)
                {
                    StepRoadUp[1].TutorialComplete.Invoke();
                    StepRoadUp[2].PlayAudio.Invoke();
                }
                else if (StepRoadUp[0].isCompleted && StepRoadUp[1].isCompleted && !StepRoadUp[2].isCompleted && chs_Input.subTransmissionLever == SubTransmissionLever.Loading)
                {
                    StepRoadUp[2].TutorialComplete.Invoke();
                    StepRoadUp[3].PlayAudio.Invoke();
                }
                else if (StepRoadUp[0].isCompleted && StepRoadUp[1].isCompleted && StepRoadUp[2].isCompleted && !StepRoadUp[3].isCompleted && 
                    ((int.TryParse(Sim_input.GetMainTransmissonInput(), out int transInputVal) && transInputVal < 150)
                     || chs_Input.mainTransmissionLever == MainTransmissionLever.Increaseing 
                     || UnityEngine.Input.GetKey(KeyCode.W) 
                     || UnityEngine.Input.GetKey(KeyCode.UpArrow)))
                {
                    StepRoadUp[3].TutorialComplete.Invoke();
                    TruckZone.SetActive(true);
                }
            }
            else if (tutorialSteps == TutorialSteps.GettingIntoTruck)
            {
                harvester_Reseter.resetPosition = ResetPosition.One;
                if (trackHandler.checkTruck.isInTruck && !GetOnTruck[0].isCompleted)
                {
                    GetOnTruck[0].TutorialComplete.Invoke();
                    GetOnTruck[1].PlayAudio.Invoke();
                    TruckZone.SetActive(false);
                }
                else if(GetOnTruck[0].isCompleted && chs_Controller.CutterMainCtr.transform.localEulerAngles.x > 15f && !GetOnTruck[1].isCompleted)
                {
                    GetOnTruck[1].TutorialComplete.Invoke();
                    DOVirtual.DelayedCall(3f, () => {
                        CheckForShiftingTutorialUpdate();
                        Stopping[0].PlayAudio.Invoke();
                    }, true);
                }
            }
            else if (tutorialSteps == TutorialSteps.Stopping)
            {
                if (chs_Input.CheckNetural("true") && !Stopping[0].isCompleted)
                {
                    Stopping[0].TutorialComplete.Invoke();
                    Stopping[1].PlayAudio.Invoke();
                }
                else if (Stopping[0].isCompleted  && !Stopping[1].isCompleted && chs_Input.stopCable == Stop_Cable.On)
                {
                    Stopping[1].TutorialComplete.Invoke();
                    Stopping[2].PlayAudio.Invoke();
                    Traffic.SetActive(true);
                }
                else if (Stopping[0].isCompleted && Stopping[1].isCompleted && !Stopping[2].isCompleted && trackHandler.truckSpeed.Position > 0.9f && chs_Input.startSwitch == IgnitionSwitch.Started)
                {
                    Stopping[2].TutorialComplete.Invoke();
                    DOVirtual.DelayedCall(5f, () => { 
                        CheckForShiftingTutorialUpdate();
                        GettingDownFromTruck[0].PlayAudio.Invoke();
                        
                    }, true);
                }
            }
            else if (tutorialSteps == TutorialSteps.GettingDownFromTruck)
            {
                if (drive_Control_CS.RPM >= 2175 && drive_Control_CS.RPM < 2225 && !GettingDownFromTruck[0].isCompleted)
                {
                    GettingDownFromTruck[0].TutorialComplete.Invoke();
                    GettingDownFromTruck[1].PlayAudio.Invoke();
                }
                else if (GettingDownFromTruck[0].isCompleted && !GettingDownFromTruck[1].isCompleted && chs_Controller.CutterMainCtr.transform.localEulerAngles.x < 7.5f)
                {
                    GettingDownFromTruck[1].TutorialComplete.Invoke();
                    GettingDownFromTruck[2].PlayAudio.Invoke();
                }
                else if (GettingDownFromTruck[0].isCompleted && GettingDownFromTruck[1].isCompleted && !GettingDownFromTruck[2].isCompleted && chs_Input.subTransmissionLever == SubTransmissionLever.Loading)
                {
                    GettingDownFromTruck[2].TutorialComplete.Invoke();
                    GettingDownFromTruck[3].PlayAudio.Invoke();
                }
                else if (GettingDownFromTruck[0].isCompleted && GettingDownFromTruck[1].isCompleted && GettingDownFromTruck[2].isCompleted && !GettingDownFromTruck[3].isCompleted && 
                    ((int.TryParse(Sim_input.GetMainTransmissonInput(), out int transInputVal) && transInputVal > 156)
                     || chs_Input.mainTransmissionLever == MainTransmissionLever.Decreaseing 
                     || UnityEngine.Input.GetKey(KeyCode.S) 
                     || UnityEngine.Input.GetKey(KeyCode.DownArrow)))
                {
                    GettingDownFromTruck[3].TutorialComplete.Invoke();
                    DOVirtual.DelayedCall(3f, () => 
                    { 
                        CheckForShiftingTutorialUpdate();
                        TravellingTowardsField[0].PlayAudio.Invoke();
                    }, true);
                }
            }
            else if (tutorialSteps == TutorialSteps.TravelingTowardsField)
            {
                harvester_Reseter.resetPosition = ResetPosition.Two;
                if (drive_Control_CS.RPM >= 2175 && drive_Control_CS.RPM < 2225 && !TravellingTowardsField[0].isCompleted)
                {
                    TravellingTowardsField[0].TutorialComplete.Invoke();
                    TravellingTowardsField[1].PlayAudio.Invoke();
                }
                else if (chs_Controller.CutterMainCtr.transform.localEulerAngles.x > 15f && TravellingTowardsField[0].isCompleted && !TravellingTowardsField[1].isCompleted)
                {
                    TravellingTowardsField[1].TutorialComplete.Invoke();
                    TravellingTowardsField[2].PlayAudio.Invoke();
                }
                else if (TravellingTowardsField[0].isCompleted && TravellingTowardsField[1].isCompleted && !TravellingTowardsField[2].isCompleted && chs_Input.subTransmissionLever == SubTransmissionLever.Travel)
                {
                    TravellingTowardsField[2].TutorialComplete.Invoke();
                    TravellingTowardsField[3].PlayAudio.Invoke();
                    FieldZone.SetActive(true);
                }
                else if (TravellingTowardsField[0].isCompleted && TravellingTowardsField[1].isCompleted && TravellingTowardsField[2].isCompleted  && Vector3.Distance(chs_Controller.drive_Control_CS.gameObject.transform.position, chs_Controller.HarvestingPosition.transform.position) < 2f && !TravellingTowardsField[3].isCompleted)
                {
                    TravellingTowardsField[3].TutorialComplete.Invoke();
                    TravellingTowardsField[4].PlayAudio.Invoke();
                    FieldZone.SetActive(false);
                }
                else if (TravellingTowardsField[0].isCompleted && TravellingTowardsField[1].isCompleted && TravellingTowardsField[2].isCompleted && TravellingTowardsField[3].isCompleted && !TravellingTowardsField[4].isCompleted && (chs_Controller.drive_Control_CS.transform.localEulerAngles.y > 265f && chs_Controller.drive_Control_CS.transform.localEulerAngles.y < 275f))
                {
                    TravellingTowardsField[4].TutorialComplete.Invoke();
                    DOVirtual.DelayedCall(3f, () => { 
                        CheckForShiftingTutorialUpdate();
                        GettingDownIntoFields[0].PlayAudio.Invoke();
                    }, true);
                }
            }
            else if (tutorialSteps == TutorialSteps.GettingIntoField)
            {
                harvester_Reseter.resetPosition = ResetPosition.Three;
                if (drive_Control_CS.RPM >= 1975 && drive_Control_CS.RPM < 2025 && !GettingDownIntoFields[0].isCompleted)
                {
                    GettingDownIntoFields[0].TutorialComplete.Invoke();
                    GettingDownIntoFields[1].PlayAudio.Invoke();
                }
                else if (GettingDownIntoFields[0].isCompleted && chs_Controller.CutterMainCtr.transform.localEulerAngles.x < 7.5f && !GettingDownIntoFields[1].isCompleted) 
                {
                    GettingDownIntoFields[1].TutorialComplete.Invoke();
                    GettingDownIntoFields[2].PlayAudio.Invoke();
                }
                else if (GettingDownIntoFields[0].isCompleted && chs_Input.subTransmissionLever == SubTransmissionLever.Loading && GettingDownIntoFields[1].isCompleted  && !GettingDownIntoFields[2].isCompleted)
                {
                    GettingDownIntoFields[2].TutorialComplete.Invoke();
                    GettingDownIntoFields[3].PlayAudio.Invoke();
                }
                else if (GettingDownIntoFields[0].isCompleted && GettingDownIntoFields[1].isCompleted && GettingDownIntoFields[2].isCompleted && !GettingDownIntoFields[3].isCompleted && 
                    ((int.TryParse(Sim_input.GetMainTransmissonInput(), out int transInputVal) && (transInputVal > 156 || transInputVal < 150))
                     || chs_Input.mainTransmissionLever != MainTransmissionLever.Neutral 
                     || UnityEngine.Input.GetKey(KeyCode.W) 
                     || UnityEngine.Input.GetKey(KeyCode.S) 
                     || UnityEngine.Input.GetKey(KeyCode.UpArrow) 
                     || UnityEngine.Input.GetKey(KeyCode.DownArrow)))
                {
                    GettingDownIntoFields[3].TutorialComplete.Invoke();
                    GettingDownIntoFields[4].PlayAudio.Invoke();
                    HarvestZone.SetActive(true);
                }
                else if (GettingDownIntoFields[0].isCompleted && GettingDownIntoFields[1].isCompleted && GettingDownIntoFields[2].isCompleted && GettingDownIntoFields[3].isCompleted && !GettingDownIntoFields[4].isCompleted && Vector3.Distance(chs_Controller.drive_Control_CS.gameObject.transform.position, chs_Controller.FieldPosition.transform.position) < 2f)
                {
                    GettingDownIntoFields[4].TutorialComplete.Invoke();
                    GettingDownIntoFields[5].PlayAudio.Invoke();
                    HarvestZone.SetActive(false);
                }
                else if (GettingDownIntoFields[0].isCompleted && GettingDownIntoFields[1].isCompleted && GettingDownIntoFields[2].isCompleted && GettingDownIntoFields[3].isCompleted && GettingDownIntoFields[4].isCompleted && !GettingDownIntoFields[5].isCompleted && (chs_Controller.drive_Control_CS.transform.localEulerAngles.y > 85f && chs_Controller.drive_Control_CS.transform.localEulerAngles.y < 95f))
                {
                    GettingDownIntoFields[5].TutorialComplete.Invoke();
                    DOVirtual.DelayedCall(3f, () =>
                    {
                        CheckForShiftingTutorialUpdate();
                        Harvesting[0].PlayAudio.Invoke();
                    }, true);
                }
            }
            else if (tutorialSteps == TutorialSteps.Harvesting)
            {
                if (chs_Input.clutch_Paddle == Clutch_Paddle.Down && chs_Input.subTransmissionLever == SubTransmissionLever.Loading && !Harvesting[0].isCompleted)
                {
                    Harvesting[0].TutorialComplete.Invoke();
                    Harvesting[1].PlayAudio.Invoke();
                }
                else if (Harvesting[0].isCompleted && drive_Control_CS.RPM <= 1000 && !Harvesting[1].isCompleted)
                {
                    Harvesting[1].TutorialComplete.Invoke();
                    Harvesting[2].PlayAudio.Invoke();
                }
                else if (Harvesting[0].isCompleted && Harvesting[1].isCompleted && chs_Input.thrashing_Lever == Thrashing_Lever.On && !Harvesting[2].isCompleted)
                {
                    Harvesting[2].TutorialComplete.Invoke();
                    Harvesting[3].PlayAudio.Invoke();
                }
                else if (Harvesting[0].isCompleted && Harvesting[1].isCompleted && Harvesting[2].isCompleted && chs_Input.cutting_Lever == Cutting_Lever.On && !Harvesting[3].isCompleted)
                {
                    Harvesting[3].TutorialComplete.Invoke();
                    Harvesting[4].PlayAudio.Invoke();
                }
                else if (Harvesting[0].isCompleted && Harvesting[1].isCompleted && Harvesting[2].isCompleted && Harvesting[3].isCompleted && drive_Control_CS.RPM >= 2500 && drive_Control_CS.RPM < 2700 && !Harvesting[4].isCompleted)
                {
                    Harvesting[4].TutorialComplete.Invoke();
                    Harvesting[5].PlayAudio.Invoke();
                }
                else if (Harvesting[0].isCompleted && Harvesting[1].isCompleted && Harvesting[2].isCompleted && Harvesting[3].isCompleted && Harvesting[4].isCompleted && chs_Controller.CutterMainCtr.transform.localEulerAngles.x > 27.5f && !Harvesting[5].isCompleted)
                {
                    Harvesting[5].TutorialComplete.Invoke();
                    Harvesting[6].PlayAudio.Invoke();
                }
                else if (Harvesting[0].isCompleted && Harvesting[1].isCompleted && Harvesting[2].isCompleted && Harvesting[3].isCompleted && Harvesting[4].isCompleted && Harvesting[5].isCompleted && chs_Controller.CutterOuterJntCtr.transform.localEulerAngles.x < 27.5f && !Harvesting[6].isCompleted)
                {
                    Harvesting[6].TutorialComplete.Invoke();
                    Harvesting[7].PlayAudio.Invoke();
                }
                else if (Harvesting[0].isCompleted && Harvesting[1].isCompleted && Harvesting[2].isCompleted && Harvesting[3].isCompleted && Harvesting[4].isCompleted && Harvesting[5].isCompleted && Harvesting[6].isCompleted && chs_Input.clutch_Paddle == Clutch_Paddle.Down && chs_Input.subTransmissionLever == SubTransmissionLever.Harvesting && !Harvesting[7].isCompleted)
                {
                    Harvesting[7].TutorialComplete.Invoke();
                    Harvesting[8].PlayAudio.Invoke();
                }
                else if (Harvesting[0].isCompleted && Harvesting[1].isCompleted && Harvesting[2].isCompleted && Harvesting[3].isCompleted && Harvesting[4].isCompleted && Harvesting[5].isCompleted && Harvesting[6].isCompleted && Harvesting[7].isCompleted && chs_Grain_Collection_Controller.isGrainTankFull && !Harvesting[8].isCompleted)
                {
                    Harvesting[8].TutorialComplete.Invoke();
                    DOVirtual.DelayedCall(3f, () => {
                        CheckForShiftingTutorialUpdate();
                        Unloading[0].PlayAudio.Invoke();
                    });
                    GrainTargetZone.SetActive(true);
                }
            }
            else if (tutorialSteps == TutorialSteps.GrainUnloading)
            {
                if (chs_Input.thrashing_Lever == Thrashing_Lever.Off && !Unloading[0].isCompleted)
                {
                    Unloading[0].TutorialComplete.Invoke();
                    Unloading[1].PlayAudio.Invoke();
                }
                else if (Unloading[0].isCompleted && chs_Input.cutting_Lever == Cutting_Lever.Off && !Unloading[1].isCompleted)
                {
                    Unloading[1].TutorialComplete.Invoke();
                    Unloading[2].PlayAudio.Invoke();
                }
                else if (Unloading[0].isCompleted && Unloading[1].isCompleted && !Unloading[2].isCompleted)
                {
                    if (Vector3.Distance(chs_Controller.drive_Control_CS.gameObject.transform.position, chs_Controller.UnloadingPosition.transform.position) < 2f)
                    {
                        Unloading[2].TutorialComplete.Invoke();
                        Unloading[3].PlayAudio.Invoke();
                        GrainTargetZone.SetActive(false);
                        PipePositionZone.SetActive(true);
                    }
                }
                else if (Unloading[0].isCompleted && Unloading[1].isCompleted && Unloading[2].isCompleted && !Unloading[3].isCompleted && (drive_Control_CS.RPM >= 1450 && drive_Control_CS.RPM < 1550))
                {
                    Unloading[3].TutorialComplete.Invoke();
                    Unloading[4].PlayAudio.Invoke();
                }
                else if (Unloading[0].isCompleted && Unloading[1].isCompleted && Unloading[2].isCompleted && Unloading[3].isCompleted && !Unloading[4].isCompleted  && chs_Input.clutch_Paddle == Clutch_Paddle.Down && chs_Input.subTransmissionLever == SubTransmissionLever.Loading)
                {
                    Unloading[4].TutorialComplete.Invoke();
                    Unloading[5].PlayAudio.Invoke();
                }
                else if (Unloading[0].isCompleted && Unloading[1].isCompleted && Unloading[2].isCompleted && Unloading[3].isCompleted && Unloading[4].isCompleted && !Unloading[5].isCompleted && grainUnloaderController.isPlaceOnPosition)
                {
                    Unloading[5].TutorialComplete.Invoke();
                    Unloading[6].PlayAudio.Invoke();
                    PipePositionZone.SetActive(false);
                }
                else if (Unloading[0].isCompleted && Unloading[1].isCompleted && Unloading[2].isCompleted && Unloading[3].isCompleted && Unloading[4].isCompleted && Unloading[5].isCompleted && !Unloading[6].isCompleted && chs_Input.grainLever == Grain_Main_Clutch_Lever.On)
                {
                    Unloading[6].TutorialComplete.Invoke();
                    Unloading[7].PlayAudio.Invoke();
                }
                else if (Unloading[0].isCompleted && Unloading[1].isCompleted && Unloading[2].isCompleted && Unloading[3].isCompleted && Unloading[4].isCompleted && Unloading[5].isCompleted && Unloading[6].isCompleted && !Unloading[7].isCompleted && (chs_Grain_Collection_Controller.GrainStack <= 0f && chs_Input.grainLever == Grain_Main_Clutch_Lever.Off))
                {
                    Unloading[7].TutorialComplete.Invoke();
                    Unloading[8].PlayAudio.Invoke();
                }
                else if (Unloading[0].isCompleted && Unloading[1].isCompleted && Unloading[2].isCompleted && Unloading[3].isCompleted && Unloading[4].isCompleted && Unloading[5].isCompleted && Unloading[6].isCompleted && Unloading[7].isCompleted && !Unloading[8].isCompleted && grainUnloaderController.isPlaced)
                {
                    Unloading[8].TutorialComplete.Invoke();
                    DOVirtual.DelayedCall(3f, () => 
                    {
                        CheckForShiftingTutorialUpdate();
                        ReturnToShade[0].PlayAudio.Invoke();
                    });
                }
            }
            else if (tutorialSteps == TutorialSteps.CompleteTutorial)
            {
                if (chs_Controller.isHarvestingComplete && chs_Grain_Collection_Controller.GrainStack <= 0f && !ReturnToShade[0].isCompleted )
                {
                    ReturnToShade[0].TutorialComplete.Invoke();
                    DOVirtual.DelayedCall(3f, () => {
                        UnityEngine.Cursor.lockState = CursorLockMode.None;
                        UnityEngine.Cursor.visible = true;
                        TutorialEnd();
                    });
                }
            }
            // if (LevelEnd.transform.localScale == Vector3.one && UnityEngine.Input.GetKey(KeyCode.E))
            // {
            //     LoadMainMenu();
            // }
        }
        public void TutorialEnd()
        {
            LevelEnd.transform.localScale = Vector3.zero;
            LevelEnd.SetActive(true);
            LevelEnd.transform.DOScale(Vector3.one, 1f);
        }
        public void LoadMainMenu()
        {
            UnityEngine.Cursor.visible = true;
            SceneManager.LoadScene("Menu");
        }
        public void LoadPoleWarning()
        {
            Time.timeScale = 0f;
            PoleWarning.SetActive(true);
            DOVirtual.DelayedCall(5f, () =>
            {
                Time.timeScale = 1f;
                PoleWarning.SetActive(false);
                isPoleWarningDone = true;
            }, true);
        }
        public void HittingPeopleWarning()
        {
            Time.timeScale = 0f;
            HitPeopleWarning.SetActive(true);
            DOVirtual.DelayedCall(5f, () =>
            {
                Time.timeScale = 1f;
                HitPeopleWarning.SetActive(false);
                isHitWarningDone = true;
            }, true);
        }
        public void ZoneWarningMessage()
        {
            Time.timeScale = 0f;
            ZoneWarning.SetActive(true);
            DOVirtual.DelayedCall(5f, () =>
            {
                Time.timeScale = 1f;
                ZoneWarning.SetActive(false);
                isZoneWarningDone = true;
            }, true);
        }
        public void RoadWarningMessage()
        {
            Time.timeScale = 0f;
            RoadWarning.SetActive(true);
            DOVirtual.DelayedCall(5f, () =>
            {
                Time.timeScale = 1f;
                RoadWarning.SetActive(false);
                isRoadWarningDone = true;
                trackHandler.TruckParent.gameObject.SetActive(true);
                CheckForShiftingTutorialUpdate();
                GetOnTruck[0].PlayAudio.Invoke();
            }, true);
        }
        public void BigAileWarningMessage()
        {
            Time.timeScale = 0f;
            BigAileWarning.SetActive(true);
            isBigAileWarningDone = true;
            DOVirtual.DelayedCall(5f, () =>
            {
                Time.timeScale = 1f;
                WorkerObj.SetActive(true);
                BigAileWarning.SetActive(false);
                DOVirtual.DelayedCall(5f, () =>
                {
                    AileObj.SetActive(false);
                    WorkerObj.SetActive(false);
                }, true);
            }, true);
        }
        public void WaterWarningMessage()
        {
            Time.timeScale = 0f;
            WaterWarning.SetActive(true);
            DOVirtual.DelayedCall(5f, () =>
            {
                Time.timeScale = 1f;
                WaterWarning.SetActive(false);
                isWaterWarningDone = true;
            }, true);
        }
        public void CheckForInput()
        {
            if (tutorialSteps == TutorialSteps.Dashboard_Introduction)
            {
                switch (dashboardSteps)
                {
                    case DashboardSteps.IgnitionSwitch:
                        if (Sim_input.GetIgnitionSwitchInput() == "1" 
                            || (chs_Input != null && chs_Input.startSwitch == CHS.Input.IgnitionSwitch.Started)
                            || UnityEngine.Input.GetKeyDown(KeyCode.Alpha1) 
                            || UnityEngine.Input.GetKeyDown(KeyCode.Return))
                        {
                            tutorialCameraController.SwitchCamera(3);
                            ShiftNext();
                        }
                        break;
                    case DashboardSteps.RPMLever:
                        if ((int.TryParse(Sim_input.GetRPMData(), out int rpmVal) && rpmVal < 200)
                            || (chs_Input != null && chs_Input.rpmLever != CHS.Input.RPM.Neutral)
                            || UnityEngine.Input.GetKey(KeyCode.PageUp)
                            || UnityEngine.Input.GetKey(KeyCode.PageDown)
                            || UnityEngine.Input.GetKey(KeyCode.LeftShift)
                            || UnityEngine.Input.GetKey(KeyCode.LeftControl))
                        {
                            ShiftNext();
                        }
                        break;
                    case DashboardSteps.SubTransmissionLever:
                        if (Sim_input.GetSubTransmissionInputs() == "001" 
                            || (chs_Input != null && chs_Input.subTransmissionLever != CHS.Input.SubTransmissionLever.Neutral)
                            || UnityEngine.Input.GetKeyDown(KeyCode.Alpha2)
                            || UnityEngine.Input.GetKeyDown(KeyCode.Alpha3)
                            || UnityEngine.Input.GetKeyDown(KeyCode.Alpha4)
                            || UnityEngine.Input.GetKeyDown(KeyCode.G))
                        {
                            ShiftNext();
                        }
                        break;
                    case DashboardSteps.MainTransmissionLever:
                        bool mainTransInput = (chs_Input != null && chs_Input.mainTransmissionLever != CHS.Input.MainTransmissionLever.Neutral)
                            || UnityEngine.Input.GetKey(KeyCode.W)
                            || UnityEngine.Input.GetKey(KeyCode.S)
                            || UnityEngine.Input.GetKey(KeyCode.UpArrow)
                            || UnityEngine.Input.GetKey(KeyCode.DownArrow)
                            || (int.TryParse(Sim_input.GetMainTransmissonInput(), out int transVal) && (transVal < 148 || transVal > 162));

                        if (mainTransInput)
                        {
                            tutorialCameraController.SwitchCamera(2);
                            ShiftNext();
                        }
                        break;
                    case DashboardSteps.SterringLever:
                        if (Sim_input.GetSteeringUDLRInput() != "0000"
                            || (chs_Input != null && chs_Input.steering_UpDownLeftRight != CHS.Input.SteeringUpDownLeftRight.Neutral)
                            || UnityEngine.Input.GetKey(KeyCode.I)
                            || UnityEngine.Input.GetKey(KeyCode.K)
                            || UnityEngine.Input.GetKey(KeyCode.A)
                            || UnityEngine.Input.GetKey(KeyCode.D))
                        {
                            ShiftNext();
                        }
                        break;
                    case DashboardSteps.ReelControlLever:
                        if (Sim_input.GetReelUDInput() != "00"
                            || (chs_Input != null && chs_Input.reelControlLever != CHS.Input.ReelControlLever.Neutral)
                            || UnityEngine.Input.GetKey(KeyCode.O)
                            || UnityEngine.Input.GetKey(KeyCode.L))
                        {
                            tutorialCameraController.SwitchCamera(3);
                            ShiftNext();
                        }
                        break;
                    case DashboardSteps.ThrashingLever:
                        if (Sim_input.GetThrashingInput() == "0"
                            || (chs_Input != null && chs_Input.thrashing_Lever == CHS.Input.Thrashing_Lever.On)
                            || UnityEngine.Input.GetKeyDown(KeyCode.T)
                            || UnityEngine.Input.GetKeyDown(KeyCode.F))
                        {
                            ShiftNext();
                        }
                        break;
                    case DashboardSteps.CuttingLever:
                        if (Sim_input.GetCuttingInput() == "1"
                            || (chs_Input != null && chs_Input.cutting_Lever == CHS.Input.Cutting_Lever.On)
                            || UnityEngine.Input.GetKeyDown(KeyCode.C)
                            || UnityEngine.Input.GetKeyDown(KeyCode.E))
                        {
                            tutorialCameraController.SwitchCamera(2);
                            ShiftNext();
                        }
                        break;
                    case DashboardSteps.UnloadingCylinderLever:
                        if (Sim_input.GetUnloadPipeSwitchUD() != "00"
                            || (chs_Input != null && chs_Input.unloadingCylinderLever != CHS.Input.Unloading_Cylinder_Lever.Neutral)
                            || UnityEngine.Input.GetKey(KeyCode.U)
                            || UnityEngine.Input.GetKey(KeyCode.N)
                            || UnityEngine.Input.GetKey(KeyCode.M))
                        {
                            ShiftNext();
                        }
                        break;
                    case DashboardSteps.UnloadingPipeControllSwitch:
                        if (Sim_input.GetUnloadedPipeSwitchLR() != "00"
                            || (chs_Input != null && chs_Input.unloadingPipeControlLever != CHS.Input.Unloading_Pipe_Control_Switch.Neutral)
                            || UnityEngine.Input.GetKey(KeyCode.H)
                            || UnityEngine.Input.GetKey(KeyCode.J))
                        {
                            tutorialCameraController.SwitchCamera(4);
                            ShiftNext();
                        }
                        break;
                    case DashboardSteps.GrainMainClutchLever:
                        if (Sim_input.GrainMainClutchInput() == "1"
                            || (chs_Input != null && chs_Input.grainLever == CHS.Input.Grain_Main_Clutch_Lever.On)
                            || UnityEngine.Input.GetKeyDown(KeyCode.Y)
                            || UnityEngine.Input.GetKeyDown(KeyCode.P))
                        {
                            tutorialCameraController.SwitchCamera(5);
                            ShiftNext(true);
                        }
                        break;
                    case DashboardSteps.ReverseFeederChainLever:
                        if (Sim_input.GetReverseFeedleChainInput() == "1"
                            || (chs_Input != null && chs_Input.reverse_Feeder_Chain_Lever == CHS.Input.Reverse_Feeder_Chain_Lever.Backward)
                            || UnityEngine.Input.GetKeyDown(KeyCode.R))
                        {
                            tutorialCameraController.SwitchCamera(2);
                            ShiftNext();
                        }
                        break;
                    case DashboardSteps.StopCable:
                        if (Sim_input.GetStopCableInput() == "1"
                            || (chs_Input != null && chs_Input.stopCable == CHS.Input.Stop_Cable.On)
                            || UnityEngine.Input.GetKeyDown(KeyCode.Escape)
                            || UnityEngine.Input.GetKeyDown(KeyCode.Backspace))
                        {
                            ShiftNext();
                        }
                        break;
                    case DashboardSteps.NeutralSwitch:
                        if (Sim_input.NeutralInput() == "1"
                            || (chs_Input != null && chs_Input.neutralSwitch == CHS.Input.Neutral_Switch.On)
                            || UnityEngine.Input.GetKeyDown(KeyCode.N))
                        {
                            tutorialCameraController.SwitchCamera(1);
                            ShiftNext();
                        }
                        break;
                    case DashboardSteps.ClutchPaddle:
                        if (Sim_input.ClutchPaddleInput() == "1"
                            || (chs_Input != null && chs_Input.clutch_Paddle == CHS.Input.Clutch_Paddle.Down)
                            || UnityEngine.Input.GetKeyDown(KeyCode.Space)
                            || UnityEngine.Input.GetKeyDown(KeyCode.LeftAlt))
                        {
                            tutorialCameraController.SwitchToDefault();
                            ShiftNext();
                        }
                        break;
                }
            }
        }  
    }
}