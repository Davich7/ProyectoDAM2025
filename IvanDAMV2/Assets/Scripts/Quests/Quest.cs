using System;
using UnityEngine;


//A�adimos atributo para poder crear misiones en las carpetas.
[CreateAssetMenu]
public class Quest : ScriptableObject
{
    //Lanzamos el evento notificamos que la misi�n ha sido completada.
    public static Action<Quest> EventoQuestCompletado;

    [Header("Info")]
    public string Nombre;
    public string ID;
    public int CantidadObjetivo;

    [Header("Descripcion")]
    //Atributo para que sea m�s grande si se quiere a�adir una descripci�n extensa.
    [TextArea] public string Descripcion;

    [Header("Descripcion")]
    public int RecompensaOro;
    public float RecompensaExp;
    public QuestRecompensaItem RecompensaItem;

    //Variable oculta en el inspector para controlar la cantidad hasta llegar al objetivo de la misi�n.
    [HideInInspector] public int CantidadActual;
    //Variable oculta para saber si la misi�n se ha completado.
    [HideInInspector] public bool QuestCompletadoCheck;

    public void A�adirProgreso(int cantidad)
    {
        //CantidadActual controla si hemos llegado a CantidadObjetivo.
        //Cada vez que a�adimos progreso hay que verificar si hemos llegado a CantidadObjetivo.
        CantidadActual += cantidad;
        //Cada vez que a�adimos cantidad, tenemos que verificar si se ha llegado a la cantidad objetivo.
        VerificarQuestCompletado();
    }

    private void VerificarQuestCompletado()
    {
        //Si hemos llegado al objetivo, indicamos que se ha completado la misi�n.
        if (CantidadActual >= CantidadObjetivo)
        {
            CantidadActual = CantidadObjetivo;
            QuestCompletado();
        }
    }

    private void QuestCompletado()
    {
        //Si la misi�n ha sido completada, regresamos.
        if (QuestCompletadoCheck)
        {
            return;
        }

        //Se pone el check como verdadero.
        QuestCompletadoCheck = true;
        //Lanzamos el evento indicando que esta mision acaba de ser completada
        //Si EventoQuestCompletado no es nulo, lo invocamos con la referencia de la misi�n.
        EventoQuestCompletado?.Invoke(this);
    }

    private void OnEnable()
    {
        QuestCompletadoCheck = false;
        CantidadActual = 0;
    }
}
//Para ver clase en Inspector de Unity.
[Serializable]

//Al item que damos como recompensa tenemos que definir la cantidad que se otorga.
public class QuestRecompensaItem
{
    //Referencia del Item que se entregar�.
    public InventarioItem Item;
    //Referencia de la cantidad que se otorgar�.
    public int Cantidad;
}