using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Para crear la acci�n a�adimos su atributo de tipo CreateAssetMenu
[CreateAssetMenu(menuName = "IA/Acciones/Seguir Personaje")]
public class AccionSeguirPersonaje : IAAccion
{
    //Logica para seguir al personaje.
    public override void Ejecutar(IAController controller)
    {
        SeguirPersonaje(controller);
    }

    //Hacemos un nuevo m�todo por limpieza de clase.
    private void SeguirPersonaje(IAController controller)
    {
        //Nos aseguramos que tenemos la referencia del personaje.
        //Si no tenemos la referencia del personaje volvemos.
        if (controller.PersonajeReferencia == null)
        {
            return;
        }
        //Si tenemos la referencia obtenemos la direcci�n del personaje que es donde se mover� el enemigo.
        Vector3 dirHaciaPersonaje =
            controller.PersonajeReferencia.position - controller.transform.position;
        //Como queremos que el valor de Vector3 no supere 1, indicamos que el controller es de tipo normalized en cuanto a su posici�n.
        Vector3 direccion = dirHaciaPersonaje.normalized;
        //Distancia del vector para el personaje, ya que se quiere colisionar pero hasta un punto que no colisionemos con �l.
        float distancia = dirHaciaPersonaje.magnitude;

        //Nos permite que el enemigo no est� en la misma posici�n que el personaje.
        if (distancia >= 1.30f)
        {
            controller.transform.Translate(
                direccion * controller.VelocidadMovimiento * Time.deltaTime);
        }
    
    }
}
