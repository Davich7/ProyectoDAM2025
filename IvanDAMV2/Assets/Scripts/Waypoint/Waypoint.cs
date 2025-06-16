using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private Vector3[] puntos;

    public Vector3[] Puntos => puntos;

    //Guardamos la posici�n del personaje en cada momento.

    public Vector3 PosicionActual { get; set; }
    private bool juegoIniciado;

    private void Start()
    {
        juegoIniciado = true;
        //Posici�n actual es igual a la posici�n del objeto.
        PosicionActual = transform.position;
    }

    //M�todo para conocer la posici�n a donde se mover� el personaje.
    public Vector3 ObtenerPosicionMovimiento (int index)
    {
        //De este modo sabremos la posici�n del punto a donde nos moveremos.
        return PosicionActual + puntos[index];
    }

    private void OnDrawGizmos()
    {
        //Mientras no estemos en playmode y mientras estemos cambiando la posici�n del NPC actualizamos su posici�n actual a la posici�n de su transform.
        if (juegoIniciado == false && transform.hasChanged)
        {
            PosicionActual = transform.position;
        }

        if (puntos == null || puntos.Length <= 0)
        {
            return;
        }

        for (int i = 0; i < puntos.Length; i++)
        {
            Gizmos.color = Color.blue;
            //Dibujar una esfera en cada punto de recorrido.
            Gizmos.DrawWireSphere(puntos[i] + PosicionActual, 0.5f);
            //Comprobamos que no superamos los puntos definidos en el array.
            if (i < puntos.Length - 1)
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawLine(puntos[i] + PosicionActual, puntos[i + 1] + PosicionActual);
            }
        }
    }
}
