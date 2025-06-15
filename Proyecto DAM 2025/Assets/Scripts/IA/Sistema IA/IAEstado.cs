using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Para crear un estado en el juego.
[CreateAssetMenu(menuName = "IA/Estado")]
public class IAEstado : ScriptableObject
{
    //Para tener todas las acciones en el estado, creamos la variable pública IAAccion en forma de array.
    public IAAccion[] Acciones;
    public IATransicion[] Transiciones;

    //Método que permite ejecutar transiciones y acciones.
    public void EjecutarEstado(IAController controller)
    {
        EjecutarAcciones(controller);
        RealizarTransiciones(controller);
    }

    //Ejecutaremos todas las transiciones del estado con el siguiente método.
    private void EjecutarAcciones(IAController controller)
    {
        if (Acciones == null || Acciones.Length <= 0)
        {
            return;
        }
        //Recorremos todas las acciones.
        for (int i = 0; i < Acciones.Length; i++)
        {
            //Ejecutamos las acciones en este iterador.
            Acciones[i].Ejecutar(controller);
        }
    }

    private void RealizarTransiciones(IAController controller)
    {
        if (Transiciones == null || Transiciones.Length <= 0)
        {
            return;
        }

        //Recorremos todas las transiciones del estado.
        for (int i = 0; i < Transiciones.Length; i++)
        {
            //Obtenemos el valor de la decisión de cada transición.
            bool decisionValor = Transiciones[i].Decision.Decidir(controller);
            if (decisionValor)
            {
                controller.CambiarEstado(Transiciones[i].EstadoVerdadero);
            }
            else
            {
                controller.CambiarEstado(Transiciones[i].EstadoFalso);
            }
        }
    }
}
