using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Schizohaptic;
using System;

public class TerapiaFormController : MonoBehaviour {
    private InputField inputTextPsiquiatra;
    private InputField inputTextIDPaciente;
    private Dropdown selectGenero;

    private Button botonIniciarTerapia;

    // Fecha
    private Dropdown selectMes;
    private InputField inputFieldAAA;
    private InputField inputFieldDD;

    private InputField etiqueta1;
    private InputField etiqueta2;
    private InputField etiqueta3;

    private GameObject pantallaConfiguracionTerapia;
    private bool disableOnUpdate = false;

    HapticGlovesListener hapticGlovesListener;

    // Use this for initialization
    void Start () {
        GameObject.Find("LMHeadMountedRig").GetComponent<AudioSource>().mute = false;
        this.inputTextPsiquiatra = GameObject.Find("InputFieldPsiquiatra").GetComponent<InputField>();
        this.inputTextIDPaciente = GameObject.Find("InputFieldPaciente").GetComponent<InputField>();
        this.selectGenero = GameObject.Find("DropdownGenero").GetComponent<Dropdown>();
        this.selectMes = GameObject.Find("DropdownMES").GetComponent<Dropdown>();
        this.inputFieldAAA = GameObject.Find("InputFieldAAAA").GetComponent<InputField>();
        this.inputFieldDD = GameObject.Find("InputFieldDD").GetComponent<InputField>();
        this.pantallaConfiguracionTerapia = GameObject.Find("PantallaConfiguracionTerapia");
        this.botonIniciarTerapia = GameObject.Find("BotonIniciarTerapia").GetComponent<Button>();
        this.botonIniciarTerapia.interactable = false;

        this.etiqueta1 = GameObject.Find("InputOpcional1").GetComponent<InputField>();
        this.etiqueta2 = GameObject.Find("InputOpcional2").GetComponent<InputField>();
        this.etiqueta3 = GameObject.Find("InputOpcional3").GetComponent<InputField>();

        hapticGlovesListener = GameObject.Find("HapticGlovesListener").GetComponent<HapticGlovesListener>();
        GlobalControl.Instance.LoadData();

        if (GlobalControl.Instance.database.etiqueta1 != null && GlobalControl.Instance.database.etiqueta1.Trim() != "") {
            this.etiqueta1.text = GlobalControl.Instance.database.etiqueta1;
        }
        else
        {
            this.etiqueta1.text = "Etiqueta 1";
        }
        if (GlobalControl.Instance.database.etiqueta2 != null && GlobalControl.Instance.database.etiqueta2.Trim() != "")
        {
            this.etiqueta2.text = GlobalControl.Instance.database.etiqueta2;
        }
        else
        {
            this.etiqueta2.text = "Etiqueta 2";
        }
        if (GlobalControl.Instance.database.etiqueta3 != null && GlobalControl.Instance.database.etiqueta3.Trim() != "")
        {
            this.etiqueta3.text = GlobalControl.Instance.database.etiqueta3;
        }
        else
        {
            this.etiqueta3.text = "Etiqueta 3";
        }

        hapticGlovesListener.portStringDerecha = GlobalControl.Instance.database.portDerecha;
        hapticGlovesListener.portStringIzquierda = GlobalControl.Instance.database.portIzquierda;

        if (GlobalControl.Instance.database.ultimoPsiquiatra != null && GlobalControl.Instance.database.ultimoPsiquiatra != "")
        {
            this.inputTextPsiquiatra.text = GlobalControl.Instance.database.ultimoPsiquiatra.Trim();
        }

        // Un primer centrado basico
        GameObject.Find("Escena").transform.forward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
    }
	
	// Update is called once per frame
	void Update () {
        if (!disableOnUpdate)
        {
            this.pantallaConfiguracionTerapia.SetActive(false);
            disableOnUpdate = true;
        }
        if (etiqueta1.text != null && etiqueta1.text.Trim() != "") {
            GlobalControl.Instance.database.etiqueta1 = etiqueta1.text;
        }
        if (etiqueta2.text != null && etiqueta2.text.Trim() != "")
        {
            GlobalControl.Instance.database.etiqueta2 = etiqueta2.text;
        }
        if (etiqueta3.text != null && etiqueta3.text.Trim() != "")
        {
            GlobalControl.Instance.database.etiqueta3 = etiqueta3.text;
        }
        botonIniciarTerapia.interactable = true;
        inputFieldDD.image.color = Color.white;
        inputFieldAAA.image.color = Color.white;
        this.validarCampos();
    }

    public void validarCampos()
    {
        if (inputTextPsiquiatra.text == null || inputTextPsiquiatra.text.ToString().Trim() == "")
        {
            botonIniciarTerapia.interactable = false;
            return;
        }

        if (inputTextIDPaciente.text == null || inputTextIDPaciente.text.ToString().Trim() == "")
        {
            botonIniciarTerapia.interactable = false;
            return;
        }

        this.validarFecha();
    }

    public void validarFecha()
    {
        int dia = 0;

        try
        {
            dia = Int32.Parse(inputFieldDD.text.ToString().Trim());
        }
        catch
        {
            botonIniciarTerapia.interactable = false;
            return;
        }

        int mes = selectMes.value;
        if (mes == 1 || mes == 3 || mes == 5 || mes == 7 || mes == 8 || mes == 10 || mes == 12)
        {
            if (dia > 31)
            {
                inputFieldDD.image.color = Color.yellow;
                botonIniciarTerapia.interactable = false;
                return;
            }
        }
        if (mes == 4 || mes == 6 || mes == 9 || mes == 11)
        {
            if (dia > 30)
            {
                inputFieldDD.image.color = Color.yellow;
                botonIniciarTerapia.interactable = false;
                return;
            }
        }
        if (mes == 2)
        {
            if (dia > 29)
            {
                inputFieldDD.image.color = Color.yellow;
                botonIniciarTerapia.interactable = false;
                return;
            }
        }
        if (mes < 1)
        {
            botonIniciarTerapia.interactable = false;
            return;
        }

        int anio = 0;

        try
        {
            anio = Int32.Parse(inputFieldAAA.text.ToString().Trim());
        }
        catch
        {
            botonIniciarTerapia.interactable = false;
            return;
        }

        if (anio < 1000)
        {
            botonIniciarTerapia.interactable = false;
            return;
        }

        if (anio < 1900 || anio > DateTime.Now.Year)
        {
            inputFieldAAA.image.color = Color.yellow;
            botonIniciarTerapia.interactable = false;
            return;
        }
    }

    public void iniciarTerapia()
    {
        Terapia terapia = new Terapia();
        terapia.psiquiatra = inputTextPsiquiatra.text.Trim();
        terapia.idPaciente = inputTextIDPaciente.text.Trim();
        terapia.genero = selectGenero.options[selectGenero.value].text;
        int mes = selectMes.value;
        terapia.fechaNacimiento = mes + "/" + inputFieldDD.text + "/" + inputFieldAAA.text;

        terapia.opcional1 = GameObject.Find("Opcional1").GetComponent<Toggle>().isOn;
        terapia.opcional2 = GameObject.Find("Opcional2").GetComponent<Toggle>().isOn;
        terapia.opcional3 = GameObject.Find("Opcional3").GetComponent<Toggle>().isOn;

        terapia.lugar = "Casa"; // Por defecto inicia en la casa

        GlobalControl.Instance.terapiaActual = terapia;
        GlobalControl.Instance.database.ultimoPsiquiatra = terapia.psiquiatra;

        this.pantallaConfiguracionTerapia.SetActive(true);
        this.transform.gameObject.SetActive(false);
    }
}
