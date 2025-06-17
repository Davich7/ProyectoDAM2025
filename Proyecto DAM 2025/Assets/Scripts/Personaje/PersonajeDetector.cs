using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonajeDetector : MonoBehaviour
{
    // Lanzamos el evento que se ha detectado a un enemigo de tipo melee.
    public static Action<EnemigoInteraccion> EventoEnemigoDetectado;
    public static Action EventoEnemigoPerdido;

    // Guardar referencia del enemigo que estamos detectando.
    public EnemigoInteraccion EnemigoDetectado { get; private set; }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Verificar que estamos colisionando con un objeto que tiene el tag de enemigo.
        if (other.CompareTag("Enemigo"))
        {
            EnemigoDetectado = other.GetComponent<EnemigoInteraccion>();
            // Cuando detectamos al enemigo lanzamos el evento de EnemigoDetectado.
            // Verificamos que no es nulo y lo invocamos con la referencia de EnemigoDetectado.
            if (EnemigoDetectado.GetComponent<EnemigoVida>().Salud > 0)
            {
                EventoEnemigoDetectado?.Invoke(EnemigoDetectado);
            } 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
      //Verificar que estamos colisionando con un objeto que tiene el tag de enemigo.
        if (other.CompareTag("Enemigo"))
        {
            EventoEnemigoPerdido?.Invoke();
        }
    }
}
