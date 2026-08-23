using CHS.Input;
using CHS.Tutorial;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ResetPosition
{
    Zero = 0,
    One = 1,
    Two = 2,
    Three = 3
}
public class Harvester_Reset_Controller : MonoBehaviour
{
    public List<Transform> ResetPositions;
    public Transform Harvestar;
    public bool isReseting;
    public CHS_Input chs_Input;
    public ResetPosition resetPosition;
    private void LateUpdate()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            OnPressedReset();
        }
    }
    public void ResetHarvestar(int index)
    {
        if (Harvestar != null)
        {
            isReseting = true;
            Harvestar.transform.position = ResetPositions[index].position;
            Harvestar.transform.DORotate(ResetPositions[index].localEulerAngles, 5f).OnComplete(() =>
            {
                isReseting = false;
            });
        }
    }

    public void OnPressedReset()
    {
        if (!isReseting)
        {
            if (chs_Input != null)
            {
                ResetHarvestar((int)resetPosition);
            }
        }
    }
}
