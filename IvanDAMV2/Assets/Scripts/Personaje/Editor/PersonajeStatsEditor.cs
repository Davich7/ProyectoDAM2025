using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//Especificamos a que va a editar esta clase.
[CustomEditor(typeof(PersonajeStats))]
public class PersonajeStatsEditor : Editor
{
    //Estamos obteniendo el objetivo del editor pero transformamos a tipopersonajestats para poder llamar algún tipo de método dentro de esta clase.
    public PersonajeStats StatsTargets => target as PersonajeStats;
    //Para obtener información de esta clase necesitamos obtener el target objetivo de este editor que seguirá siendo la clase PersonajeStats.
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Resetear Valores"))
        {
            StatsTargets.ResetearValores();
        }
    }
}
