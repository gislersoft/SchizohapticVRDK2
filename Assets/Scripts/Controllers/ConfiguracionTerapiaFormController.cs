using UnityEngine;
using UnityEngine.UI;
using Schizohaptic;

public class ConfiguracionTerapiaFormController : MonoBehaviour
{
    public GameObject refPantallaConfiguracionLugar;
    public GameObject refPantallaEntidades;
    public GameObject refPantallaTerapiaEnCurso;

    public Button botonIniciar;

    public Dropdown selectorVoces;



    // Use this for initialization
    void Start()
    {
        this.refPantallaConfiguracionLugar = GameObject.Find("PantallaLugares");
        this.refPantallaConfiguracionLugar.SetActive(false);
        this.refPantallaEntidades = GameObject.Find("PantallaEntidades");
        this.refPantallaEntidades.SetActive(false);
        this.refPantallaTerapiaEnCurso = GameObject.Find("PantallaTerapiaEnCurso");
        this.refPantallaTerapiaEnCurso.SetActive(false);

        this.selectorVoces = GameObject.Find("DropdownVoces").GetComponent<Dropdown>();

        this.botonIniciar = GameObject.Find("BotonIniciar").GetComponent<Button>();
        GameObject.Find("LMHeadMountedRig").GetComponent<AudioSource>().mute = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalControl.Instance.terapiaActual == null || GlobalControl.Instance.terapiaActual.entidades == null || GlobalControl.Instance.terapiaActual.entidades.Length == 0)
        {
            this.botonIniciar.interactable = false;
        } else
        {
            this.botonIniciar.interactable = true;
        }
    }

    public void configurarLugar()
    {
        this.refPantallaConfiguracionLugar.SetActive(true);
    }

    public void configurarEntidades()
    {
        this.refPantallaEntidades.SetActive(true);
        this.refPantallaEntidades.GetComponent<EntidadesFormController>().recordarEntidadesSeleccionadas();
    }

    public void iniciarTerapia()
    {
        GameObject.Find("LMHeadMountedRig").GetComponent<AudioSource>().mute = false;
        GlobalControl.Instance.terapiaActual.vocesDropdown = selectorVoces.value;
        GlobalControl.Instance.terapiaActual.voces = selectorVoces.options[selectorVoces.value].text;
        this.refPantallaTerapiaEnCurso.SetActive(true);
        this.refPantallaTerapiaEnCurso.GetComponent<TerapiaEnCursoFormController>().cargarAudiosVoces();
        this.refPantallaTerapiaEnCurso.GetComponent<TerapiaEnCursoFormController>().cargarEntidades();
        this.refPantallaTerapiaEnCurso.GetComponent<TerapiaEnCursoFormController>().centrar();
        GlobalControl.Instance.terapiaActual.horaInicio = System.DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");

        
        this.transform.gameObject.SetActive(false);
    }

    public void iniciarTerapiaPorDefecto()
    {
        Entidad[] entidadesPorDefecto = new Entidad[3];
        entidadesPorDefecto[0] = new Entidad();
        entidadesPorDefecto[0].nombre = "Mujer";
        entidadesPorDefecto[0].real = true;
        entidadesPorDefecto[1] = new Entidad();
        entidadesPorDefecto[1].nombre = "Alien";
        entidadesPorDefecto[1].real = false;
        entidadesPorDefecto[2] = new Entidad();
        entidadesPorDefecto[2].nombre = "Perro";
        entidadesPorDefecto[2].real = true;

        GlobalControl.Instance.actualizarEntidadesTerapia(entidadesPorDefecto);
        GlobalControl.Instance.terapiaActual.entidadesReales = GlobalControl.Instance.GetEntidadesPorComa(GlobalControl.Instance.terapiaActual.entidades, true);
        GlobalControl.Instance.terapiaActual.entidadesNoReales = GlobalControl.Instance.GetEntidadesPorComa(GlobalControl.Instance.terapiaActual.entidades, false);
  
        this.iniciarTerapia();
    }
}
