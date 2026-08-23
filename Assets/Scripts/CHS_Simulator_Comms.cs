using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

namespace CHS.Input
{
    public enum State
    {
        Neutral,
        Active,
    }
    public class CHS_Simulator_Comms : MonoBehaviour
    {
        [Header("I/O Configs")]
        public State state;
        [SerializeField]
        private string PortNo;
        [SerializeField]
        private int BaudRate;

        private SerialPort serialPort;
        private Thread serialThread;
        [SerializeField]
        private bool isRunning = false;
        [SerializeField] string RAW_DATA;
        [SerializeField] private string[] DeconfigRawData;

        [Header("Simulator Data")]
        [SerializeField] private string Simulator_Version;
        [SerializeField] private string RPMData;
        [SerializeField] private string MainTransmission_Data;
        [SerializeField] private string Steering_Right;
        [SerializeField] private string Steering_Down;
        [SerializeField] private string Steering_Up;
        [SerializeField] private string Steering_Left;
        [SerializeField] private string Reer_Control_Wheel_Down;
        [SerializeField] private string Reer_Control_Wheel_Up;
        [SerializeField] private string Unload_Pipe_Control_Switch_Down;
        [SerializeField] private string Unload_Pipe_Control_Switch_Up;
        [SerializeField] private string TranmissionLever_Travel;
        [SerializeField] private string TranmissionLever_Neutral;
        [SerializeField] private string TranmissionLever_Harvesting;
        [SerializeField] private string Cutting_Lever_Forward;
        [SerializeField] private string Unloading_Pipe_Control_Switch_Left;
        [SerializeField] private string Unloading_Pipe_Control_Switch_Right;
        [SerializeField] private string Thashing_Lever_Backward;
        [SerializeField] private string Stop_Cable;
        [SerializeField] private string Neutral_Switch;
        [SerializeField] private string Reverse_Feeder_Chain_Leverr;
        [SerializeField] private string Grain_Main_Clutch_Lever;
        [SerializeField] private string Ignition_Switch;
        [SerializeField] private string Clutch_Paddle;


        private CHS_Input chs_Input;

        void Awake()
        {
            chs_Input = GetComponent<CHS_Input>();
        }

        // Start is called before the first frame update
        void Start()
        {
            state = State.Neutral;
            if (!RetriveBinDatafromFile())
            {
                PortNo = "COM7";
                BaudRate = 9600;
            }
            if(PortNo != null && BaudRate != 0)
            {
                ConnectToSimulator();
            }
        }
        private void ConnectToSimulator()
        {
            StartCoroutine("ConnectToSimulator_IE");
        }
        public IEnumerator ConnectToSimulator_IE()
        {
            serialPort = new SerialPort(PortNo, BaudRate); // Change COM3 to match your Arduino's port
            OpenPort();
            yield return new WaitUntil(() => serialPort.IsOpen);
            print("Successfully Connected");
            isRunning = true;
            serialThread = new Thread(ReadSerialData);
            serialThread.Start();
            state = State.Active;

        }
        private void OpenPort()
        {
            try
            {
                serialPort.DtrEnable = false;
                serialPort.RtsEnable = false;
                serialPort.Open();
                Debug.Log("Port opened");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        public void ClosePort()
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
                isRunning = false;
                serialThread.Join();
            }
        }
        private void OnDestroy()
        {
            isRunning = false; // Signal the thread to stop
            ClosePort();

            // Wait for the serial thread to finish
            if (serialThread != null && serialThread.IsAlive)
            {
                serialThread.Join();
            }
        }
        public bool RetriveBinDatafromFile()
        {
            ///Will be written Later
            return false;
        }
        private void ReadSerialData()
        {
            if (state == State.Active)
            {
                while (serialPort.IsOpen)
                {
                    string data = serialPort.ReadLine();
                    if (data == null)
                    {
                        Debug.LogError("Comms Not Working!!!!");
                    }
                    else
                    {
                        //print("reading data");
                        RAW_DATA = data;
                        RunDeconfig();
                    }
                }
            }
        }

