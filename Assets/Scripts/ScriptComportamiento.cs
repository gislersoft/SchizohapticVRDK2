using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptComportamiento : MonoBehaviour
{
    public GameObject referenciaAlien;
    private int frames = 0;
    private int countFrames = 300;
    private int modo = 0;
    private bool mostrarAlien = true;
    private AudioSource audioSource;
    public AudioClip minimo;
    public AudioClip ligero;
    public AudioClip extremo;
    public AudioClip severo;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (modo)
        {
            case 0:  referenciaAlien.SetActive(false);
                break;
            case 1:  referenciaAlien.SetActive(false);
                if (!audioSource.isPlaying)
                    audioSource.PlayOneShot(minimo, 1.0f);
                break;
            case 2:  referenciaAlien.SetActive(false);
                if (!audioSource.isPlaying)
                    audioSource.PlayOneShot(ligero, 1.0f);
                break;
            case 3:  frames = frames + 1;
                     if (frames > countFrames) {
                         mostrarAlien = !mostrarAlien;
                         frames = 0;
                     }
                     if (!audioSource.isPlaying && mostrarAlien)
                            audioSource.PlayOneShot(ligero, 1.0f);
                     referenciaAlien.SetActive(mostrarAlien);
                break;
            case 4:  referenciaAlien.SetActive(true);
                if (!audioSource.isPlaying)
                    audioSource.PlayOneShot(ligero, 1.0f);
                break;
            case 5:  referenciaAlien.SetActive(true);
                if (!audioSource.isPlaying)
                    audioSource.PlayOneShot(severo, 1.0f);
                break;
            case 6:  referenciaAlien.SetActive(true);
                if (!audioSource.isPlaying)
                    audioSource.PlayOneShot(extremo, 1.0f);
                break;
            default:
                 break;
        }
    }

     //Ouput the new value of the Dropdown into Text
    public void DropdownValueChanged(Dropdown change)
    {
        modo = change.value;
        audioSource.Stop();
    }
}
