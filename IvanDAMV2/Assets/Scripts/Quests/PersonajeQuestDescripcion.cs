using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//PersonajeQuestDescripcion hereda de la clase baes QuestDescripcion para así sobreescribir el método.
public class PersonajeQuestDescripcion : QuestDescripcion
{
    //Creado para la tarea objetivo que se requiere para completar la misión.
    [SerializeField] private TextMeshProUGUI tareaObjetivo;
    [SerializeField] private TextMeshProUGUI recompensaOro;
    [SerializeField] private TextMeshProUGUI recompensaExp;

    //Referencias del sprite que vamos a actualizar del item que se otorgará y su cantidad.
    [Header("Item")]
    [SerializeField] private Image recompensaItemIcono;
    [SerializeField] private TextMeshProUGUI recompensaItemCantidad;

    private void Update()
    {
        //Mientras el quest no ha sido completado, hay que actualizar su texto.
        if (QuestPorCompletar.QuestCompletadoCheck)
        {
            return;
        }

        tareaObjetivo.text = $"{QuestPorCompletar.CantidadActual}/{QuestPorCompletar.CantidadObjetivo}";
    }

    /*Obtenemos una referencia del texto de recompensa oro, recompensa experiencia y recomp
        ensa de la cantidad del item, el sprite del item y la tarea objetivo para poder actualizarlos.*/
    public override void ConfigurarQuestUI(Quest quest)
    {
        //Actualizamos el nombre y la descripción de la misión.
        base.ConfigurarQuestUI(quest);
        recompensaOro.text = quest.RecompensaOro.ToString();
        recompensaExp.text = quest.RecompensaExp.ToString();
        //Actualizamos la tarea objetivo para completar la misión.
        tareaObjetivo.text = $"{quest.CantidadActual}/{quest.CantidadObjetivo}";

        //Actualizamos el sprite (icono item) que damos como recompensa y su cantidad.
        recompensaItemIcono.sprite = quest.RecompensaItem.Item.Icono;
        recompensaItemCantidad.text = quest.RecompensaItem.Cantidad.ToString();
        
    }

    //Para actualizar tareaobjetivo a 10/10 hay que verificar si la misión que se ha completado es la misma que está cargada en método Update() para evitar actualizar otras misiones.
    private void QuestCompletadoRespuesta(Quest questCompletado)
    {
        if (questCompletado.ID == QuestPorCompletar.ID)
        {
            tareaObjetivo.text = $"{QuestPorCompletar.CantidadActual}/{QuestPorCompletar.CantidadObjetivo}";
            //Indicamos que la misión se ha completado y la eliminamos del panel.
            gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        if (QuestPorCompletar.QuestCompletadoCheck)
        {
            gameObject.SetActive(false);
        }

        Quest.EventoQuestCompletado += QuestCompletadoRespuesta;
    }

    private void OnDisable()
    {
        Quest.EventoQuestCompletado -= QuestCompletadoRespuesta;
    }
}
