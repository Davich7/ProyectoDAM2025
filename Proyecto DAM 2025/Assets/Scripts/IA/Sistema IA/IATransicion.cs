using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Para ver la transacci�n en el inspector
[Serializable]
//Controla la transacci�n de un estado a otro.
public class IATransicion
{
    //Para poder hacer una transacci�n de un estado a otro hay que conocer el valor de decisi�n.
    public IADecision Decision;

    //Si la decision devuelve verdadero, se har� la transici�n a otro estado.
    //Verdadero hace referencia al valor devuelto por Decision.
    public IAEstado EstadoVerdadero;

    //Si decisi�n devuelve falso, se har� la transacci�n a este estado.
    public IAEstado EstadoFalso;

}
