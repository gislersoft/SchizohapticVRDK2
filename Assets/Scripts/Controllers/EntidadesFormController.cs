using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Schizohaptic;

public class EntidadesFormController : MonoBehaviour {
    public ArrayList entidades;
    public ArrayList gameObjectsEntidades;
    private static int ENTIDADES_MAXIMAS = 3;
    private Toggle[] toggles;
    private Button botonAceptar;

    GameObject mensajeError;

    // Use this for initialization
    void Start () {
        entidades = new ArrayList();

        gameObjectsEntidades = new ArrayList();
        for (int i = 0; i < ENTIDADES_MAXIMAS; i++)
        {
            GameObject reference = GameObject.Find("Entidad" + (i + 1));
            reference.transform.Find("Toggle").GetComponent<Toggle>().isOn = true;
            gameObjectsEntidades.Add(reference);
            reference.SetActive(false);
        }

        toggles = GameObject.Find("Content").GetComponentsInChildren<Toggle>();
        botonAceptar = GameObject.Find("BotonAceptarEntidades").GetComponent<Button>();
        mensajeError = GameObject.Find("MensajeError");
        mensajeError.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        botonAceptar.interactable = false;
        for (int i = 0; i < toggles.Length; i++)
        {
            if (entidades.Count >= ENTIDADES_MAXIMAS) {
                if (!toggles[i].isOn) {
                    toggles[i].interactable = false;
                }
                botonAceptar.interactable = true;
            } else
            {
                toggles[i].interactable = true;
                botonAceptar.interactable = false;
            }
        }
  
        for (int i = 0; i < ENTIDADES_MAXIMAS; i++)
        {
            GameObject entidadRef = (GameObject) gameObjectsEntidades[i];
            Text texto = entidadRef.transform.Find("Text").GetComponent<Text>();
            Image image = entidadRef.GetComponent<Image>();
            if (i < entidades.Count)
            {
                string imagenName = entidades[i].ToString();
                imagenName = imagenName.Replace("ú", "u");
                imagenName = imagenName.Replace("ñ", "n");
                Sprite imageToLoad = Resources.Load<Sprite>("Entidades/Sprites/"+ imagenName);
                if (imageToLoad == null)
                {
                    imageToLoad = Resources.Load<Sprite>("Entidades/Sprites/Vacio");
                }
 
                image.sprite = imageToLoad;

                texto.text = entidades[i].ToString();
                entidadRef.SetActive(true);
            } else
            {
                texto.text = "";
                entidadRef.SetActive(false);
            }
        }

        if (entidades.Count == ENTIDADES_MAXIMAS)
        {
            // Verificamos que no todas las entidades seleccionadas sean reales o todas no reales.
            bool checkAllTrue = true;
            bool checkAllFalse = true;
            for (int i = 0; i < ENTIDADES_MAXIMAS; i++)
            {

                GameObject entidadRef = (GameObject)gameObjectsEntidades[i];
                if (!entidadRef.transform.Find("Toggle").GetComponent<Toggle>().isOn)
                {
                    checkAllTrue = false;
                    break;
                }
            }
            for (int i = 0; i < ENTIDADES_MAXIMAS; i++)
            {

                GameObject entidadRef = (GameObject)gameObjectsEntidades[i];
                if (entidadRef.transform.Find("Toggle").GetComponent<Toggle>().isOn)
                {
                    checkAllFalse = false;
                    break;
                }
            }
            if (checkAllTrue || checkAllFalse)
            {
                mensajeError.SetActive(true);
                botonAceptar.interactable = false;
            }
            else
            {
                mensajeError.SetActive(false);
            }
        }
    }

    public void recordarEntidadesSeleccionadas()
    {
        entidades = new ArrayList();
        if (GlobalControl.Instance.terapiaActual != null)
        {
            Entidad[] entidadesSeleccionadas = GlobalControl.Instance.terapiaActual.entidades;
            if (entidadesSeleccionadas != null)
            {
                for (int i = 0; i < entidadesSeleccionadas.Length; i++)
                {
                    if (entidadesSeleccionadas[i] != null)
                    {
                        entidades.Add(entidadesSeleccionadas[i].nombre);

                        GameObject entidadRef = (GameObject)gameObjectsEntidades[i];
                        entidadRef.transform.Find("Toggle").GetComponent<Toggle>().isOn = entidadesSeleccionadas[i].real;
                    }
                }
            }
        }
    }

    public void seleccionarEntidad(Toggle entidadToggle)
    {
        Text texto = entidadToggle.transform.Find("Label").GetComponent<Text>();
        if (entidadToggle.isOn)
        {
            if (entidades.Count < ENTIDADES_MAXIMAS)
            {
                entidades.Add(texto.text.ToString().Trim());
            }
        } else
        {
            for (int i = 0; i < entidades.Count; i++)
            {
                if (entidades[i].ToString().Trim() == texto.text.ToString().Trim())
                {
                    entidades.RemoveAt(i);
                }
            }
        }
    }

    public Entidad[] getEntidadesSeleccionadas()
    {
        Entidad[] entidadesSeleccionadas = new Entidad[ENTIDADES_MAXIMAS];

        for (int i = 0; i < ENTIDADES_MAXIMAS; i++)
        {
            entidadesSeleccionadas[i] = new Entidad();

            GameObject entidadRef = (GameObject)gameObjectsEntidades[i];
            Toggle toggle = entidadRef.transform.Find("Toggle").GetComponent<Toggle>();

            entidadesSeleccionadas[i].nombre = entidades[i].ToString();
            entidadesSeleccionadas[i].real = toggle.isOn;
        }
        return entidadesSeleccionadas;
    }

    public void volverAConfiguracion()
    {
        Entidad[] entidadesSeleccionadas = this.getEntidadesSeleccionadas();
        GlobalControl.Instance.actualizarEntidadesTerapia(entidadesSeleccionadas);
        GlobalControl.Instance.terapiaActual.entidadesReales = GlobalControl.Instance.GetEntidadesPorComa(GlobalControl.Instance.terapiaActual.entidades, true);
        GlobalControl.Instance.terapiaActual.entidadesNoReales = GlobalControl.Instance.GetEntidadesPorComa(GlobalControl.Instance.terapiaActual.entidades, false);
        this.transform.gameObject.SetActive(false);
    }
}
