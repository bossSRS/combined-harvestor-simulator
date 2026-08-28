using CHS.Input;
using CHS.Tutorial;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CHS
{
    public enum CameraPosition
    {
        TravelPosition = 1,
        HarvestingPostion = 2,
        LoadingPosition = 3
    }
    public class CHS_CameraController : MonoBehaviour
    {
        [SerializeField]
        private CameraPosition _cameraState;
        [SerializeField]
        List<Transform> CameraTransforms;
        [SerializeField]
        public CHS_Input cHS_Input;

        public UnityEvent ShiftCamera;
        public GameObject CameraObj;
        [HideInInspector]
        public TutorialManager tutorialManager;

        public Material[] ShadeMaterial;
        public MeshRenderer ShadeRenderer;
        // Start is called before the first frame update

        private void Start()
        {
            tutorialManager = FindAnyObjectByType<TutorialManager>();
            CameraObj.transform.localPosition = CameraTransforms[2].localPosition;
        }
        public void FixedUpdate()
        {
            if(tutorialManager != null) 
            {
                if (tutorialManager.tutorialSteps == TutorialSteps.Dashboard_Introduction)
                    return;
            }
            if (cHS_Input != null)
            {
                if (UnityEngine.Input.GetKeyDown(KeyCode.LeftShift))
                {
                    DebugCamera();
                }
                else if (isTutorialisInHarvesting())
                {
                    _cameraState = CameraPosition.HarvestingPostion;
                    ShiftCamera.Invoke();
                }
                else if(cHS_Input.subTransmissionLever == SubTransmissionLever.Travel && _cameraState != CameraPosition.TravelPosition)
                {
                    _cameraState = CameraPosition.TravelPosition;
                    ShiftCamera.Invoke();
                }
                else if (cHS_Input.subTransmissionLever == SubTransmissionLever.Loading && _cameraState != CameraPosition.LoadingPosition)
                {
                    _cameraState = CameraPosition.LoadingPosition;
                    ShiftCamera.Invoke();
                }
                else if (cHS_Input.subTransmissionLever == SubTransmissionLever.Harvesting && _cameraState != CameraPosition.HarvestingPostion)
                {
                    _cameraState = CameraPosition.HarvestingPostion;
                    ShiftCamera.Invoke();
                }
            }
        }
        public void OnShiftCamera()
        {
            int index = (int)_cameraState;
            CameraObj.transform.DOLocalMove(CameraTransforms[index].localPosition,1f);
            CameraObj.transform.DOLocalRotate(CameraTransforms[index].localEulerAngles,1f);
        }
        public void DebugCamera()
        {
            CameraObj.transform.localPosition = CameraTransforms[2].localPosition;
            CameraObj.transform.localEulerAngles = Vector3.zero;
        }
        public void ChangeShadeState()
        {
            if(_cameraState == CameraPosition.HarvestingPostion)
            {
                Material m = new Material(ShadeMaterial[0]);
                ShadeRenderer.material = m;
            }
            else
            {
                Material m = new Material(ShadeMaterial[1]);
                ShadeRenderer.material = m;
            }
        }
        
        public bool isTutorialisInHarvesting()
        {
            if(tutorialManager != null)
            {
                if (tutorialManager.tutorialSteps == TutorialSteps.Harvesting) return true;
            }
            return false;
        }
    }
}