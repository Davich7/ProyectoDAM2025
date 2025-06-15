using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//DecisionDetectarPersonaje lo creamos en nuestra carpeta.
[CreateAssetMenu(menuName = "IA/Decisiones/Detectar Personaje")]
public class DecisionDetectarPersonaje : IADecision
{
    public override bool Decidir(IAController controller)
    {
        return DetectarPersonaje(controller);
    }

    private bool DetectarPersonaje(IAController controller)
    {
        //Para poder detectar al personaje creamos una variable que nos indique nuestro rango de detección.
        //Para detectar al personaje utilizamos las físicas de Unity que permite crear una colisión en círculo y detectar al personaje.
        //Este método nos pide el punto en el que estará centrado el círculo de detección, se solicita el radio del círculo que será el rango de detección
        //Y el personajeLayerMask que es al personaje que queremos detectar.

        //Si hemos colisionado con el personaje, tendremos la referencia guardada en personajeDetectado.
        Collider2D personajeDetectado = Physics2D.OverlapCircle(controller.transform.position,
            controller.RangoDeteccion,controller.PersonajeLayerMask);
        if (personajeDetectado != null)
        {
            //Devolvemos true ya que habremos detectado al personaje.
            controller.PersonajeReferencia = personajeDetectado.transform;
            return true;
        }
        //Hacemos un check, si dejamos de detectar al personaje para evitar errores.
        controller.PersonajeReferencia = null;
        return false;
    }
}
