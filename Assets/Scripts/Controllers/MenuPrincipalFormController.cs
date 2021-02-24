using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Schizohaptic;

public class MenuPrincipalFormController : MonoBehaviour {
    private InputField inputTextPsiquiatra;
    private InputField inputTextIDPaciente;
    private Dropdown selectGenero;

    // Fecha
    private Dropdown selectMes;
    private InputField inputFieldAAA;
    private InputField inputFieldDD;

    // Use this for initialization
    void Start () {
        this.inputTextPsiquiatra = GameObject.Find("InputFieldPsiquiatra").GetComponent<InputField>();
        this.inputTextIDPaciente = GameObject.Find("InputFieldPaciente").GetComponent<InputField>();
        this.selectGenero = GameObject.Find("DropdownGenero").GetComponent<Dropdown>();
        this.selectMes = GameObject.Find("DropdownMES").GetComponent<Dropdown>();
        this.inputFieldAAA = GameObject.Find("InputFieldAAAA").GetComponent<InputField>();
        this.inputFieldDD = GameObject.Find("InputFieldDD").GetComponent<InputField>();
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void iniciarTerapia()
    {
        
        // Debug.Log(inputTextPsiquiatra.text);
        // Debug.Log(inputTextIDPaciente.text);
        // Debug.Log(selectGenero.options[selectGenero.value].text);
        // Debug.Log(selectMes.value);
        // Debug.Log(inputFieldAAA.text);
        // Debug.Log(inputFieldDD.text);
        

        Terapia terapia = new Terapia();
        terapia.psiquiatra = inputTextPsiquiatra.text;
        terapia.idPaciente = inputTextIDPaciente.text;
        terapia.genero = selectGenero.options[selectGenero.value].text;
        int mes = selectMes.value + 1;
        terapia.fechaNacimiento = mes + "/" + inputFieldDD.text + "/" + inputFieldAAA.text;

        terapia.opcional1 = GameObject.Find("Opcional1").GetComponent<Toggle>().isOn;
        terapia.opcional2 = GameObject.Find("Opcional2").GetComponent<Toggle>().isOn;
        terapia.opcional3 = GameObject.Find("Opcional3").GetComponent<Toggle>().isOn;

        // GlobalControl.Instance.AddTerapia(terapia);
    }
}
