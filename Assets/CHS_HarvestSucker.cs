using CHS.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHS_HarvestSucker : MonoBehaviour
{
    public GameObject Target;
    // Start is called before the first frame update
    void Awake()
    {
        //Target = GameObject.FindWithTag("Harvested");
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, Target.transform.position) > 1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, 0.05f);
            transform.Rotate(Vector3.right * 45f *  Time.deltaTime,Space.Self);
        }
        else if(Vector3.Distance(transform.position, Target.transform.position) <= 1f)
        {
            gameObject.SetActive(false);
        }
    }
}
