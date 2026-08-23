using UnityEngine;
using System;
using System.Drawing;

public class CoordinateMapper : MonoBehaviour
{
    [Range(-10,10)]
    public float X,Y;
    // Reference points
    private readonly double[] xRefs = { 10, -10, 10, -10, 0, 0, -10, 10, 0 };
    //private readonly double[] xRefs = { 0, 15, -15, 0, 0, -15, 15, -15, 15 };
    private readonly double[] yRefs = { 0, 0, 0, 10, -10, -10, 10, 10, -10 };
    private readonly double[] motor1Refs = { 505, 560, 450, 450, 560, 505, 505, 450, 560 };
    private readonly double[] motor2Refs = { 565, 680, 450, 680, 450, 450, 680, 565, 565 };

    public SerialCommunication SC;
    // Mapping function
    private void Awake()
    {
        SC = GetComponent<SerialCommunication>();
    }
    public Vector2 MapToMotor(double x, double y)
    {
        // Calculate weights for interpolation
        double weightX1 = 0, weightX2 = 0, weightY1 = 0, weightY2 = 0;

        for (int i = 0; i < xRefs.Length; i++)
        {
            double distX = Math.Abs(x - xRefs[i]);
            double distY = Math.Abs(y - yRefs[i]);

            if (distX == 0 && distY == 0)
                return new Vector2((float)motor1Refs[i], (float)motor2Refs[i]); // Return exact match

            double weight = 1 / (distX + distY);

            weightX1 += weight * motor1Refs[i];
            weightX2 += weight * motor2Refs[i];
            weightY1 += weight;
            weightY2 += weight;
        }

        // Interpolate
        double motor1 = weightX1 / weightY1;
        double motor2 = weightX2 / weightY2;

        return new Vector2((int)motor1, (int)motor2);
    }

    // Example usage
    void LateUpdate()
    {
        var motorValues = MapToMotor(X, Y);
        SC.SetMotorValues((int)motorValues.x, (int)motorValues.y);
    }

    public void SetXYRotationValues(float angleX, float angleY)
    {
        X = angleX; Y = angleY;
    }
}