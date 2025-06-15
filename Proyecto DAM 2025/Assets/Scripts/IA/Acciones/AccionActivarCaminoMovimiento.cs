using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "IA/Acciones/Activar Camino Movimiento")]
public class AccionActivarCaminoMovimiento : IAAccion
{
    public override void Ejecutar(IAController controller)
    {
        //Si no tenemos la referencia de la clase EnemigoMovimiento no continuamos la lógica.
        if (controller.EnemigoMovimiento == null)
        {
            return;
        }
        //Lo activamos.
        controller.EnemigoMovimiento.enabled = true;
    }
}
