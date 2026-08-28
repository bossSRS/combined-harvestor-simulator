using CHS.Input;
using CHS.Tutorial;
using FluffyUnderware.Curvy.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackHandler : MonoBehaviour
{
    public SplineController truckSpeed;
    public Transform TruckParent;
    public Transform CHSParent;
    public float TopSpeed;
    public bool isMoving;
    public Transform TruckWheel;
    public CheckTruck checkTruck;
    public CHS_Input chsInput;
    public GameObject Uploader;
    public TutorialManager tutorialManager;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            CHSParent.transform.parent = TruckParent;
            isMoving = true;
        }
        if (tutorialManager != null)
        {
            if ((truckSpeed.Position >= 0.5f && truckSpeed.Position < 0.6f) && !tutorialManager.isPoleWarningDone)
            {
                tutorialManager.LoadPoleWarning();
            }
            else if((truckSpeed.Position >= 0f && truckSpeed.Position < 1) && checkTruck.isInTruck)
            {
                checkTruck.FixPosition();
            }
        }
        if (truckSpeed.Position == 1)
        {
            Uploader.SetActive(true);
            isMoving = false;
            truckSpeed.Speed = 0;
            CHSParent.transform.parent = null;
            TruckWheel.gameObject.GetComponent<Animator>().SetFloat("speed", 0);
        }
        if (isMoving) 
        { 
            SetSpeed();
        }

        if(checkTruck.isInTruck && truckSpeed.Position == 0 && chsInput.CheckNetural(true)) 
        {
            Invoke("StartTruck", 2f);
        }
    }

    public void StartTruck()
    {
        print("Truck Started");
        Uploader.SetActive(false);
        CancelInvoke("StartTruck");
        CHSParent.transform.parent = TruckParent;
        isMoving = true;
    }
    private void SetSpeed()
    {
        if(truckSpeed.Position <= 0.25f)
        {
            if(truckSpeed.Speed < TopSpeed * 0.01f)
            {
                truckSpeed.Speed += (0.125f * 0.01f);
                
            }
        }
        else if (truckSpeed.Position >= 0.25f && truckSpeed.Position < 0.875f)
        {
            truckSpeed.Speed = TopSpeed * 0.01f;
        }
        else if(truckSpeed.Position >= 0.875f && truckSpeed.Position < 1f)
        {
            if (truckSpeed.Speed <= 0.1f)
            {
                truckSpeed.Speed -= (0.0025f * 0.01f);
            }
        }
        TruckWheel.gameObject.GetComponent<Animator>().SetFloat("speed", truckSpeed.Speed * 10);
    }
}
