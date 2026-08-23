using System;
using System.Collections;
using System.IO;
using System.IO.Ports;
using System.Text;
using TMPro; 
using UnityEngine;
using UnityEngine.UI;

public class ArduinoCommunication : MonoBehaviour
{
    [SerializeField]
    public TMP_InputField PortNo;
    [SerializeField]
    public TMP_InputField BanuRate;

    SerialPort serialPort;
    [SerializeField]
    TMP_InputField inputField;
    [SerializeField]
    Button Send;
    [SerializeField]
    TMP_Text ChatComms;


    public Transform Connector;
    public Transform ChatCommunicator;

    public ByteConverter LeftMotorData;
    public ByteConverter RightMotorData;

    public string LeftMotorDataTxt;
    public string RightMotorDataTxt;

    public enum State
    {
        Neutral,
        Read,
        Write,
    }

    public State state;
    void Start()
    {
        Application.targetFrameRate = 60;
        PortNo.text = "COM7";
        BanuRate.text = "500000";
        state = State.Neutral;
        Send.onClick.AddListener(() =>
        {
            SendDataToArduino();
        });
    }
    public void Port()
    {
        StartCoroutine("Port_IE");
    }
    public IEnumerator Port_IE()
    {
        serialPort = new SerialPort(PortNo.text, int.Parse(BanuRate.text)); // Change COM3 to match your Arduino's port
        serialPort.Open();
        //serialPort = new SerialPort(PortNo.text, int.Parse(BanuRate.text)); // Change COM3 to match your Arduino's port
        //serialPort.Open();
        serialPort.ReadTimeout = 10000;

        yield return new WaitUntil(()=> serialPort.IsOpen);
        print("Successfully Connected");
        Connector.gameObject.SetActive(false);
        ChatCommunicator.gameObject.SetActive(true);
        state = State.Write;
        //InvokeRepeating("ReadData", 1f, 0.03f);
    }
    void Update()
    {
        // Example: Send data to Arduino
        if (Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            SendDataToArduino();
        }
        /*if (state == State.Read) 
        { 
            ReadData();
        }*/
    }
    private void LateUpdate()
    {/*
        if (ChatCommunicator.gameObject.activeInHierarchy && state == State.Read)
        {
            if (serialPort.IsOpen)
            {
                string data = serialPort.ReadLine();
                if (data != null)
                {
                    UpdateChatComms(data, "ADU");
                    state = State.Neutral;
                }
                else return;
            }
        }
        */
    }

    public void ReadData()
    {
        if (ChatCommunicator.gameObject.activeInHierarchy && state == State.Read)
        {
            if (serialPort.IsOpen)
            {
                string data = serialPort.ReadLine();
                if (data != null)
                {
                    UpdateChatComms(data, "ADU");
                }
            }
        }
    }
    private void UpdateChatComms(string text, string sender)
    {
        //ChatComms.text += "\n" + sender +" :" + text;
        ChatComms.text = sender + " :" + text;
        print(sender + " :" + text);
    }
    public byte[] SendEnableDataToArduino()
    {
        string strA = "[A";
        string strB = "][B";
        byte[] hexPosition = Encoding.UTF8.GetBytes("450");

        byte[] bytesA = Encoding.UTF8.GetBytes(strA);
        byte[] bytesB = Encoding.UTF8.GetBytes(strB);

        using (MemoryStream stream = new MemoryStream())
        {
            stream.Write(bytesA, 0, bytesA.Length);
            stream.Write(hexPosition, 0, hexPosition.Length);
            stream.Write(bytesB, 0, bytesB.Length);
            stream.Write(hexPosition, 0, hexPosition.Length);
            stream.WriteByte((byte)']');

            byte[] result = stream.ToArray();

            foreach (byte b in result)
            {
                print(b + " ");
            }
            return result;
        }
        /*string data = "[ena]";
        byte[] bytesData = Encoding.UTF8.GetBytes(data);

        // Print the byte array
        print("Byte representation of the string:");
        foreach (byte b in bytesData)
        {
            print(b + " ");
        }
        //UpdateChatComms(inputField.text, "CHS");
        state = State.Write;*/
    }
    public void SendDataToArduino()
    { 
        serialPort.Write(SendEnableDataToArduino(),0, SendEnableDataToArduino().Length);
        //UpdateChatComms(inputField.text,"CHS");
        //state = State.Read;
    }
    void OnApplicationQuit()
    {
        serialPort.Close();
    }
}
