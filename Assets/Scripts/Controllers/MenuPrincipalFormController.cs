using UnityEngine;
using Schizohaptic;

public class MenuPrincipalFormController : MonoBehaviour {
    GameObject pantallaPruebaManos;
    GameObject pantallaPruebHDM;
    GameObject panelPantallaMenuPrincipal;
    GameObject menu;

    HapticGlovesListener hapticGlovesListener;

	// Use this for initialization
	void Start () {
        pantallaPruebaManos = GameObject.Find("PantallaPruebaManos");
        pantallaPruebaManos.SetActive(false);
        pantallaPruebHDM = GameObject.Find("PantallaPruebaHMD");
        pantallaPruebHDM.SetActive(false);

        panelPantallaMenuPrincipal = GameObject.Find("PanelPantallaMenuPrincipal");
        menu = GameObject.Find("Menu");

        hapticGlovesListener = GameObject.Find("HapticGlovesListener").GetComponent<HapticGlovesListener>();
        GlobalControl.Instance.LoadData();

        hapticGlovesListener.portStringDerecha = GlobalControl.Instance.database.portDerecha;
        hapticGlovesListener.portStringIzquierda = GlobalControl.Instance.database.portIzquierda;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void IniciarTerapia()
    {

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
    }

    public void About()
    {

    }
}
