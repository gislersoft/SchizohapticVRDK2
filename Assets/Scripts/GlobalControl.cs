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
    public class Entidad
    {
        public string nombre;
        public bool real;
    }
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

        public string horaInicio;
        public string horaFin;
        public int vocesDropdown;
        public string voces;
        public string lugar;

        public bool suspende;
        public bool distraido;
        public bool noEntiende;
        public bool reconoce;
        public bool mareo;

        public string notas;
        public string entidadesNoReales;
        public string entidadesReales;
 
        public Entidad[] entidades;
    }
    [Serializable]
    public class BaseDatos
    {
        public string portDerecha;
        public string portIzquierda;
        public ArrayList terapias;
        public string ultimoPsiquiatra;

        public string etiqueta1;
        public string etiqueta2;
        public string etiqueta3;
        public BaseDatos()
        {
            terapias = new ArrayList();
        }
    }
    public class GlobalControl : MonoBehaviour
    {
        public BaseDatos database = new BaseDatos();
        public bool IsSceneBeingLoaded = false;

        public string saveFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).Replace("\\", "/");
        public string dataFolder = "";
        public string dataFile = "/data.binary";

        public Terapia terapiaActual;
        

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
            this.dataFolder = saveFolder + "/Schizohaptic/Database";
            if (!Directory.Exists(dataFolder))
            {
                Debug.Log("No existe el directorio "+dataFolder+ " creandolo...");
                Directory.CreateDirectory(dataFolder);
            }

            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                FileStream saveFile = File.Create(dataFolder + dataFile);

                database = GlobalControl.Instance.database;

                formatter.Serialize(saveFile, database);

                saveFile.Close();
                Debug.Log("Database saved");
            } catch(Exception e)
            {
                Debug.LogError("Error creando el archivo "+ dataFolder + dataFile);
                Debug.LogError(e);
            }
        }

        public void LoadData()
        {
            this.dataFolder = saveFolder + "/Schizohaptic/Database";
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
            this.terapiaActual = terapia;
            SaveData();
        }

        public Terapia GetTerapiaActual()
        {
            return this.terapiaActual;
        }

        public void actualizarEntidadesTerapia(Entidad[] entidades)
        {
            this.terapiaActual.entidades = entidades;
        }

        public string GetEntidadesPorComa(Entidad[] entidades, bool real)
        {
            string entidadesString = "";
            for (int i = 0; i < entidades.Length; i++)
            {
                if (entidades[i].real == real)
                {
                    if (entidadesString == "")
                    {
                        entidadesString = entidades[i].nombre.Trim();
                    }
                    else
                    {
                        entidadesString = entidadesString + "," + entidades[i].nombre.Trim();
                    }
                }
            }
            return entidadesString;
        }
    }
}