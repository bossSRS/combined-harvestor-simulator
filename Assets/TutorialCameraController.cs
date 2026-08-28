using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHS.Tutorial
{
    public class TutorialCameraController : MonoBehaviour
    {
        [SerializeField]
        List<Transform> CamTransforms;
        public Camera mainCam;
        public Transform DefCamPosition;
        // Start is called before the first frame update
        void Awake()
        {
            mainCam = Camera.main;
        }
        public void SwitchCamera(int index)
        {
            mainCam.transform.DOMove(CamTransforms[index].position, 1f);
            mainCam.transform.DORotate(CamTransforms[index].eulerAngles, 1f);
        }

        public void SwitchToDefault()
        {
            mainCam.transform.DOMove(DefCamPosition.position, 1f);
            mainCam.transform.DORotate(DefCamPosition.eulerAngles, 1f);
        }
    }
}
