using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Schizohaptic;
using System;

public class TerapiaEnCursoFormController : MonoBehaviour {
    GameObject panelFade;

    GameObject mesa;
    GameObject miniaturas;
    GameObject entidadesTamReal;

    GameObject pantallaNotasTerapia;

    public GameObject escena;
    public GameObject ojos;

    Toggle toggleMiniaturas;

    private bool modoMiniaturas = true;


    public AudioSource audioSource;

    public AudioClip minimo;
    public AudioClip ligero;
    public AudioClip extremo;
    public AudioClip severo;

    private ArrayList refEntidadesReales;
    private ArrayList refEntidadesNoReales;

    private ArrayList refEntidadesRealesMini;
    private ArrayList refEntidadesNoRealesMini;


    private int frames = 0;
    private int countFrames = 300;
    private int modo = 0;
    private bool mostrarEntidadesNoReales = true;

    private float alturaDefecto = 0.3f;

    Text textoPanelMensajes;

    Slider sliderAltura;

    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();

        panelFade = GameObject.Find("PanelFade");
        mesa = GameObject.Find("Mesa");
        miniaturas = GameObject.Find("Miniaturas");
        entidadesTamReal = GameObject.Find("EntidadesTamanioReal");
        toggleMiniaturas = GameObject.Find("ToggleMiniaturas").GetComponent<Toggle>();
        toggleMiniaturas.isOn = modoMiniaturas;
        pantallaNotasTerapia = GameObject.Find("PantallaNotasTerapia");
        pantallaNotasTerapia.SetActive(false);
        this.escena = GameObject.Find("Escena");
        this.ojos = GameObject.Find("CenterEyeAnchor");


        textoPanelMensajes = GameObject.Find("TextoPanelMensajes").GetComponent<Text>();

        sliderAltura = GameObject.Find("SliderAltura").GetComponent<Slider>();

