using UnityEngine;
using Schizohaptic;

public class TurOnActuator : MonoBehaviour
{
    public string actuador = "";
    public string mano = "";
    private bool discontino = false;
    private bool touch = false;

    private HapticGlovesListener hapticListener;

    // Use this for initialization
    void Start()
    {
        hapticListener = GameObject.Find("HapticListener").GetComponent<HapticGlovesListener>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (!touch)
        {
            hapticListener.turnOnActuator(mano, actuador);
            if (discontino)
            {
                touch = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        touch = false;
    }
}
