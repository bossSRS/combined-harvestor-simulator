using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.VFX;

public class GrainUnloaderController : MonoBehaviour
{
    [SerializeField]
    private Transform UpperPipe;
    [SerializeField]
    private Transform BasePipe;

    [SerializeField]
    public float UpperPipeSpeed;
    [SerializeField]
    private float BasePipeSpeed;
    public bool isPlaced;
    public bool isUnlocked;
    public bool isPlaceOnPosition;

    [ShowInInspector,ReadOnly]
    private float UpperPipeMax;
    [ShowInInspector, ReadOnly] 
    private float BasePipeMax;
    [ShowInInspector, ReadOnly] 
    private float BasePipeMin;
    [ShowInInspector, ReadOnly] 
    private float UpperPipeMin;

    public VisualEffect GrainUnloadingVFX;
    public VisualEffect ByProductReleasingVFX;

    public CHS_Grain_Collection_Controller grain_Collection_Controller;
    // Start is called before the first frame update
    void Start()
    {
        //isPlaced = true;
        UpperPipeMax = 32;
        BasePipeMax = 359;
        UpperPipeMin = 2;
        BasePipeMin = 0;
    }

    public void RotateUpper(int direction)
    {
        UpperPipe.transform.localEulerAngles += new Vector3(direction * UpperPipeSpeed * Time.deltaTime, 0, 0);
    }
    public void RotateBase(int direction)
    {
        //if (isPlaced) return;
        print("Rotatiing Base");
        BasePipe.transform.localEulerAngles += new Vector3(0, direction * BasePipeSpeed * Time.deltaTime, 0);
    }

    private void LateUpdate()
    {
        UpperPipe.transform.localEulerAngles = new Vector3(Mathf.Clamp(UpperPipe.transform.localEulerAngles.x,UpperPipeMin,UpperPipeMax), UpperPipe.transform.localEulerAngles.y, UpperPipe.transform.localEulerAngles.z);
        BasePipe.transform.localEulerAngles = new Vector3(BasePipe.transform.localEulerAngles.x, Mathf.Clamp(BasePipe.transform.localEulerAngles.y, BasePipeMin, BasePipeMax), BasePipe.transform.localEulerAngles.z);
        if (isUnlocked)
        {
            grain_Collection_Controller.GrainTankActive(-1);
        }
        CheckforInPlace();
    }


    public void CheckforInPlace()
    {
        if (BasePipe.transform.localEulerAngles.y > 74 && BasePipe.transform.localEulerAngles.y < 76)
        {
            if (UpperPipe.transform.localEulerAngles.x > 29 && UpperPipe.transform.localEulerAngles.x < 33)
            {
                isPlaced = true;
            }
            else
                isPlaced = false;
        }
        else
            isPlaced = false;
    }
    public void GrainUnloadingONOFF(bool FXSwitch)
    {
        if (grain_Collection_Controller.GetGrainStack() > 0 && FXSwitch)
        {
            GrainUnloadingVFX.enabled = true;
            isUnlocked = true;
            GrainUnloadingVFX.SendEvent("OnPlay");
            grain_Collection_Controller.chs_Controller.PlayGrainUnloadingSound(true);
        }
        else if (grain_Collection_Controller.GetGrainStack() <= 0 || !FXSwitch)
        {
            isUnlocked = false;
            GrainUnloadingVFX.SendEvent("OnStop");
            grain_Collection_Controller.chs_Controller.PlayGrainUnloadingSound(false);
            GrainUnloadingVFX.enabled = false;
        }
    }
    public void ByProductReleasingONOFF(bool FXSwitch)
    {
        if (FXSwitch)
        {
            isUnlocked = true;
            ByProductReleasingVFX.SendEvent("OnPlay");
        }
        else
        {
            isUnlocked = false;
            ByProductReleasingVFX.SendEvent("OnStop");
        }
    }
}
