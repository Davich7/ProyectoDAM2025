using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TipoDeteccion
{
    Rango,
    Melee
}

public class EnemigoInteraccion : MonoBehaviour
{
    [SerializeField] private GameObject seleccionRangoFX;
    [SerializeField] private GameObject seleccionMeleeFX;

    public void MostrarEnemigoSeleccionado(bool estado, TipoDeteccion tipo)
    {
        if (tipo == TipoDeteccion.Rango)
        {
            //En funci�n si estado es verdadero o falso, se mostrar� o no.
            seleccionRangoFX.SetActive(estado);
        } 
        else
        {
            seleccionMeleeFX.SetActive(estado);
        }
    }

    // M�todo que nos permite desactivar ambos sprites si el enemigo muere.
    public void DesactivarSpriteSeleccion()
    {
        seleccionMeleeFX.SetActive(false);
        seleccionRangoFX.SetActive(false);
    }
}
