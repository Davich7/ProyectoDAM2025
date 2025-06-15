using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "IA/Acciones/Atacar Personaje")]
//Crear acción dentro de nuestras carpetas en Unity.
public class AccionAtacarPersonaje : IAAccion
{
    public override void Ejecutar(IAController controller)
    {
        Atacar(controller);
        
    }

    private void Atacar(IAController controller)
    {
        //Antes de atacar tenemos que verificar si tenemos una referencia del personaje y si es tiempo de atacar.
        if (controller.PersonajeReferencia == null)
        {
            return;
        }
        if (controller.EsTiempoDeAtacar() == false)
        {
            return;
        }
        //Si el personaje está en rango de ataque utilizando el rango según el tipo de ataque del enemigo.
        if (controller.PersonajeEnRangoDeAtaque(controller.RangoDeAtaqueDeterminado))
        {
            if(controller.TipoAtaque == TiposDeAtaque.Embestida)
            {
                controller.AtaqueEmbestida(controller.Daño);
            } 
            else
            {
                controller.AtaqueMelee(controller.Daño);
            }
                //Atacamos personaje y actualizamos el tiempo entre ataques.
            controller.ActualizarTiempoEntreAtaques();

        }
    
       
    }
}
