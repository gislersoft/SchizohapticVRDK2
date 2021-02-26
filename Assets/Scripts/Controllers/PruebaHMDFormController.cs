using UnityEngine;
using System.Collections;

public class PruebaHMDFormController : MonoBehaviour {
    GameObject panelPantallaMenuPrincipal;
    GameObject menu;
	// Use this for initialization
	void Start () {
        panelPantallaMenuPrincipal = GameObject.Find("PanelPantallaMenuPrincipal");
        menu = GameObject.Find("Menu");
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void VolverAlMenuPrincipal()
    {
        panelPantallaMenuPrincipal.SetActive(true);
        menu.SetActive(true);
        this.transform.gameObject.SetActive(false);
    }
}
