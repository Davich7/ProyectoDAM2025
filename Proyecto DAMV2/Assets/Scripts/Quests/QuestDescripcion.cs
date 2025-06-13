using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestDescripcion : MonoBehaviour
{
    //Variables a referenciar.
    [SerializeField] private TextMeshProUGUI questNombre;
    [SerializeField] private TextMeshProUGUI questDescripcion;

    public Quest QuestPorCompletar { get; set; }

    //Método que permite actualizar las variables. Para poder actualizar
    //El nombre y la descripción de una misión, necesitamos pasar como parámetro la clase Quest que es ScriptableObject que contiene toda la información.
    public virtual void ConfigurarQuestUI(Quest quest)
    {
        QuestPorCompletar = quest;
        questNombre.text = quest.Nombre;
        questDescripcion.text = quest.Descripcion;
    }
}
