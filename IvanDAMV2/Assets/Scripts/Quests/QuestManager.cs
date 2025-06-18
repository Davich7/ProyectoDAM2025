using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : Singleton<QuestManager>
{
    [Header("Personaje")]
    [SerializeField] private Personaje personaje;

    [Header("Quests")]
    [SerializeField] private Quest[] questDisponibles;

    [Header("Inspector Quests")]
    [SerializeField] private InspectorQuestDescripcion inspectorQuestPrefab;
    [SerializeField] private Transform inspectorQuestContenedor;

    [Header("Personaje Quests")]
    [SerializeField] private PersonajeQuestDescripcion personajeQuestPrefab;
    [SerializeField] private Transform personajeQuestContenedor;

    [Header("Panel Quest Completado")]
    //Referencia de QuestCompletado
    [SerializeField] private GameObject panelQuestCompletado;
    [SerializeField] private TextMeshProUGUI questNombre;
    [SerializeField] private TextMeshProUGUI questRecompensaOro;
    [SerializeField] private TextMeshProUGUI questRecompensaExp;
    [SerializeField] private TextMeshProUGUI questRecompensaItemCantidad;
    [SerializeField] private Image questRecompensaItemIcono;

    //Para llamar al m�todo MostrarQuestCompletado, hay que verificar que la misi�n completada existe.
    //private ya que solo lo estableceremos dentro de esta clase.
    public Quest QuestPorReclamar { get; private set; }
    private void Start()
    {
        CargarQuestEnInspector();
    }

    //Manualmente a�adimos progreso a las misiones de matar enemigos.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            A�adirProgreso("Mata10", 1);
            A�adirProgreso("Mata25", 1);
            A�adirProgreso("Mata50", 1);
        }
    }

    //M�todo que permita cargar misiones en el panel del inspector.
    private void CargarQuestEnInspector()
    {
        for (int i = 0; i < questDisponibles.Length; i++)
        {
            //Configuramos todas las tarjetas que a�adimos con cada misi�n.
            InspectorQuestDescripcion nuevoQuest = Instantiate(inspectorQuestPrefab, inspectorQuestContenedor);
            nuevoQuest.ConfigurarQuestUI(questDisponibles[i]);
        }
    }

    //Este m�todo requiere como par�metro la misi�n que tenemos que completar.
    private void A�adirQuestPorCompletar(Quest questPorCompletar)
    {
        //Instanciaremos el Prefab, lo guardaremos en una referencia de su mismo
        //tipo PersonajeQuestDescripcion para poder llamar al m�todo de
        //ConfigurarQuestUI. 

        PersonajeQuestDescripcion nuevoQuest = Instantiate(personajeQuestPrefab, personajeQuestContenedor);
        nuevoQuest.ConfigurarQuestUI(questPorCompletar);
    }

    public void A�adirQuest(Quest questPorCompletar)
    {
        A�adirQuestPorCompletar(questPorCompletar);
    }

    public void ReclamarRecompensa()
    {
        //Verificamos que tenemos una misi�n por reclamar.
        if (QuestPorReclamar == null)
        {
            return;
        }
        //A�adimos las monedas.
        MonedasManager.Instance.A�adirMonedas(QuestPorReclamar.RecompensaOro);
        //A�adimos la experiencia.
        personaje.PersonajeExperiencia.A�adirExperiencia(QuestPorReclamar.RecompensaExp);
        //A�adimos el item.
        Inventario.Instance.A�adirItem(QuestPorReclamar.RecompensaItem.Item, QuestPorReclamar.RecompensaItem.Cantidad);
        //Si apretamos el bot�n de recompensa, obtenemos las recompensas y tambi�n hay que desactivar el panel de QuestCompletado
        panelQuestCompletado.SetActive(false);
        //Reseteamos el valor ya que se ha reclamado.
        QuestPorReclamar = null;
    }

    //Para a�adir progreso a una misi�n se tiene que pasar como par�metro el nombre de la misi�n y la cantidad que se est� a�adiendo como progreso.
    //Buscamos si el quest con ID espec�fico existe, si existe la referencia la guardamos en questPorActualizar.
    public void A�adirProgreso(string questID, int cantidad)
    {
        Quest questPorActualizar = QuestExiste(questID);
        if (questPorActualizar.QuestAceptado)
        {
            questPorActualizar.A�adirProgreso(cantidad);
        }
    }

    //M�todo que regrese la referencia de la misi�n con el identificador questID.
    private Quest QuestExiste(string questID)
    {
        for (int i = 0; i < questDisponibles.Length; i++)
        {
            //Si alguna misi�n disponible, su ID es igual a este ID de la misi�n que estamos buscando, devolvemos su referencia.
            if (questDisponibles[i].ID == questID)
            {
                //Devolvemos la referencia de la misi�n que tiene el mismo identificador
                return questDisponibles[i];
            }
        }
        //Si no lo encontramos, devolvemos null.
        return null;
    }

    //M�todo que actualice todos los componentes al completar una misi�n.
    //Para poder mostrar una misi�n completada con la informaci�n de la misi�n, hay que pasar como argumento del m�todo el par�metro de tipo Quest llamado questCompletado.
    private void MostrarQuestCompletado(Quest questCompletado)
    {
        panelQuestCompletado.SetActive(true);
        questNombre.text = questCompletado.Nombre;
        questRecompensaOro.text = questCompletado.RecompensaOro.ToString();
        questRecompensaExp.text = questCompletado.RecompensaExp.ToString();
        questRecompensaItemCantidad.text = questCompletado.RecompensaItem.Cantidad.ToString();
        questRecompensaItemIcono.sprite = questCompletado.RecompensaItem.Item.Icono;
    }

    
    private void QuestCompletadoRespuesta(Quest questCompletado)
    {
        //Si la misi�n que ha sido completada existe, (tenemos su referencia dentro de la propiedad QuestPorReclamar) hay que mostrar el panel de QuestCompletado.
        QuestPorReclamar = QuestExiste(questCompletado.ID);
        if (QuestPorReclamar != null)
        {
            MostrarQuestCompletado(QuestPorReclamar);
        }
    }

  
    private void OnEnable()
    {
        for (int i = 0; i < questDisponibles.Length; i++)
        {
            questDisponibles[i].ResetQuest();
        }
        Quest.EventoQuestCompletado += QuestCompletadoRespuesta;
    }
    private void OnDisable()
    {
        Quest.EventoQuestCompletado += QuestCompletadoRespuesta;
    }
}