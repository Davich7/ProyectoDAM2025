using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Para ver la transacción en el inspector
[Serializable]
//Controla la transacción de un estado a otro.
public class IATransicion
{
    //Para poder hacer una transacción de un estado a otro hay que conocer el valor de decisión.
    public IADecision Decision;

    //Si la decision devuelve verdadero, se hará la transición a otro estado.
    //Verdadero hace referencia al valor devuelto por Decision.
    public IAEstado EstadoVerdadero;

    //Si decisión devuelve falso, se hará la transacción a este estado.
    public IAEstado EstadoFalso;

}
