using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Schizohaptic
{
    public class ScriptPruebaGuantes : MonoBehaviour
    {
        SerialPort serialPortDetect;
        SerialPort serialPortDerecha;
        SerialPort serialPortIzquierda;
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
        private string[] actuadores = {
        "X", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N"
    };

        void Start()
        {
            this.timeRemaining = this.timePerPulse;
            hideAllActuators("R");
            hideAllActuators("L");
            if (portStringIzquierda == "" || portStringDerecha == "")
            {
                if (this.hilo == null || !this.hilo.IsAlive)
                {
                    detectar = true;
                    this.hilo = new Thread(detectDevicesInPort);
                    this.hilo.Start();
                    this.hilo2 = new Thread(sendOSignal);
                    hilo2.Start();
                }
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
            }
            else
            {
                serial.Open();
                serial.Write(actuador);
                serial.Write(actuador);
                serial.Write(actuador);
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
                    }
                }
            }
        }

        public void sendOSignal()
        {
            Debug.Log("Iniciando SendOSignal...");
            while (detectar)
            {
                Debug.Log("...");
                Thread.Sleep(1000);
                try
                {
                    if (serialPortDetect != null)
                    {
                        this.turOnSignal("O", serialPortDetect);
                    }
                }
                catch
                {
                    try
                    {
                        if (serialPortDetect != null)
                        {
                            this.turOnSignal("O", serialPortDetect);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                    }
                }
            }
            Debug.Log("Fin de envio de senal");
        }

        public void detectDevicesInPort()
        {
            bool manoDerecha = false;
            bool manoIzquierda = false;
            while (detectar)
            {
                // Get a list of serial port names.
                string[] ports = SerialPort.GetPortNames();
                Debug.Log(ports.Length + " port(s) found:");

                foreach (string port in ports)
                {
                    try
                    {
                        Debug.Log("Puerto " + port + "...");

                        if (serialPortDetect != null)
                        {
                            if (serialPortDetect.IsOpen)
                            {
                                serialPortDetect.Close();
                            }
                            serialPortDetect = null;
                        }
                        serialPortDetect = new SerialPort(port, BAUD_RATE);
                        serialPortDetect.ReadTimeout = 12000;

                        if (!serialPortDetect.IsOpen)
                        {
                            serialPortDetect.Open();
                        }

                        String dataRead = serialPortDetect.ReadLine();
                        Debug.Log(dataRead);

                        if (dataRead == "Q")
                        {
                            Debug.Log("Mano derecha en puerto " + port);
                            manoDerecha = true;
                            portStringDerecha = port;
                        }
                        else if (dataRead == "P")
                        {
                            Debug.Log("Mano izquierda en puerto " + port);
                            manoIzquierda = true;
                            portStringIzquierda = port;
                        }

                        if (manoDerecha && manoIzquierda)
                        {
                            detectar = false;
                            Debug.Log("Fin de la deteccion");
                        }
                    }
                    catch
                    {
                        // Do nothing.
                    }
                }
                serialPortDetect.Close();
                serialPortDetect = null;
            }
        }

        public void prueba()
        {
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
                    serialPortDerecha.Open();
                }
            }
            else
            {
                if (!serialPortDerecha.IsOpen)
                {
                    serialPortDerecha.Open();
                }
            }
            if (serialPortIzquierda == null)
            {
                serialPortIzquierda = new SerialPort(this.portStringIzquierda, BAUD_RATE);
                if (!serialPortIzquierda.IsOpen)
                {
                    serialPortIzquierda.Open();
                }
            }
            else
            {
                if (!serialPortIzquierda.IsOpen)
                {
                    serialPortIzquierda.Open();
                }
            }
        }

        public void showActuator(string mano, string name)
        {
            return;
            if (name == "X") return;
            Image image = GameObject.Find(name + mano).GetComponent<Image>();
            var tempColor = image.color;
            tempColor.a = 1f;
            image.color = tempColor;
        }

        public void hideActuator(string mano, string name)
        {
            return;
            if (name == "X") return;
            Image image = GameObject.Find(name + mano).GetComponent<Image>();
            var tempColor = image.color;
            tempColor.a = 0f;
            image.color = tempColor;
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
            // GameObject.Find("ButtonProbar").GetComponent<Button>().interactable = !this.detectar;
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
                    showActuator(manoActual, actuadores[actuadorActual]);
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
        }

        void OnApplicationQuit()
        {
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
            if (this.hilo != null && this.hilo.IsAlive)
            {
                this.hilo.Abort();
            }
            if (this.hilo2 != null && this.hilo2.IsAlive)
            {
                this.hilo2.Abort();
            }
        }
    }
}
