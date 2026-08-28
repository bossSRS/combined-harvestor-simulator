using System;
using UnityEngine;

public class MotorMapping : MonoBehaviour
{
    [Range(-1f,1f)]
    public float InputX,InputY;

    public Vector2 motorOutput;
    // Define the input and output ranges
    private Vector2 inputRange = new Vector2(-1, 1);
    private Vector2 motor1Range = new Vector2(450, 560);
    private Vector2 motor2Range = new Vector2(450, 680);

    // Define the mapping points
    private Vector2[] mappingPoints = new Vector2[]
    {
        new Vector2(0, 0), new Vector2(1, 0), new Vector2(-1, 0),
        new Vector2(0, 1), new Vector2(0, -1), new Vector2(-1, -1),
        new Vector2(1, 1), new Vector2(-1, 1), new Vector2(1, -1)
    };

    private Vector2[] motorOutputs = new Vector2[]
    {
        new Vector2(505, 565), new Vector2(560, 680), new Vector2(450, 450),
        new Vector2(450, 680), new Vector2(560, 450), new Vector2(505, 450),
        new Vector2(505, 680), new Vector2(450, 565), new Vector2(560, 565)
    };

    // Function to map XY coordinates to motor outputs
    public Vector2 MapXYToMotorOutput(Vector2 input)
    {
        // Clamp input to input range
        input.x = Mathf.Clamp(input.x, inputRange.x, inputRange.y);
        input.y = Mathf.Clamp(input.y, inputRange.x, inputRange.y);

        // Find the nearest mapping points
        Vector2 closestPoint1 = mappingPoints[0];
        Vector2 closestPoint2 = mappingPoints[1];
        float minDistance1 = Vector2.Distance(input, closestPoint1);
        float minDistance2 = Vector2.Distance(input, closestPoint2);

        for (int i = 1; i < mappingPoints.Length; i++)
        {
            float distance = Vector2.Distance(input, mappingPoints[i]);
            if (distance < minDistance1)
            {
                minDistance2 = minDistance1;
                closestPoint2 = closestPoint1;
                minDistance1 = distance;
                closestPoint1 = mappingPoints[i];
            }
            else if (distance < minDistance2)
            {
                minDistance2 = distance;
                closestPoint2 = mappingPoints[i];
            }
        }

        // Interpolate between the two closest mapping points
        float t = Mathf.InverseLerp(minDistance1, minDistance2, Vector2.Distance(input, closestPoint1));
        Vector2 motorOutput = Vector2.Lerp(motorOutputs[Array.IndexOf(mappingPoints, closestPoint1)], motorOutputs[Array.IndexOf(mappingPoints, closestPoint2)], t);

        return motorOutput;
    }

    // Example usage
    void Update()
    {
        // Test mapping
        motorOutput = MapXYToMotorOutput(new Vector2(InputX, InputY));
    }

}