        public void RunDeconfig()
        {
            //print("Running Deconfig");
            if (RAW_DATA == null) return;
            DeconfigRawData = RAW_DATA.Split(',');
            Simulator_Version = DeconfigRawData[0];
            RPMData = DeconfigRawData[1];
            MainTransmission_Data = DeconfigRawData[2];
            Steering_Left = DeconfigRawData[3]; Steering_Right = DeconfigRawData[6];
            Steering_Up = DeconfigRawData[4]; Steering_Down = DeconfigRawData[5];
            Reer_Control_Wheel_Down = DeconfigRawData[7];
            Reer_Control_Wheel_Up = DeconfigRawData[8];
            Unload_Pipe_Control_Switch_Down = DeconfigRawData[9];
            Unload_Pipe_Control_Switch_Up = DeconfigRawData[10];
            //clear
            
            TranmissionLever_Travel = DeconfigRawData[13];
            TranmissionLever_Neutral = DeconfigRawData[14];
            TranmissionLever_Harvesting = DeconfigRawData[15];
            Cutting_Lever_Forward = DeconfigRawData[16];
            Unloading_Pipe_Control_Switch_Left = DeconfigRawData[17];
            Unloading_Pipe_Control_Switch_Right = DeconfigRawData[18];
            Thashing_Lever_Backward = DeconfigRawData[19];

            Reverse_Feeder_Chain_Leverr = DeconfigRawData[11];
            Neutral_Switch = DeconfigRawData[12];
            Stop_Cable = DeconfigRawData[20];
            Grain_Main_Clutch_Lever = DeconfigRawData[21];
            Ignition_Switch = DeconfigRawData[22];
            Clutch_Paddle = DeconfigRawData[23];
        }

        internal string GetRPMData()
        {
            if (state == State.Active && !string.IsNullOrEmpty(RPMData)) return RPMData;
            return (chs_Input != null ? chs_Input.DefRPMValue.ToString() : "305");
        }

        public string GetMainTransmissonInput()
        {
            if (state == State.Active && !string.IsNullOrEmpty(MainTransmission_Data)) return MainTransmission_Data;
            return (chs_Input != null ? chs_Input.DefMainTransmissionValue.ToString() : "155");
        }

        internal string GetSteeringUDLRInput()
        {
            if (state == State.Active && !string.IsNullOrEmpty(Steering_Up))
                return Steering_Up + Steering_Down + Steering_Left + Steering_Right;
            if (chs_Input != null)
            {
                switch (chs_Input.steering_UpDownLeftRight)
                {
                    case CHS.Input.SteeringUpDownLeftRight.Up: return "1000";
                    case CHS.Input.SteeringUpDownLeftRight.Down: return "0100";
                    case CHS.Input.SteeringUpDownLeftRight.Left: return "0010";
                    case CHS.Input.SteeringUpDownLeftRight.Right: return "0001";
                }
            }
            return "0000";
        }

        internal string GetReelUDInput()
        {
            if (state == State.Active && !string.IsNullOrEmpty(Reer_Control_Wheel_Up))
                return Reer_Control_Wheel_Up + Reer_Control_Wheel_Down;
            if (chs_Input != null)
            {
                switch (chs_Input.reelControlLever)
                {
                    case CHS.Input.ReelControlLever.Up: return "10";
                    case CHS.Input.ReelControlLever.Down: return "01";
                }
            }
            return "00";
        }

        internal string GetThrashingInput()
        {
            if (state == State.Active && !string.IsNullOrEmpty(Thashing_Lever_Backward)) return Thashing_Lever_Backward;
            return (chs_Input != null && chs_Input.thrashing_Lever == CHS.Input.Thrashing_Lever.On) ? "0" : "1";
        }

        internal string GetCuttingInput()
        {
            if (state == State.Active && !string.IsNullOrEmpty(Cutting_Lever_Forward)) return Cutting_Lever_Forward;
            return (chs_Input != null && chs_Input.cutting_Lever == CHS.Input.Cutting_Lever.On) ? "1" : "0";
        }

