using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

namespace Schizohaptic
{
    [Serializable]
    public class Terapia
    {
        public string psiquiatra;
        public string idPaciente;
        public string genero;
        public string fechaNacimiento;
        public bool opcional1;
        public bool opcional2;
        public bool opcional3;
    }
    [Serializable]
    public class BaseDatos
    {
        public string portDerecha;
        public string portIzquierda;
        public ArrayList terapias;
        public BaseDatos()
        {
            terapias = new ArrayList();
        }
    }
    public class GlobalControl : MonoBehaviour
    {
        public BaseDatos database = new BaseDatos();
        public bool IsSceneBeingLoaded = false;
        public string dataFolder = "Database";
        public string dataFile = "/data.binary";

        public static GlobalControl Instance;

        // Singleton pattern
        void Awake()
        {
            if (Instance == null)
            {
                DontDestroyOnLoad(gameObject);
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void SaveData()
        {
            if (!Directory.Exists(dataFolder))
                Directory.CreateDirectory(dataFolder);

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream saveFile = File.Create(dataFolder + dataFile);

            database = GlobalControl.Instance.database;

            formatter.Serialize(saveFile, database);

            saveFile.Close();
            Debug.Log("Database saved");
        }

        public void LoadData()
        {
            if (File.Exists(dataFolder + dataFile))
            {
                Debug.Log("Cargando archivo "+ dataFolder + dataFile + "...");
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream saveFile = File.Open(dataFolder+dataFile, FileMode.Open);

                database = (BaseDatos)formatter.Deserialize(saveFile);
                saveFile.Close();
            } else
            {
                Debug.Log("No hay archivo de BD creando nueva...");
                database = new BaseDatos();
                database.terapias = new ArrayList();
                database.portDerecha = "";
                database.portIzquierda = "";
            }
        }

        public void AddTerapia(Terapia terapia)
        {
            database.terapias.Add(terapia);
            SaveData();
        }
    }
}