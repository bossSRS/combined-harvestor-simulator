using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CharToByteConverter : MonoBehaviour
{
    public static byte[] CharToBytes(char c)
    {
        // Convert the char to a string
        string charString = c.ToString();
       
        // Convert the string to bytes using Unicode encoding
        return Encoding.Unicode.GetBytes(charString);
    }
}
