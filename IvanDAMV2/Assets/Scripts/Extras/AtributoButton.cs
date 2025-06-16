using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TipoAtributo
{
    Fuerza,
    Inteligencia,
    Destreza,
}

public class AtributoButton : MonoBehaviour
    //Lanzaremos un evento y especificaremos que tipo de atributo estamos clicando mediante una referencia.
{
    //Cuando pulsamos el botón lanzamos evento, especificando el tipo de atributo que se está tratando de aumentar.
    public static Action<TipoAtributo> EventoAgregarAtributo;

    [SerializeField] private TipoAtributo tipo;

    public void AgregarAtributo()
    {
        //Lanzamos evento si no es nulo lo invocamos indicando la referencia del tipo de atributo para poder lanzar el evento.
        EventoAgregarAtributo?.Invoke(tipo);
    }
}
