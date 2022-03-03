using UnityEngine;
using UnityEngine.UI;
using Schizohaptic;
using System;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;

public class BaseDatosFormController : MonoBehaviour
{
    GameObject dialogoBorrar;
    Text textoTotalTerapias;
    public string exportFolder = "/Exports";
    public string exportFile = "/SchizohapticExport-";
    // Use this for initialization
    void Start()
    {
        dialogoBorrar = GameObject.Find("DeleteDialog");
        dialogoBorrar.SetActive(false);
        textoTotalTerapias = GameObject.Find("TextTotalTerapias").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalControl.Instance.database.terapias != null)
        {
            textoTotalTerapias.text = GlobalControl.Instance.database.terapias.Count + "";
        }
    }

    public void BorrarBaseDatos()
    {
        dialogoBorrar.SetActive(true);
    }

    public void VolverAlMenuPrincipal()
    {
        this.transform.gameObject.SetActive(false);
    }

    private string estaMarcado(bool opcion)
    {
        if (opcion)
        {
            return "X";
        }
        return "";
    }

    private string escaparCampo(string campo)
    {
        if (campo != null && campo != "")
        {
            Regex reg = new Regex("[*'\",_&#^@]");
            campo = reg.Replace(campo, string.Empty);
        }
        return "\"" + campo + "\"";
    }

    public void ExportarAExcel()
    {
        if (GlobalControl.Instance.database.terapias != null)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).Replace("\\", "/") + "/Schizohaptic/";
            if (!Directory.Exists(path+exportFolder))
                Directory.CreateDirectory(path+exportFolder);

            DateTime theTime = DateTime.Now;
            string date = theTime.ToString("yyyy-MM-dd-HH-mm-ss");
            string filePath = path + exportFolder + exportFile + date + ".csv";

            StreamWriter writer = new StreamWriter(filePath);

            string etiqueta1 = "Etiqueta 1";
            string etiqueta2 = "Etiqueta 2";
            string etiqueta3 = "Etiqueta 3";

            if (GlobalControl.Instance.database.etiqueta1 != null && GlobalControl.Instance.database.etiqueta1.Trim() != "")
            {
                etiqueta1 = GlobalControl.Instance.database.etiqueta1;
            }
            if (GlobalControl.Instance.database.etiqueta2 != null && GlobalControl.Instance.database.etiqueta2.Trim() != "")
            {
                etiqueta2 = GlobalControl.Instance.database.etiqueta2;
            }
            if (GlobalControl.Instance.database.etiqueta3 != null && GlobalControl.Instance.database.etiqueta3.Trim() != "")
            {
                etiqueta3 = GlobalControl.Instance.database.etiqueta3;
            }

            string[] encabezados =
            {
                "Psiquiatra",
                "ID Paciente",
                "Género",
                "Fecha de Nacimiento",
                etiqueta1,
                etiqueta2,
                etiqueta3,
                "Hora Inicio",
                "Hora Fin",
                "Voces",
                "Lugar",
                "Entidades Reales",
                "Entidades No Reales",
                "Suspende",
                "Distraido",
                "No entiende VR",
                "Reconoce",
                "Mareo por VR",
                "Notas"
            };

            string encabezado = "";
 
            for (int i = 0; i < encabezados.Length; i++)
            {
                if (encabezado == "")
                {
                    encabezado = this.escaparCampo(encabezados[i]);
                }
                else
                {
                    encabezado = encabezado + "," + this.escaparCampo(encabezados[i]);
                }
            }

            writer.WriteLine(encabezado);

            ArrayList terapiasAExportar = GlobalControl.Instance.database.terapias;

            for (int i = 0; i < terapiasAExportar.Count; ++i)
            {
                Terapia terapia = (Terapia)terapiasAExportar[i];
                writer.WriteLine(
                    this.escaparCampo(terapia.psiquiatra) + "," +
                    this.escaparCampo(terapia.idPaciente) + "," +
                    this.escaparCampo(terapia.genero) + "," +
                    this.escaparCampo(terapia.fechaNacimiento) + "," +
                    this.estaMarcado(terapia.opcional1) + "," +
                    this.estaMarcado(terapia.opcional2) + "," +
                    this.estaMarcado(terapia.opcional3) + "," +
                    this.escaparCampo(terapia.horaInicio) + "," +
                    this.escaparCampo(terapia.horaFin) + "," +
                    this.escaparCampo(terapia.voces) + "," +
                    this.escaparCampo(terapia.lugar) + "," +
                    "\""+terapia.entidadesReales + "\"," +
                    "\""+terapia.entidadesNoReales + "\"," +
                    this.estaMarcado(terapia.suspende) + "," +
                    this.estaMarcado(terapia.distraido) + "," +
                    this.estaMarcado(terapia.noEntiende) + "," +
                    this.estaMarcado(terapia.reconoce) + "," +
                    this.estaMarcado(terapia.mareo) + "," +
                    this.escaparCampo(terapia.notas)
                );
            }

            writer.Close();

            //string path = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/') - 1);
            // string path = Directory.GetCurrentDirectory();
           

            Debug.Log("file:///" + filePath);
            Application.OpenURL("file:///" + filePath);
        }
    }
}
