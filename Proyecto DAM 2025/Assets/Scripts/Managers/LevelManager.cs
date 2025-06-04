using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //Referencia del personaje.
    [SerializeField] private Personaje personaje;
    //Obtenemos referencia de donde queremos mover al personaje cuando se restaure.
    [SerializeField] private Transform puntoReaparicion;

    //Necesitamos conocer si el personaje ha sido derrotado (propiedad de PersonajeVida.cs) para restaurarlo.
    private void Update()
    {
        //Al pulsar en R restauramos el personaje a la posición inicial.
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (personaje.PersonajeVida.Derrotado)
            {
                personaje.transform.localPosition = puntoReaparicion.position;
                personaje.RestaurarPersonaje();
            }
        }
    }
}
