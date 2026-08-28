using CHS;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CHS_Grain_Collection_Controller : MonoBehaviour
{
    [Range(0f,100f)]
    [ShowInInspector]
    private float _grainStack;
    public float GrainStack => _grainStack;
    [SerializeField]
    private float GrainFlowOffset;
    public bool isGrainTankFull;

    public Image GrainBarUI;
    public CHS_Controller chs_Controller;
    // Start is called before the first frame update

    private void Awake()
    {
        chs_Controller = GetComponent<CHS_Controller>();
    }
    public float GetGrainStack()
    {
        return _grainStack;
    }
    public void GrainTankActive(int GrainFlow)
    {
        if(_grainStack <= 100f)
        {
            if(GrainFlow  == -1)
            {
                _grainStack += (GrainFlow * GrainFlowOffset * 2 * 0.02f);
            }
            else if (GrainFlow == 1)
            {
                _grainStack += (GrainFlow * GrainFlowOffset * 0.02f);
            }
            GrainBarUI.fillAmount = _grainStack * 0.01f;
            GrainBarUI.fillAmount = Mathf.Clamp01(GrainBarUI.fillAmount);
        }
        else if(_grainStack > 100)
        {
            _grainStack = 100;
            isGrainTankFull = true;
            chs_Controller.PlayBeepSound();

        }
        else if(_grainStack <= 0)
        {
            _grainStack = 0;
        }
    }
    private void FixedUpdate()
    {
        if(_grainStack < 100)
        {
            isGrainTankFull = false;
        }
    }
}
