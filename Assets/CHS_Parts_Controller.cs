using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHS.Controller
{
    public class CHS_Parts_Controller : MonoBehaviour
    {
        public string Name;
        public float TopValue;
        public float BottomValue;
        public float speed;

        public Mobility mobilityType;
        // Start is called before the first frame update
        void Start()
        {

        }
        public void moveParts(int direction)
        {
            if (mobilityType == Mobility.Transition)
            {

            }
            else if (mobilityType == Mobility.Rotation)
            {
                transform.localEulerAngles += new Vector3(direction * speed * Time.deltaTime, 0f, 0f);
            }
        }
        private void LateUpdate()
        {
            transform.localEulerAngles = new Vector3(Mathf.Clamp(transform.localEulerAngles.x,BottomValue,TopValue), 0f, 0f);
        }
    }
}
public enum Mobility
{
    Transition = 1,
    Rotation =  2,
}
