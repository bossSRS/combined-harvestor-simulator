using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Transform Target;
    [Range(0,10)]
    public float speed;

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        if(Vector3.Distance(transform.position,Target.position) < 1f)
        {
            gameObject.SetActive(false);
        }
    }
}
