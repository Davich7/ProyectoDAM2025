using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("Paneles")]
    [SerializeField] private PersonajeStats stats;

    [Header("Paneles")]
    [SerializeField] private GameObject panelStats;
    //Creamos un Singleton Pattern para el uso de instancias.
    [Header("Barra")]
    [SerializeField] private Image vidaPlayer;
    [SerializeField] private Image manaPlayer;
    [SerializeField] private Image expPlayer;

    [Header("Texto")]
    [SerializeField] private TextMeshProUGUI vidaTMP;
    [SerializeField] private TextMeshProUGUI manaTMP;
    [SerializeField] private TextMeshProUGUI expTMP;
    [SerializeField] private TextMeshProUGUI nivelTMP;


    //Obtenemos referencia de todos los valores del panel
    [Header("Stats")]
    //Referencia de textos
    [SerializeField] private TextMeshProUGUI statDa�oTMP;
    [SerializeField] private TextMeshProUGUI statDefensaTMP;
    [SerializeField] private TextMeshProUGUI statCriticoTMP;
    [SerializeField] private TextMeshProUGUI statBloqueoTMP;
    [SerializeField] private TextMeshProUGUI statVelocidadTMP;
    [SerializeField] private TextMeshProUGUI statNivelTMP;
    [SerializeField] private TextMeshProUGUI statExpTMP;
    [SerializeField] private TextMeshProUGUI statExpRequeridaTMP;
    [SerializeField] private TextMeshProUGUI atributoFuerzaTMP;
    [SerializeField] private TextMeshProUGUI atributoInteligenciaTMP;
    [SerializeField] private TextMeshProUGUI atributoDestrezaTMP;
    [SerializeField] private TextMeshProUGUI atributosDisponiblesTMP;

    private float vidaActual;
    private float vidaMax;
    
    private float manaActual;
    private float manaMax;

    private float expActual;
    private float expRequeridaNuevoNivel;

    private void Start()
    {
        
    }

    private void Update()
    {
        ActualizarUIPersonaje();
        ActualizarPanelStats();
    }
    
    private void ActualizarUIPersonaje() 
    { 
        /*Uso de interpolaci�n lineal para cambiar un valor a otro. La vida del jugador actual la modificamos y como fillAmount es un bolean, dividimos la
        vida actual entre la vida m�xima, el valor por el que hacemos la interpolaci�n es 10 multiplicado por Time.deltaTime*/
        vidaPlayer.fillAmount = Mathf.Lerp(vidaPlayer.fillAmount, 
            vidaActual / vidaMax, 10f * Time.deltaTime);
        //Actualizaci�n barra de mana.
        manaPlayer.fillAmount = Mathf.Lerp(manaPlayer.fillAmount,
            manaActual / manaMax, 10f * Time.deltaTime);
        //Actualizaci�n barra de experiencia.
        expPlayer.fillAmount = Mathf.Lerp(expPlayer.fillAmount,
            expActual / expRequeridaNuevoNivel, 10f * Time.deltaTime);

        //Modificaci�n del texto usando String interpolation. Vida Actual/Vida Maxima.
        vidaTMP.text = $"{vidaActual}/{vidaMax}";

        //Actualizacion texto mana
        manaTMP.text = $"{manaActual}/{manaMax}";

        //Actualizacion texto experiencia en decimales.
        expTMP.text = $"{((expActual/expRequeridaNuevoNivel) * 100):F2}%";
        //Actualizaci�n del nivel en la User Interface.
        nivelTMP.text = $"Nivel {stats.Nivel}";
    }

    //Metodo para actualizar  el panel.
    private void ActualizarPanelStats()
    {
        //Si el panel no est� activo, no hacemos nada.
        if (panelStats.activeSelf == false)
        {
            return;
        }
        statDa�oTMP.text = stats.Da�o.ToString();
        statDefensaTMP.text = stats.Defensa.ToString();
        //Usamos StringInterpolation.
        statCriticoTMP.text = $"{stats.PorcentajeCritico}%";
        statBloqueoTMP.text = $"{stats.PorcentajeBloqueo}%";
        statVelocidadTMP.text = stats.Velocidad.ToString();
        statNivelTMP.text = stats.Nivel.ToString();
        statExpTMP.text = stats.ExpActual.ToString();
        statExpRequeridaTMP.text = stats.ExpRequeridaSiguienteNivel.ToString();
        //Actualizamos datos del panel.

        atributoFuerzaTMP.text = stats.Fuerza.ToString();
        atributoInteligenciaTMP.text = stats.Inteligencia.ToString();
        atributoDestrezaTMP.text = stats.Destreza.ToString();
        atributosDisponiblesTMP.text = $"Puntos: {stats.PuntosDisponibles}";
    }

    public void ActualizarVidaPersonaje(float pVidaActual, float pVidaMax)
    {
        vidaActual = pVidaActual;
        vidaMax = pVidaMax;
    }

    public void ActualizarManaPersonaje(float pManaActual, float pManaMax)
    {
        manaActual = pManaActual;
        manaMax = pManaMax;
    }
    public void ActualizarExpPersonaje(float pExpActual, float pExpRequerida)
    {
        expActual = pExpActual;
        expRequeridaNuevoNivel = pExpRequerida;
    }
}
