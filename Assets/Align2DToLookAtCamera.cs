using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Align2DToLookAtCamera : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        // Get the main camera
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Align the object to look at the camera
        if (mainCamera != null)
        {
            Vector3 direction = mainCamera.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}