        internal string GetUnloadPipeSwitchUD()
        {
            if (state == State.Active && !string.IsNullOrEmpty(Unload_Pipe_Control_Switch_Up))
                return Unload_Pipe_Control_Switch_Up + Unload_Pipe_Control_Switch_Down;
            if (chs_Input != null)
            {
                switch (chs_Input.unloadingCylinderLever)
                {
                    case CHS.Input.Unloading_Cylinder_Lever.Up: return "01";
                    case CHS.Input.Unloading_Cylinder_Lever.Down: return "10";
                }
            }
            return "00";
        }

        internal string GetUnloadedPipeSwitchLR() 
        {
            if (state == State.Active && !string.IsNullOrEmpty(Unloading_Pipe_Control_Switch_Left))
                return Unloading_Pipe_Control_Switch_Left + Unloading_Pipe_Control_Switch_Right;
            if (chs_Input != null)
            {
                switch (chs_Input.unloadingPipeControlLever)
                {
                    case CHS.Input.Unloading_Pipe_Control_Switch.Left: return "10";
                    case CHS.Input.Unloading_Pipe_Control_Switch.Right: return "01";
                }
            }
            return "00";
        }

        internal string GetSubTransmissionInputs()
        {
            if (state == State.Active && !string.IsNullOrEmpty(TranmissionLever_Travel))
                return TranmissionLever_Travel + TranmissionLever_Neutral + TranmissionLever_Harvesting;
            if (chs_Input != null)
            {
                switch (chs_Input.subTransmissionLever)
                {
                    case CHS.Input.SubTransmissionLever.Harvesting: return "001";
                    case CHS.Input.SubTransmissionLever.Neutral: return "010";
                    case CHS.Input.SubTransmissionLever.Travel: return "110";
                    case CHS.Input.SubTransmissionLever.Loading: return "000";
                }
            }
            return "010";
        }

        internal string GetIgnitionSwitchInput()
        {
            if (state == State.Active && !string.IsNullOrEmpty(Ignition_Switch)) return Ignition_Switch;
            return (chs_Input != null && chs_Input.startSwitch == CHS.Input.IgnitionSwitch.Started) ? "1" : "0";
        }

        internal string GetStopCableInput()
        {
            if (state == State.Active && !string.IsNullOrEmpty(Stop_Cable)) return Stop_Cable;
            return (chs_Input != null && chs_Input.stopCable == CHS.Input.Stop_Cable.On) ? "1" : "0";
        }

        internal string GetReverseFeedleChainInput()
        {
            if (state == State.Active && !string.IsNullOrEmpty(Reverse_Feeder_Chain_Leverr)) return Reverse_Feeder_Chain_Leverr;
            return (chs_Input != null && chs_Input.reverse_Feeder_Chain_Lever == CHS.Input.Reverse_Feeder_Chain_Lever.Backward) ? "1" : "0";
        }

        internal string GrainMainClutchInput()
        {
            if (state == State.Active && !string.IsNullOrEmpty(Grain_Main_Clutch_Lever)) return Grain_Main_Clutch_Lever;
            return (chs_Input != null && chs_Input.grainLever == CHS.Input.Grain_Main_Clutch_Lever.On) ? "1" : "0";
        }

        internal string ClutchPaddleInput()
        {
            if (state == State.Active && !string.IsNullOrEmpty(Clutch_Paddle)) return Clutch_Paddle;
            return (chs_Input != null && chs_Input.clutch_Paddle == CHS.Input.Clutch_Paddle.Down) ? "1" : "0";
        }

        internal string NeutralInput()
        {
            if (state == State.Active && !string.IsNullOrEmpty(Neutral_Switch)) return Neutral_Switch;
            return (chs_Input != null && chs_Input.neutralSwitch == CHS.Input.Neutral_Switch.On) ? "1" : "0";
        }

        void OnApplicationQuit()
        {
            ClosePort();
        }
    }   
}
