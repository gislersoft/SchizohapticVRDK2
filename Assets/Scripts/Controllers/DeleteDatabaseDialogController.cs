using UnityEngine;
using UnityEngine.UI;
using Schizohaptic;
using System.Collections;

public class DeleteDatabaseDialogController : MonoBehaviour {
    Text texto;
    Button botonSi;

	// Use this for initialization
	void Start () {
        texto = GameObject.Find("TextoDialogo").GetComponent<Text>();
        botonSi = GameObject.Find("BotonSi").GetComponent<Button>();
    }

    public void BorrarBaseDatos()
    {
        int totalTerapiasBorradas = 0;
        if (GlobalControl.Instance.database.terapias != null)
        {
            totalTerapiasBorradas = GlobalControl.Instance.database.terapias.Count;
        }
        GlobalControl.Instance.database = new BaseDatos();
        GlobalControl.Instance.SaveData();
        texto.text = "Se borraron "+ totalTerapiasBorradas + " terapias.\n¡Base de datos limpia!";
        botonSi.interactable = false;
    }

    public void AccionSecundaria()
    {
        texto.text = "¿Está seguro de borrar la información?\n(no se puede deshacer)";
        botonSi.interactable = true;
        this.transform.gameObject.SetActive(false);
    }
}