        sliderAltura.onValueChanged.AddListener(delegate { cambioAltura(); });
    }

    public void cargarEntidadMiniatura(int posicion, string nombreEntidad, bool esReal)
    {
        nombreEntidad = nombreEntidad.Replace("ú", "u");
        nombreEntidad = nombreEntidad.Replace("ñ", "n");
        string pathToPrefab = "Entidades/Miniaturas/" + nombreEntidad + "Mini";
        GameObject entidadPrefab = Resources.Load<GameObject>(pathToPrefab);
        if (entidadPrefab != null)
        {
            Vector3 originalLocalPosition = entidadPrefab.transform.localPosition;
            Quaternion originalLocalRotation = entidadPrefab.transform.localRotation;
            GameObject entidadObjeto = Instantiate(entidadPrefab) as GameObject;
            switch (posicion)
            {
                case 0:
                    GameObject izquierda = GameObject.Find("PosIzquierda");
                    entidadObjeto.transform.SetParent(izquierda.transform);
                    break;
                case 1:
                    GameObject centro = GameObject.Find("PosCentro");
                    entidadObjeto.transform.SetParent(centro.transform);
                    break;
                case 2:
                    GameObject derecha = GameObject.Find("PosDerecha");
                    entidadObjeto.transform.SetParent(derecha.transform);
                    break;
            }
            entidadObjeto.transform.localPosition = originalLocalPosition;
            entidadObjeto.transform.localRotation = originalLocalRotation;
            this.activarDesactivarColliders(entidadObjeto, esReal);
            if (esReal)
            {
                this.refEntidadesRealesMini.Add(entidadObjeto);
            }
            else
            {
                this.refEntidadesNoRealesMini.Add(entidadObjeto);
            }
        }
        else
        {
            Debug.LogError("No existe el prefab " + pathToPrefab);
        }
    }

    public void cargarEntidadGrande(int posicion, string nombreEntidad, bool esReal)
    {
        nombreEntidad = nombreEntidad.Replace("ú", "u");
        nombreEntidad = nombreEntidad.Replace("ñ", "n");
        string pathToPrefab = "Entidades/Grandes/" + nombreEntidad + "Grande";
        GameObject entidadPrefab = Resources.Load<GameObject>(pathToPrefab);
        if (entidadPrefab != null)
        {
            Vector3 originalLocalPosition = entidadPrefab.transform.localPosition;
            Quaternion originalLocalRotation = entidadPrefab.transform.localRotation;
            GameObject entidadObjeto = Instantiate(entidadPrefab) as GameObject;
            switch (posicion)
            {
                case 0:
                    GameObject izquierda = GameObject.Find("PosIzquierdaGrande");
                    entidadObjeto.transform.SetParent(izquierda.transform);
                    break;
                case 1:
                    GameObject centro = GameObject.Find("PosCentroGrande");
                    entidadObjeto.transform.SetParent(centro.transform);
                    break;
                case 2:
                    GameObject derecha = GameObject.Find("PosDerechaGrande");
                    entidadObjeto.transform.SetParent(derecha.transform);
                    break;
            }
            entidadObjeto.transform.localPosition = originalLocalPosition;
            entidadObjeto.transform.localRotation = originalLocalRotation;
 
            // No rote la Vaca o el Dragon son muy grandes.
            if (nombreEntidad != "Vaca" && nombreEntidad != "Dragon") {
                switch (posicion)
                {
                    case 0:
                        entidadObjeto.transform.localEulerAngles = new Vector3(
                            entidadObjeto.transform.localEulerAngles.x,
                            entidadObjeto.transform.localEulerAngles.y - 45,
                            entidadObjeto.transform.localEulerAngles.z
                        );
                        break;
                    case 2:
                        entidadObjeto.transform.localEulerAngles = new Vector3(
                            entidadObjeto.transform.localEulerAngles.x,
                            entidadObjeto.transform.localEulerAngles.y + 45,
                            entidadObjeto.transform.localEulerAngles.z
                        );
                        break;
                }
            }
 
            this.activarDesactivarColliders(entidadObjeto, esReal);
            if (esReal)
            {
                this.refEntidadesReales.Add(entidadObjeto);
            } else
            {
                this.refEntidadesNoReales.Add(entidadObjeto);
            }
        }
        else
        {
            Debug.LogError("No existe el prefab " + pathToPrefab);
        }
    }

    public void activarDesactivarColliders(GameObject entidadObjeto, bool esReal)
    {
        Collider[] colliders = entidadObjeto.GetComponentsInChildren<Collider>();
        if (colliders != null && colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].isTrigger = true; // Todos los colliders que tenga se vuelven triggers.
                colliders[i].enabled = esReal;
            }
        } else
        {
            Debug.LogError("No se encontraron colliders para "+ entidadObjeto.name + " verfique el prefab.");
        }
    }

    public void cargarEntidades()
    {
        this.refEntidadesReales = new ArrayList();
        this.refEntidadesNoReales = new ArrayList();
        this.refEntidadesRealesMini = new ArrayList();
        this.refEntidadesNoRealesMini = new ArrayList();
        Entidad[] entidades = GlobalControl.Instance.terapiaActual.entidades;
        for (int i = 0; i < entidades.Length; i++)
        {
            this.cargarEntidadGrande(i, entidades[i].nombre, entidades[i].real);
            this.cargarEntidadMiniatura(i, entidades[i].nombre, entidades[i].real);
        }

        if (modoMiniaturas)
        {
            entidadesTamReal.SetActive(false);
            miniaturas.SetActive(true);
        } else
        {
            entidadesTamReal.SetActive(true);
            miniaturas.SetActive(false);
        }
    }

    public void cargarAudiosVoces()
    {
        switch(GlobalControl.Instance.terapiaActual.vocesDropdown)
        {
            case 0:
                this.minimo = Resources.Load<AudioClip>("Voces/Desconocidos/minimo");
                this.ligero = Resources.Load<AudioClip>("Voces/Desconocidos/ligero");
                this.extremo = Resources.Load<AudioClip>("Voces/Desconocidos/extremo");
                this.severo = Resources.Load<AudioClip>("Voces/Desconocidos/severo");
                break;
            case 1:
                this.minimo = Resources.Load<AudioClip>("Voces/Religion/minimo");
                this.ligero = Resources.Load<AudioClip>("Voces/Religion/ligero");
                this.extremo = Resources.Load<AudioClip>("Voces/Religion/extremo");
                this.severo = Resources.Load<AudioClip>("Voces/Religion/severo");
                break;
            case 2:
                this.minimo = Resources.Load<AudioClip>("Voces/Extraterrestres/minimo");
                this.ligero = Resources.Load<AudioClip>("Voces/Extraterrestres/ligero");
                this.extremo = Resources.Load<AudioClip>("Voces/Extraterrestres/extremo");
                this.severo = Resources.Load<AudioClip>("Voces/Extraterrestres/severo");
                break;
            case 3:
                this.minimo = Resources.Load<AudioClip>("Voces/Armados/minimo");
                this.ligero = Resources.Load<AudioClip>("Voces/Armados/ligero");
                this.extremo = Resources.Load<AudioClip>("Voces/Armados/extremo");
                this.severo = Resources.Load<AudioClip>("Voces/Armados/severo");
                break;
            case 4:
                this.minimo = Resources.Load<AudioClip>("Voces/Deudas/minimo");
                this.ligero = Resources.Load<AudioClip>("Voces/Deudas/ligero");
                this.extremo = Resources.Load<AudioClip>("Voces/Deudas/extremo");
                this.severo = Resources.Load<AudioClip>("Voces/Deudas/severo");
                break;
            default:
                break;
        }
    }

    public void desactivarActivarEntidades(ArrayList entidadesList, bool activar)
    {
        for (int i = 0; i < entidadesList.Count; i++)
        {
            GameObject gameObject = (GameObject) entidadesList[i];
            if (gameObject != null)
            {
                gameObject.SetActive(activar);
            }
        }
    }

    public void desactivarEntidadesNoReales()
    {
        if (modoMiniaturas)
        {
            desactivarActivarEntidades(this.refEntidadesNoRealesMini, false);
        }
        else
        {
            desactivarActivarEntidades(this.refEntidadesNoReales, false);
        }
    }

    public void activarEntidadesNoReales()
    {
        if (modoMiniaturas)
        {
            desactivarActivarEntidades(this.refEntidadesNoRealesMini, true);
        }
        else
        {
            desactivarActivarEntidades(this.refEntidadesNoReales, true);
        }
    }

    // Update is called once per frame
    void Update () {
        switch (modo)
        {
            case 0:
                // Ausente: Paciente no presenta alucinaciones de ningún tipo.
                desactivarEntidadesNoReales();
                textoPanelMensajes.text = "Ausente: Solo las entidades que son reales y tangibles están presentes. No existen alucinaciones auditivas (Persona Normal).";
                break;
            case 1:
                desactivarEntidadesNoReales();
                if (!audioSource.isPlaying)
                    audioSource.PlayOneShot(minimo, 1.0f);
                textoPanelMensajes.text = "Mínimo: Solo las entidades que son reales y tangibles están presentes. Alucinaciones auditivas leves.";
                break;
            case 2:
                desactivarEntidadesNoReales();
                if (!audioSource.isPlaying)
                    audioSource.PlayOneShot(ligero, 1.0f);
                textoPanelMensajes.text = "Ligero: Solo las entidades que son reales y tangibles están presentes. Alucinaciones auditivas.";
                break;
            case 3:
                frames = frames + 1;
                if (frames > countFrames)
                {
                    mostrarEntidadesNoReales = !mostrarEntidadesNoReales;
                    frames = 0;
                }
                if (!audioSource.isPlaying && mostrarEntidadesNoReales)
                    audioSource.PlayOneShot(ligero, 1.0f);
                if (mostrarEntidadesNoReales)
                {
                    activarEntidadesNoReales();
                } else
                {
                    desactivarEntidadesNoReales();
                }
                textoPanelMensajes.text = "Moderado: Las entidades no reales aparecen de vez en cuando. Alucinaciones auditivas moderadas.";
                break;
            case 4:
                activarEntidadesNoReales();
                if (!audioSource.isPlaying)
                    audioSource.PlayOneShot(ligero, 1.0f);
                textoPanelMensajes.text = "Moderado Severo: Las entidades no reales aparecen y son constantes. Alucinaciones auditivas moderadas.";
                break;
            case 5:
                activarEntidadesNoReales();
                if (!audioSource.isPlaying)
                    audioSource.PlayOneShot(severo, 1.0f);
                textoPanelMensajes.text = "Severo: Las entidades no reales son constantes. Alucinaciones auditivas severas.";
                break;
            case 6:
                activarEntidadesNoReales();
                if (!audioSource.isPlaying)
                    audioSource.PlayOneShot(extremo, 1.0f);
                textoPanelMensajes.text = "Extremo: Las entidades no reales parecen reales para el paciente. Alucinaciones auditivas extremas, paciente totalmente distraido por alucionaciones.";
                break;
            default:
                break;
        }
    }

    public void DropdownChangedComportamientoAlucinatorio(Dropdown change)
    {
        modo = change.value;
        audioSource.Stop();
    }

    public void silenciarVoces(Toggle change)
    {
        audioSource.mute = change.isOn;
    }

    public void activarModoConfrontamiento(Toggle change)
    {
        modoMiniaturas = change.isOn;
        toggleMiniaturas.interactable = false;
        this.fadeIn();
    }

    public void fadeIn()
    {
        StartCoroutine(FadeTo(1.0f, 3.0f, true));
    }

    public void fadeOut()
    {
        StartCoroutine(FadeTo(0.0f, 3.0f, false));
    }

    IEnumerator FadeTo(float aValue, float aTime, bool cambieModo)
    {
        float alpha = panelFade.GetComponent<Image>().color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            panelFade.GetComponent<Image>().color = newColor;
            yield return null;
        }
        if (cambieModo)
        {
            this.cambiarModo();
            this.fadeOut();
        } else
        {
            toggleMiniaturas.interactable = true;
        }
    }

    public void cambiarModo()
    {
        if (!modoMiniaturas)
        {
            mesa.SetActive(false);
            miniaturas.SetActive(false);
            entidadesTamReal.SetActive(true);
           
        } else
        {
            mesa.SetActive(true);
            miniaturas.SetActive(true);
            entidadesTamReal.SetActive(false);
        }
    }

    public void terminarTerapia()
    {
        audioSource.mute = true;
        GameObject.Find("LMHeadMountedRig").GetComponent<AudioSource>().mute=true;
        GlobalControl.Instance.terapiaActual.horaFin = System.DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
        this.pantallaNotasTerapia.SetActive(true);
    }

    public void centrar()
    {
        cambioAltura();
        escena.transform.forward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
    }

    public void cambioAltura()
    {
        float altura = this.alturaDefecto - sliderAltura.value;
        escena.transform.localPosition = ojos.transform.localPosition + new Vector3(0, altura, 0);
    }
}
