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

    //Para llamar al método MostrarQuestCompletado, hay que verificar que la misión completada existe.
    //private ya que solo lo estableceremos dentro de esta clase.
    public Quest QuestPorReclamar { get; private set; }
    private void Start()
    {
        CargarQuestEnInspector();
    }

    //Manualmente añadimos progreso a las misiones de matar enemigos.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            AñadirProgreso("Mata10", 1);
            AñadirProgreso("Mata25", 1);
            AñadirProgreso("Mata50", 1);
        }
    }

    //Método que permita cargar misiones en el panel del inspector.
    private void CargarQuestEnInspector()
    {
        for (int i = 0; i < questDisponibles.Length; i++)
        {
            //Configuramos todas las tarjetas que añadimos con cada misión.
            InspectorQuestDescripcion nuevoQuest = Instantiate(inspectorQuestPrefab, inspectorQuestContenedor);
            nuevoQuest.ConfigurarQuestUI(questDisponibles[i]);
        }
    }

    //Este método requiere como parámetro la misión que tenemos que completar.
    private void AñadirQuestPorCompletar(Quest questPorCompletar)
    {
        //Instanciaremos el Prefab, lo guardaremos en una referencia de su mismo
        //tipo PersonajeQuestDescripcion para poder llamar al método de
        //ConfigurarQuestUI. 

        PersonajeQuestDescripcion nuevoQuest = Instantiate(personajeQuestPrefab, personajeQuestContenedor);
        nuevoQuest.ConfigurarQuestUI(questPorCompletar);
    }

    public void AñadirQuest(Quest questPorCompletar)
    {
        AñadirQuestPorCompletar(questPorCompletar);
    }

    public void ReclamarRecompensa()
    {
        //Verificamos que tenemos una misión por reclamar.
        if (QuestPorReclamar == null)
        {
            return;
        }
        //Añadimos las monedas.
        MonedasManager.Instance.AñadirMonedas(QuestPorReclamar.RecompensaOro);
        //Añadimos la experiencia.
        personaje.PersonajeExperiencia.AñadirExperiencia(QuestPorReclamar.RecompensaExp);
        //Añadimos el item.
        Inventario.Instance.AñadirItem(QuestPorReclamar.RecompensaItem.Item, QuestPorReclamar.RecompensaItem.Cantidad);
        //Si apretamos el botón de recompensa, obtenemos las recompensas y también hay que desactivar el panel de QuestCompletado
        panelQuestCompletado.SetActive(false);
        //Reseteamos el valor ya que se ha reclamado.
        QuestPorReclamar = null;
    }

    //Para añadir progreso a una misión se tiene que pasar como parámetro el nombre de la misión y la cantidad que se está añadiendo como progreso.
    //Buscamos si el quest con ID específico existe, si existe la referencia la guardamos en questPorActualizar.
    public void AñadirProgreso(string questID, int cantidad)
    {
        Quest questPorActualizar = QuestExiste(questID);
        if (questPorActualizar.QuestAceptado)
        {
            questPorActualizar.AñadirProgreso(cantidad);
        }
    }

    //Método que regrese la referencia de la misión con el identificador questID.
    private Quest QuestExiste(string questID)
    {
        for (int i = 0; i < questDisponibles.Length; i++)
        {
            //Si alguna misión disponible, su ID es igual a este ID de la misión que estamos buscando, devolvemos su referencia.
            if (questDisponibles[i].ID == questID)
            {
                //Devolvemos la referencia de la misión que tiene el mismo identificador
                return questDisponibles[i];
            }
        }
        //Si no lo encontramos, devolvemos null.
        return null;
    }

    //Método que actualice todos los componentes al completar una misión.
    //Para poder mostrar una misión completada con la información de la misión, hay que pasar como argumento del método el parámetro de tipo Quest llamado questCompletado.
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
        //Si la misión que ha sido completada existe, (tenemos su referencia dentro de la propiedad QuestPorReclamar) hay que mostrar el panel de QuestCompletado.
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