using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ByteConverter : MonoBehaviour
{
    [SerializeField]
    int value;
    [SerializeField]
    string hexString;
    public static string ToHex(int value, bool bigEndian)
    {
        byte[] byteArray;
        
        if (bigEndian)
        {
            byteArray = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(byteArray);
        }
        else
        {
            byteArray = BitConverter.GetBytes(value);
        }
        string tempInput = BitConverter.ToString(byteArray).Replace("-", "");
        tempInput = tempInput.Remove(0, 4);
        return tempInput;
    }
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        hexString = ByteConverter.ToHex(value, true); // For big endian
        //print(hexString); // Output: "02" for the integer 2
    }
}
