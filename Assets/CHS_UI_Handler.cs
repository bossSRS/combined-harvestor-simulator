using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CHS_UI_Handler : MonoBehaviour
{
    public Image RMP_Meter;
    public Image Fuel_Meter;
    public Image Temperature_Meter;

    public AnimationCurve RPM_Meter_C;
    public AnimationCurve Fuel_Meter_C;
    public AnimationCurve Temperature_Meter_C;

    int RPM_Constant = 100;
    int Fuel_Constant = 45;
    int Temperature_Constant = 45;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateValue(string ValueType, float Value)
    {
        if (ValueType == "RPM") 
        {
            float tempValue = Mathf.Clamp(Value, RPM_Constant * RPM_Meter_C.keys[0].value, RPM_Constant * RPM_Meter_C.keys[1].value);
            RMP_Meter.transform.eulerAngles = new Vector3(0, 0, tempValue);
        }
        else if (ValueType == "Temperature") 
        {
            float tempValue = Mathf.Clamp(Value, Temperature_Constant * Fuel_Meter_C.keys[0].value, Fuel_Constant * Fuel_Meter_C.keys[1].value);
            Temperature_Meter.transform.eulerAngles = new Vector3(0, 0, tempValue);
        }
        else if (ValueType == "Fuel")
        {
            float tempValue = Mathf.Clamp(Value, Fuel_Constant * Fuel_Meter_C.keys[0].value, Fuel_Constant * Fuel_Meter_C.keys[1].value);
            Fuel_Meter.transform.eulerAngles = new Vector3(0, 0, tempValue);
        }
    }
}
