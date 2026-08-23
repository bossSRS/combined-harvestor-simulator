using System;
using System.IO.Ports;
using System.Text;
using System.Collections;
using UnityEngine;
using System.Threading;

public class SerialCommunication : MonoBehaviour
{
    public bool isRunning;
    private SerialPort serialPort;
    private int baudRate = 500000;
    private string portName = "COM9"; // Use the appropriate COM port
    private byte[] ena = Encoding.UTF8.GetBytes("[ena]");

    private int delayMicroseconds = 25; // Delay time in microseconds

    [Range(450,560)]
    public int LeftMotor;
    [Range(450, 680)]
    public int RightMotor;
    private Thread serialThread;



    public void Start()
    {
        if (!RetriveBinDataFromFile())
        {
            portName = "COM9";
            baudRate = 500000;
        }
        if (portName != null && baudRate != 0)
        {
            ConnectToSimulator();
        }
        // Open a serial connection to the Arduino
        
        
    }

    private void ConnectToSimulator()
    {
        StartCoroutine("ConnectToSimulator_IE");
    }
    public IEnumerator ConnectToSimulator_IE()
    {
        serialPort = new SerialPort(portName, baudRate);
        serialPort.DtrEnable = false;
        serialPort.RtsEnable = false;
        serialPort.Open();
        yield return new WaitUntil(() => serialPort.IsOpen);
        isRunning = true;
        serialThread = new Thread(WriteSerialData);
        serialThread.Start();
    }

    private void WriteSerialData()
    {
        while (isRunning)
        {
            if(serialPort.IsOpen)
            {
                SendDataToHarvestor();
            }
        }
    }
    /*private void Update()
    {
        if(serialPort.IsOpen)
        {
            SendDataToHarvestor();
        }
    }*/
    private void OpenPort()
    {
        try
        {
            serialPort.Open();
            Debug.Log("Port opened");
        }
        catch (Exception e)
        {
            Debug.LogError("Error opening port: " + e.Message);
        }
    }
    private void SendPositionData(int positionLeft, int positionRight)
    {
        if (0 <= positionLeft && positionLeft <= 0xFFFF) // Ensure the position fits in 2 bytes (0-65535 in decimal)
        {
            if (0 <= positionRight && positionRight <= 0xFFFF)
            {
                // Convert the position to a 2-byte hexadecimal bytes object
                byte[] hexPositionL = BitConverter.GetBytes((ushort)positionLeft);
                byte[] hexPositionR = BitConverter.GetBytes((ushort)positionRight);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(hexPositionL);
                    Array.Reverse(hexPositionR);
                }

                byte[] dataPacket = Encoding.UTF8.GetBytes("[A");
                dataPacket = Combine(dataPacket, hexPositionL);
                dataPacket = Combine(dataPacket, Encoding.UTF8.GetBytes("],[B"));
                dataPacket = Combine(dataPacket, hexPositionR);
                dataPacket = Combine(dataPacket, Encoding.UTF8.GetBytes("]"));

                //Debug.Log(BitConverter.ToString(dataPacket));
                // Send the data packet to the Arduino
                serialPort.Write(dataPacket, 0, dataPacket.Length);
            }
        }
        else
        {
            Debug.LogError("Position value is out of range (0-0xFFFF)");
        }
    }

    private byte[] Combine(byte[] first, byte[] second)
    {
        byte[] combined = new byte[first.Length + second.Length];
        Buffer.BlockCopy(first, 0, combined, 0, first.Length);
        Buffer.BlockCopy(second, 0, combined, first.Length, second.Length);
        return combined;
    }
    void OnApplicationQuit()
    {
        isRunning = false;
        ClosePort();
        // Wait for the serial thread to finish
        if (serialThread != null && serialThread.IsAlive)
        {
            serialThread.Join();
        }
    }
    public void ClosePort()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("Port closed");
        }
    }
    public void SendDataToHarvestor()
    {
        serialPort.Write(ena, 0, ena.Length);
        //GetInputData();
        SendPositionData(LeftMotor, RightMotor);
    }

    public void SetMotorValues(int x, int y)
    {
        LeftMotor = x;
        RightMotor = y;
    }
    private bool RetriveBinDataFromFile()
    {
        return false;
    }
}
