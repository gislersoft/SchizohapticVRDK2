using UnityEngine;
using Schizohaptic;
using System.Collections;

public class PruebaGuantesFormController : MonoBehaviour {
    HapticGlovesListener hapticGlovesListener;
    // Use this for initialization
    void Start () {
        hapticGlovesListener = GameObject.Find("HapticGlovesListener").GetComponent<HapticGlovesListener>();
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void AbrirManual()
    {
        string pathDoc = Application.streamingAssetsPath + "/Docs/ManualGuantes.pdf";
        Application.OpenURL("file:///"+ pathDoc);
    }

    public void VolverAlMenuPrincipal()
    {
        hapticGlovesListener.detectar = false;
        hapticGlovesListener.iniciarPrueba = false;
        this.transform.gameObject.SetActive(false);
    }
}
