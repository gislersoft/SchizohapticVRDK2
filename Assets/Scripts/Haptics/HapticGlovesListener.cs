using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Schizohaptic
{
    public class HapticGlovesListener : MonoBehaviour
    {
        SerialPort serialPortDetect;
        SerialPort serialPortDerecha;
        SerialPort serialPortIzquierda;


        SerialPort[] serialPorts;

        public string portStringIzquierda = "";
        public string portStringDerecha = "";

        public string manoActual = "R";
        public float timePerPulse = 1;
        private float timeRemaining = 1;
        private static int BAUD_RATE = 9600;

        private Thread hilo = null;
        private Thread hilo2 = null;

        public int actuadorActual = 0;
        public bool iniciarPrueba = false;
        public bool detectar = false;
        public bool modoPruebas = true;

        private string[] actuadores = {
            "X", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N"
        };

        void Start()
        {
            this.timeRemaining = this.timePerPulse;
           // hideAllActuators("R");
           // hideAllActuators("L");
        }

        public void restartValues()
        {
            portStringIzquierda = "";
            portStringDerecha = "";

            manoActual = "R";
            timePerPulse = 1;
            timeRemaining = 1;

            actuadorActual = 0;
            iniciarPrueba = false;
            detectar = false;
    }

        public void DetectHands()
        {
            detectar = false;
            this.timeRemaining = this.timePerPulse;
            hideAllActuators("R");
            hideAllActuators("L");
            if (portStringIzquierda == "" || portStringDerecha == "")
            {
                this.restartValues();
                if (this.hilo != null)
                {
                    if (this.hilo.IsAlive)
                    {
                        this.hilo.Abort();
                    }
                    this.hilo = null;
                }
                if (this.hilo2 != null)
                {
                    if (this.hilo2.IsAlive)
                    {
                        this.hilo2.Abort();
                    }
                    this.hilo2 = null;
                }
                detectar = true;
                this.hilo = new Thread(detectDevicesInPort);
                this.hilo.Start();
                this.hilo2 = new Thread(sendOSignal);
                hilo2.Start();
            }
        }

        public void AllActuatorsTest(string mano)
        {
            for (int i = 0; i < actuadores.Length; i++)
            {
                this.turnOnActuator(mano, actuadores[i]);
            }
        }

        public void turOnSignal(string actuador, SerialPort serial)
        {
            if (serial.IsOpen)
            {
                serial.Write(actuador);
                serial.Write(actuador);
                serial.Write(actuador);
                if (modoPruebas)
                {
                    serial.Write(actuador);
                    serial.Write(actuador);
                    serial.Write(actuador);
                    serial.Write(actuador);
                    serial.Write(actuador);
                    serial.Write(actuador);
                }
            }
            else
            {
                serial.Open();
                serial.Write(actuador);
                serial.Write(actuador);
                serial.Write(actuador);
                if (modoPruebas)
                {
                    serial.Write(actuador);
                    serial.Write(actuador);
                    serial.Write(actuador);
                    serial.Write(actuador);
                    serial.Write(actuador);
                    serial.Write(actuador);
                }
            }
        }

        public void turnOnActuator(string mano, string actuador)
        {
            if (mano == "R")
            {
                Debug.Log(mano + " " + this.portStringDerecha + " " + actuador);
                try
                {
                    if (serialPortDerecha == null)
                    {
                        serialPortDerecha = new SerialPort(this.portStringDerecha, BAUD_RATE);
                    }
                    this.turOnSignal(actuador, serialPortDerecha);
                }
                catch
                {
                    try
                    {
                        if (serialPortDerecha == null)
                        {
                            serialPortDerecha = new SerialPort(this.portStringDerecha, BAUD_RATE);
                        }
                        this.turOnSignal(actuador, serialPortDerecha);
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                        try
                        {
                            serialPortDerecha.Close();
                            serialPortDerecha = null;
                        }
                        catch {}
                        if (modoPruebas)
                        {
                            iniciarPrueba = false;
                            this.portStringDerecha = "";
                            if (this.serialPorts != null)
                            {
                                for (int j = 0; j < this.serialPorts.Length; j++)
                                {
                                    if (this.serialPorts != null)
                                    {
                                        try
                                        {
                                            this.serialPorts[j].Close();
                                        }
                                        catch { }
                                    }
                                }
                            }
                            this.serialPorts = null;
                            DetectHands();
                        }
                    }
                }
            }
            else
            {
                Debug.Log(mano + " " + this.portStringIzquierda + " " + actuador);
                try
                {
                    if (serialPortIzquierda == null)
                    {
                        serialPortIzquierda = new SerialPort(this.portStringIzquierda, BAUD_RATE);
                    }
                    this.turOnSignal(actuador, serialPortIzquierda);
                }
                catch
                {
                    try
                    {
                        if (serialPortIzquierda == null)
                        {
                            serialPortIzquierda = new SerialPort(this.portStringIzquierda, BAUD_RATE);
                        }
                        this.turOnSignal(actuador, serialPortIzquierda);
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                        try
                        {
                            serialPortIzquierda.Close();
                            serialPortIzquierda = null;
                        }
                        catch { }
                        if (modoPruebas)
                        {
                            iniciarPrueba = false;
                            this.portStringIzquierda = "";
                            if (this.serialPorts != null)
                            {
                                for (int j = 0; j < this.serialPorts.Length; j++)
                                {
                                    if (this.serialPorts != null)
                                    {
                                        try
                                        {
                                            this.serialPorts[j].Close();
                                        }
                                        catch { }
                                    }
                                }
                            }
                            this.serialPorts = null;
                            DetectHands();
                        }
                    }
                }
            }
        }

        public void sendOSignal()
        {
            Debug.Log("Iniciando SendOSignal...");
            while (detectar)
            {
                // Debug.Log("...");
                Thread.Sleep(500);
                if (serialPorts != null)
                {
                    for (int k = 0; k < serialPorts.Length; k++)
                    {
                        try
                        {
                            if (serialPorts[k] != null)
                            {
                                //Debug.Log("Enviando senal O aa" + serialPorts[k].PortName);
                                this.turOnSignal("O", serialPorts[k]);
                            }
                        }
                        catch
                        {
                            try
                            {
                                if (serialPorts[k] != null)
                                {
                                    //Debug.Log("Enviando senal O a"+serialPortDetect.PortName);
                                    this.turOnSignal("O", serialPorts[k]);
                                }
                            }
                            catch (Exception e)
                            {
                                Debug.Log(e);
                            }
                        }
                    }
                }
            }
            Debug.Log("Fin de envio de senal");
        }

        //Handler as indicatted above will be run when the event is triggered.
        private void ProcessReceivedData(object sender, SerialDataReceivedEventArgs e)
        {
            Debug.Log(e);
            Debug.Log(sender);
        }

        public void detectDevicesInPort()
        {
            bool manoDerecha = false;
            bool manoIzquierda = false;
            while (detectar)
            {
                // Get a list of serial port names.
                string[] ports = SerialPort.GetPortNames();
                this.serialPorts = new SerialPort[ports.Length];
                int i = 0;
                Debug.Log(ports.Length + " port(s) found:");

                foreach (string port in ports)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        try
                        {

                            if (serialPorts[i] != null)
                            {
                                if (!serialPorts[i].IsOpen)
                                {
                                    serialPorts[i].Open();
                                }
                                // serialPortDetect = null;
                                Debug.Log("Leyendo en puerto " + port + "...");
                                String dataRead = serialPorts[i].ReadLine();
                                Debug.Log("[" + dataRead.Trim() + "]");

                                if (dataRead.Trim() == "Q")
                                {
                                    Debug.Log("Mano derecha en puerto " + port);
                                    manoDerecha = true;
                                    portStringDerecha = port;
                                }
                                else if (dataRead.Trim() == "P")
                                {
                                    Debug.Log("Mano izquierda en puerto " + port);
                                    manoIzquierda = true;
                                    portStringIzquierda = port;
                                }

                                if (manoDerecha && manoIzquierda)
                                {
                                    detectar = false;

                                    GlobalControl.Instance.database.portDerecha = this.portStringDerecha;
                                    GlobalControl.Instance.database.portIzquierda = this.portStringIzquierda;
                                    GlobalControl.Instance.SaveData();
                                    Debug.Log("Fin de la deteccion");
                                }
                            }
                            else
                            {
                                
                                serialPorts[i] = new SerialPort(port, BAUD_RATE);
                                serialPorts[i].ReadTimeout = 12000;
                                serialPorts[i].WriteTimeout = 12000;
                                // serialPortDetect.DataReceived += new SerialDataReceivedEventHandler(ProcessReceivedData);


                                if (!serialPorts[i].IsOpen)
                                {
                                    serialPorts[i].Open();
                                }

                                /*
                                Debug.Log("Escribiendo en puerto " + port + "...");

                                /this.turOnSignal("O", serialPorts[i]);
                                */

                            }
                        }
                        catch (Exception excep)
                        {
                            Debug.Log(excep);
                        }
                    }

                    serialPorts[i].Close();
                    serialPorts[i] = null;
                    i++;
                }

            }
        }

        public void prueba()
        {
            this.modoPruebas = true;
            this.timeRemaining = this.timePerPulse;
            hideAllActuators("R");
            hideAllActuators("L");
            iniciarPrueba = true;
            actuadorActual = 0;
            manoActual = "R";
            Debug.Log("Derecha:" + this.portStringDerecha + " Izquierda:" + this.portStringIzquierda);
            if (serialPortDerecha == null)
            {
                serialPortDerecha = new SerialPort(this.portStringDerecha, BAUD_RATE);
                if (!serialPortDerecha.IsOpen)
                {
                    try
                    {
                        serialPortDerecha.Open();
                    }
                    catch { }
                }
            }
            else
            {
                if (!serialPortDerecha.IsOpen)
                {
                    try {
                        serialPortDerecha.Open();
                    }
                    catch { }
                }
            }
            if (serialPortIzquierda == null)
            {
                serialPortIzquierda = new SerialPort(this.portStringIzquierda, BAUD_RATE);
                if (!serialPortIzquierda.IsOpen)
                {
                    try {
                        serialPortIzquierda.Open();
                    }
                    catch { }
                }
            }
            else
            {
                if (!serialPortIzquierda.IsOpen)
                {
                    try {
                        serialPortIzquierda.Open();
                    }
                    catch { }
                }
            }
        }

        public void showActuator(string mano, string name)
        {
            if (name == "X")
            {
                return;
            }
            if (GameObject.Find(name + mano) != null)
            {
                Image image = GameObject.Find(name + mano).GetComponent<Image>();
                var tempColor = image.color;
                tempColor.a = 1f;
                image.color = tempColor;
            }
        }

        public void hideActuator(string mano, string name)
        {
            if (name == "X")
            {
                return;
            }
            if (GameObject.Find(name + mano) != null)
            {
                Image image = GameObject.Find(name + mano).GetComponent<Image>();
                var tempColor = image.color;
                tempColor.a = 0f;
                image.color = tempColor;
            }
        }

        public void hideAllActuators(string mano)
        {
            for (int i = 0; i < actuadores.Length; i++)
            {
                this.hideActuator(mano, actuadores[i]);
            }
        }

        void Update()
        {
            if (modoPruebas)
            {
                if (detectar)
                {
                    GameObject.Find("Manos").GetComponent<Image>().color = new Color(1f, 1f, 1f, Mathf.PingPong(Time.time, 1));
                    GameObject.Find("BotonIniciarPrueba").GetComponent<Button>().interactable = false;
                } else
                {
                    if (GameObject.Find("Manos") != null) {
                        GameObject.Find("Manos").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                    }
                    if (GameObject.Find("BotonIniciarPrueba") != null)
                    {
                        GameObject.Find("BotonIniciarPrueba").GetComponent<Button>().interactable = true;
                    }
                }
            }
            if (Input.GetKey("escape"))
            {
                Application.Quit();
            }

            if (iniciarPrueba == true)
            {
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                }
                else
                {
                    this.timeRemaining = this.timePerPulse;
                    if (modoPruebas)
                    {
                        showActuator(manoActual, actuadores[actuadorActual]);
                    }
                    turnOnActuator(manoActual, actuadores[actuadorActual]);
                    actuadorActual++;
                    if (actuadorActual >= actuadores.Length)
                    {
                        actuadorActual = 0;
                        if (manoActual == "R")
                        {
                            manoActual = "L";
                        }
                        else
                        {
                            // Termino de probar todas las manos.
                            iniciarPrueba = false;
                            manoActual = "R";
                        }
                    }
                }
            }

            if (detectar)
            {
                if (portStringDerecha != "" && portStringIzquierda != "")
                {
                    detectar = false;
                }
            }
        }

        void OnApplicationQuit()
        {
            detectar = false;
            Debug.Log("Closing ports...");
            if (serialPortDetect != null)
            {
                if (serialPortDetect.IsOpen)
                {
                    serialPortDetect.Close();
                }
                serialPortDetect = null;
            }
            if (serialPortIzquierda != null)
            {
                if (serialPortIzquierda.IsOpen)
                {
                    serialPortIzquierda.Close();
                }
                serialPortIzquierda = null;
            }
            if (serialPortDerecha != null)
            {
                if (serialPortDerecha.IsOpen)
                {
                    serialPortDerecha.Close();
                }
                serialPortDerecha = null;
            }
            Debug.Log("Killing threads...");
            if (this.hilo != null)
            {
                this.hilo.Abort();
            }
            if (this.hilo2 != null)
            {
                this.hilo2.Abort();
            }
        }
    }
}
