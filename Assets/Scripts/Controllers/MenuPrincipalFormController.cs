using UnityEngine;
using Schizohaptic;
using UnityEngine.SceneManagement;

public class MenuPrincipalFormController : MonoBehaviour {
    GameObject pantallaPruebaManos;
    GameObject pantallaPruebHDM;
    GameObject pantallaBaseDeDatos;
    GameObject pantallaSobreSoftware;

    GameObject panelPantallaMenuPrincipal;
    GameObject menu;

    HapticGlovesListener hapticGlovesListener;

    bool desactivarPantallaBD = true;

	// Use this for initialization
	void Start () {
        pantallaPruebaManos = GameObject.Find("PantallaPruebaManos");
        pantallaPruebaManos.SetActive(false);
        pantallaPruebHDM = GameObject.Find("PantallaPruebaHMD");
        pantallaPruebHDM.SetActive(false);
        pantallaSobreSoftware = GameObject.Find("PantallaSobreSoftware");
        pantallaSobreSoftware.SetActive(false);
        pantallaBaseDeDatos = GameObject.Find("PantallaBaseDeDatos");

        panelPantallaMenuPrincipal = GameObject.Find("PanelPantallaMenuPrincipal");
        menu = GameObject.Find("Menu");

        hapticGlovesListener = GameObject.Find("HapticGlovesListener").GetComponent<HapticGlovesListener>();
        GlobalControl.Instance.LoadData();

        hapticGlovesListener.portStringDerecha = GlobalControl.Instance.database.portDerecha;
        hapticGlovesListener.portStringIzquierda = GlobalControl.Instance.database.portIzquierda;
    }
	
	// Update is called once per frame
	void Update () {
        // Desactivar en Update Porque la pantalla tiene otras pantallas dependientes.
        if (desactivarPantallaBD)
        {
            pantallaBaseDeDatos.SetActive(false);
            desactivarPantallaBD = false;
        }
	}

    public void IniciarTerapia()
    {
        SceneManager.LoadScene("Terapia");
    }

    public void PruebaGuantesHaptica()
    {
        hapticGlovesListener.modoPruebas = true;
        pantallaPruebaManos.SetActive(true);
        hapticGlovesListener.DetectHands();
    }

    public void PruebaCascoVR()
    {
        hapticGlovesListener.modoPruebas = false; // Desactive modo pruebas de guantes
        panelPantallaMenuPrincipal.SetActive(false);
        menu.SetActive(false);
        pantallaPruebHDM.SetActive(true);
        pantallaPruebHDM.GetComponent<PruebaHMDFormController>().centrarEscena();
    }

    public void BaseDeDatos()
    {
        pantallaBaseDeDatos.SetActive(true);
    }

    public void About()
    {
        pantallaSobreSoftware.SetActive(true);
    }

    public void Salir()
    {
        Application.Quit();
    }
}
