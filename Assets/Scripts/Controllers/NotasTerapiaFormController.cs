using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Schizohaptic;

public class NotasTerapiaFormController : MonoBehaviour
{
    Text textHoraInicio;
    Text textHoraFin;
    Text textEntidadesReales;
    Text textEntidadesNoReales;
    Text textVoces;
    Text textLugar;

    Text textNotas;

    Toggle opcion1, opcion2, opcion3, opcion4, opcion5;

    // Use this for initialization
    void Start()
    {
        this.textHoraInicio = GameObject.Find("TextHoraInicio").GetComponent<Text>();
        this.textHoraFin = GameObject.Find("TextHoraFin").GetComponent<Text>();
        this.textEntidadesReales = GameObject.Find("TextEntidadesReales").GetComponent<Text>();
        this.textEntidadesNoReales = GameObject.Find("TextEntidadesNoReales").GetComponent<Text>();
        this.textVoces = GameObject.Find("TextPaqueteVoces").GetComponent<Text>();
        this.textLugar = GameObject.Find("TextLugar").GetComponent<Text>();

        this.opcion1 = GameObject.Find("Opcion1").GetComponent<Toggle>();
        this.opcion2 = GameObject.Find("Opcion2").GetComponent<Toggle>();
        this.opcion3 = GameObject.Find("Opcion3").GetComponent<Toggle>();
        this.opcion4 = GameObject.Find("Opcion4").GetComponent<Toggle>();
        this.opcion5 = GameObject.Find("Opcion5").GetComponent<Toggle>();

        this.textNotas = GameObject.Find("TextNotas").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalControl.Instance.terapiaActual != null)
        {
            textHoraInicio.text = GlobalControl.Instance.terapiaActual.horaInicio;
            textHoraFin.text = GlobalControl.Instance.terapiaActual.horaFin;
            textEntidadesReales.text = GlobalControl.Instance.terapiaActual.entidadesReales;
            textEntidadesNoReales.text = GlobalControl.Instance.terapiaActual.entidadesNoReales;
            textVoces.text = GlobalControl.Instance.terapiaActual.voces;
            textLugar.text = GlobalControl.Instance.terapiaActual.lugar;

            GlobalControl.Instance.terapiaActual.suspende = opcion1.isOn;
            GlobalControl.Instance.terapiaActual.distraido = opcion2.isOn;
            GlobalControl.Instance.terapiaActual.noEntiende = opcion3.isOn;
            GlobalControl.Instance.terapiaActual.reconoce = opcion4.isOn;
            GlobalControl.Instance.terapiaActual.mareo = opcion5.isOn;

            GlobalControl.Instance.terapiaActual.notas = textNotas.text;
        }
    }

    public void guardarTerapia()
    {
        GlobalControl.Instance.AddTerapia(GlobalControl.Instance.terapiaActual);
        GlobalControl.Instance.SaveData();

        SceneManager.LoadScene("MenuPrincipal");
    }
}
