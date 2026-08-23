using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHS.Controller
{
    public class Lever_Controller : MonoBehaviour
    {
        public enum LeverType
        {
            Forward,
            Backward,
            Sideways,
            Bothways,
            Button,
            Key,
        }
        [SerializeField]
        private LeverType type;

        public bool forwardPress;
        public bool backwarPress;
        public bool LeftPress;
        public bool RightPress;
        public bool ButtonPress;

        public Vector3 TargetPosition;
        public Vector3 DefaultPosition;

        float MAXAngle = 25;
        float MinAngie = -25;
        public void LevelTransition()
        {
            switch (type)
            {
                case LeverType.Forward:
                    if (forwardPress)
                    {
                        transform.localEulerAngles += new Vector3(0, 0, 5f * Time.deltaTime);
                    }
                    break;
                case LeverType.Backward:
                    if (backwarPress)
                    {
                        transform.localEulerAngles -= new Vector3(0, 0, 5f * Time.deltaTime);
                    }
                    break;
                case LeverType.Sideways:
                    if (LeftPress)
                    {
                        transform.localEulerAngles += new Vector3(0, 5f * Time.deltaTime, 0);
                    }
                    else if (RightPress)
                    {
                        transform.localEulerAngles -= new Vector3(0, 5f * Time.deltaTime, 0);
                    }
                    else if (!RightPress && !LeftPress)
                    {
                        if(transform.localEulerAngles.y < 0)
                        {
                            transform.localEulerAngles += new Vector3(0, 5f * Time.deltaTime, 0);
                        }
                        else if (transform.localEulerAngles.y > 0)
                        {
                            transform.localEulerAngles -= new Vector3(0, 5f * Time.deltaTime, 0);
                        }
                    }
                    break;
                case LeverType.Bothways:
                    if (forwardPress)
                    {
                        transform.localEulerAngles += new Vector3(0, 0, 5f * Time.deltaTime);
                    }
                    else if (backwarPress)
                    {
                        transform.localEulerAngles -= new Vector3(0, 0, 5f * Time.deltaTime);
                    }
                    else if (!forwardPress && !backwarPress)
                    {
                        if (transform.localEulerAngles.z < 0)
                        {
                            transform.localEulerAngles += new Vector3(0, 0, 5f * Time.deltaTime);
                        }
                        else if (transform.localEulerAngles.z > 0)
                        {
                            transform.localEulerAngles -= new Vector3(0, 0, 5f * Time.deltaTime);
                        }
                    }
                    if (LeftPress)
                    {
                        transform.localEulerAngles -= new Vector3(0, 5f * Time.deltaTime, 0);
                    }
                    else if (RightPress)
                    {
                        transform.localEulerAngles += new Vector3(0, 5f * Time.deltaTime, 0);
                    }
                    else if (!RightPress && !LeftPress)
                    {
                        if (transform.localEulerAngles.y < 0)
                        {
                            transform.localEulerAngles += new Vector3(0, 5f * Time.deltaTime, 0);
                        }
                        else if (transform.localEulerAngles.y > 0)
                        {
                            transform.localEulerAngles -= new Vector3(0, 5f * Time.deltaTime, 0);
                        }
                    }
                    break;
                case LeverType.Button:
                    if (ButtonPress)
                    {
                        transform.localPosition = TargetPosition;
                    }
                    else
                    {
                        transform.localPosition = DefaultPosition;
                    }
                    break;
                case LeverType.Key:
                    if (ButtonPress)
                    {
                        transform.localEulerAngles += new Vector3(0, 0, 5 * Time.deltaTime);
                    }
                    else if (backwarPress)
                    {
                        transform.localEulerAngles -= new Vector3(0, 0, -5 * Time.deltaTime);
                    }
                    break;
            }
            //transform.localEulerAngles = new Vector3(Mathf.Clamp(transform.localEulerAngles.x,-25f,25f),
            //    Mathf.Clamp(transform.localEulerAngles.y, -25f, 25f),
            //    Mathf.Clamp(transform.localEulerAngles.z, -25f, 25f));
        }

        private void LateUpdate()
        {
            LevelTransition();
        }
    }
}