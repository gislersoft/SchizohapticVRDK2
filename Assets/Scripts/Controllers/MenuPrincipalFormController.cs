using UnityEngine;
using Schizohaptic;

public class MenuPrincipalFormController : MonoBehaviour {
    GameObject pantallaPruebaManos;
    HapticGlovesListener hapticGlovesListener;

	// Use this for initialization
	void Start () {
        pantallaPruebaManos = GameObject.Find("PantallaPruebaManos");
        pantallaPruebaManos.SetActive(false);
        hapticGlovesListener = GameObject.Find("HapticGlovesListener").GetComponent<HapticGlovesListener>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void IniciarTerapia()
    {

    }

    public void PruebaGuantesHaptica()
    {
        pantallaPruebaManos.SetActive(true);
        hapticGlovesListener.DetectHands();
    }

    public void PruebaCascoVR()
    {

    }

    public void About()
    {

    }
}
