using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("Stats")]
    [SerializeField] private PersonajeStats stats;

    [Header("Paneles")]
    [SerializeField] private GameObject panelStats;
    [SerializeField] private GameObject panelTienda;
    [SerializeField] private GameObject panelCrafting;
    [SerializeField] private GameObject panelCraftingInfo;
    [SerializeField] private GameObject panelInventario;
    [SerializeField] private GameObject panelInspectorQuests;
    [SerializeField] private GameObject panelPersonajeQuests;
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
    [SerializeField] private TextMeshProUGUI monedasTMP;


    //Obtenemos referencia de todos los valores del panel
    [Header("Stats")]
    //Referencia de textos
    [SerializeField] private TextMeshProUGUI statDañoTMP;
    [SerializeField] private TextMeshProUGUI statDefensaTMP;
    [SerializeField] private TextMeshProUGUI statCriticoTMP;
    [SerializeField] private TextMeshProUGUI statBloqueoTMP;
    [SerializeField] private TextMeshProUGUI statVelocidadTMP;
    [SerializeField] private TextMeshProUGUI statNivelTMP;
    [SerializeField] private TextMeshProUGUI statExpTMP;
    [SerializeField] private TextMeshProUGUI statExpRequeridaTMP;
    [SerializeField] private TextMeshProUGUI statExpTotalTMP;
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
        /*Uso de interpolación lineal para cambiar un valor a otro. La vida del jugador actual la modificamos y como fillAmount es un bolean, dividimos la
        vida actual entre la vida máxima, el valor por el que hacemos la interpolación es 10 multiplicado por Time.deltaTime*/
        vidaPlayer.fillAmount = Mathf.Lerp(vidaPlayer.fillAmount, 
            vidaActual / vidaMax, 10f * Time.deltaTime);
        //Actualización barra de mana.
        manaPlayer.fillAmount = Mathf.Lerp(manaPlayer.fillAmount,
            manaActual / manaMax, 10f * Time.deltaTime);
        //Actualización barra de experiencia.
        expPlayer.fillAmount = Mathf.Lerp(expPlayer.fillAmount,
            expActual / expRequeridaNuevoNivel, 10f * Time.deltaTime);

        //Modificación del texto usando String interpolation. Vida Actual/Vida Maxima.
        vidaTMP.text = $"{vidaActual}/{vidaMax}";

        //Actualizacion texto mana
        manaTMP.text = $"{manaActual}/{manaMax}";

        //Actualizacion texto experiencia en decimales.
        expTMP.text = $"{((expActual/expRequeridaNuevoNivel) * 100):F2}%";
        //Actualización del nivel en la User Interface.
        nivelTMP.text = $"Nivel {stats.Nivel}";
        //Actualización del oro ganado en el personaje.
        monedasTMP.text = MonedasManager.Instance.MonedasTotales.ToString();
    }

    //Metodo para actualizar el panel.
    private void ActualizarPanelStats()
    {
        //Si el panel no está activo, no hacemos nada.
        if (panelStats.activeSelf == false)
        {
            return;
        }
        statDañoTMP.text = stats.Daño.ToString();
        statDefensaTMP.text = stats.Defensa.ToString();
        //Usamos StringInterpolation.
        statCriticoTMP.text = $"{stats.PorcentajeCritico}%";
        statBloqueoTMP.text = $"{stats.PorcentajeBloqueo}%";
        statVelocidadTMP.text = stats.Velocidad.ToString();
        statNivelTMP.text = stats.Nivel.ToString();
        statExpTMP.text = stats.ExpActual.ToString();
        statExpRequeridaTMP.text = stats.ExpRequeridaSiguienteNivel.ToString();
        statExpTotalTMP.text = stats.ExpTotal.ToString();
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

    #region Paneles

    /*Abrir y poder cerrar el panel.
    PanelStats vamos a activarlo o desactivarlo, panelStats.activeSelf devuelve true o false en función de si está activado o no. 
    Como es falso y lo estamos negando vendría a ser verdadero.*/
    public void AbrirCerrarPanelStats()
    {
        panelStats.SetActive(!panelStats.activeSelf);
    }

    public void AbrirCerrarPanelTienda()
    {
        panelTienda.SetActive(!panelTienda.activeSelf);
    }

    public void AbrirPanelCrafting()
    {
        panelCrafting.SetActive(true);
    }

    public void CerrarPanelCrafting()
    {
        panelCrafting.SetActive(false);
        AbrirCerrarPanelCraftingInformacion(false);
    }

    public void AbrirCerrarPanelCraftingInformacion(bool estado)
    {
        panelCraftingInfo.SetActive(estado);
    }

    public void AbrirCerrarPanelInventario()
    {
        panelInventario.SetActive(!panelInventario.activeSelf);
    }

    public void AbrirCerrarPanelPersonajeQuests()
    {
        panelPersonajeQuests.SetActive(!panelPersonajeQuests.activeSelf);
    }

    public void AbrirCerrarPanelInspectorQuests()
    {
        panelInspectorQuests.SetActive(!panelInspectorQuests.activeSelf);
    }

    /*Método que permita activar el panel correspondiente según el tipo de interacción del NPC.
    Para saber que panel abrir pasamos como parámetro InteracciónExtraNPC*/
    public void AbrirPanelInteraccion(InteraccionExtraNPC tipoInteraccion)
    {
        switch (tipoInteraccion)
        {
            case InteraccionExtraNPC.Quests:
                //Acción de abrir panel misiones.
                AbrirCerrarPanelInspectorQuests();
                break;
            case InteraccionExtraNPC.Tienda:
                AbrirCerrarPanelTienda();
                break;
            case InteraccionExtraNPC.Crafting:
                AbrirPanelCrafting();
                break;
        }
        
    }


    #endregion
}
