using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "IA/Acciones/Desactivar Camino Movimiento")]
public class AccionDesactivarCaminoMovimiento : IAAccion
{
    public override void Ejecutar(IAController controller)
    {
        //Si no tenemos la referencia de la clase EnemigoMovimiento no continuamos la lógica.
        if (controller.EnemigoMovimiento == null)
        {
            return;
        }
        //Lo desactivamos.
        controller.EnemigoMovimiento.enabled = false;
    }
}
