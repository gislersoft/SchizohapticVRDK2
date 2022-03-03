using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PruebaHMDFormController : MonoBehaviour {
    GameObject panelPantallaMenuPrincipal;
    GameObject menu;
    GameObject escena;
    GameObject ojos;

    public float alturaDefecto = -1.12f;

    Slider sliderAltura;
    // Use this for initialization
    void Start () {
        panelPantallaMenuPrincipal = GameObject.Find("PanelPantallaMenuPrincipal");
        menu = GameObject.Find("Menu");
        escena = GameObject.Find("Escena");
        ojos = GameObject.Find("CenterEyeAnchor");
        sliderAltura = GameObject.Find("SliderAltura").GetComponent<Slider>();

        sliderAltura.onValueChanged.AddListener(delegate { cambioAltura(); });

    }
	
	// Update is called once per frame
	void Update () {
	
	}


    public void centrarEscena()
    {
        cambioAltura();
        escena.transform.forward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
    }

    public void cambioAltura()
    {
        float altura = this.alturaDefecto - sliderAltura.value;
        escena.transform.localPosition = ojos.transform.localPosition + new Vector3(0, altura, 0);
    }

    public void VolverAlMenuPrincipal()
    {
        panelPantallaMenuPrincipal.SetActive(true);
        menu.SetActive(true);
        this.transform.gameObject.SetActive(false);
    }
}
