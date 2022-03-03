using UnityEngine;
using Schizohaptic;

public class LugaresFormController : MonoBehaviour {
    private GameObject casaRef;
	// Use this for initialization
	void Start () {
        this.casaRef = GameObject.Find("CASA");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void seleccionarCasa()
    {
        GlobalControl.Instance.terapiaActual.lugar = "Casa";
        this.casaRef.SetActive(true);
        this.transform.gameObject.SetActive(false);
    }

    public void seleccionarAireLibre()
    {
        GlobalControl.Instance.terapiaActual.lugar = "Aire Libre";
        this.casaRef.SetActive(false);
        this.transform.gameObject.SetActive(false);
    }
}
