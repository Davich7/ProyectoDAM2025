using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//El ScriptableObject ya puede ser creado en el men�, ya que es un Asset.
[CreateAssetMenu (menuName = "Stats")]

//En ScriptableObject es seguro a�adir variables de tipo p�blico, esto se debe a que debe compartir su informaci�n.
public class PersonajeStats : ScriptableObject
{
    [Header("Stats")]
    //Para resetear valores tenemos que definir el valor de cada.
    public float Da�o = 5f;
    public float Defensa = 2f;
    public float Velocidad = 5f;
    public float Nivel;
    public float ExpActual;
    public float ExpRequeridaSiguienteNivel;
    //Modificar valores en rango de 0-100
    [Range(0f, 100f)] public float PorcentajeCritico;
    [Range(0f, 100f)] public float PorcentajeBloqueo;

    [Header("Atributos")]

    public int Fuerza;
    public int Inteligencia;
    public int Destreza;

    //Ocultamos en el inspector
    [HideInInspector] public int PuntosDisponibles;

    //Cuando aumentamos un atributo de fuerza estos son los puntos que se a�aden al personaje.
    public void A�adirBonusPorAtributoFuerza()
    {
        Da�o += 2f;
        Defensa += 1f;
        PorcentajeBloqueo += 0.03f;
    }
    public void A�adirBonusPorAtributoInteligencia()
    {
        Da�o += 3f;
        PorcentajeCritico += 0.2f;
    }

    public void A�adirBonusPorAtributoDestreza()
    {
        Velocidad += 0.1f;
        PorcentajeBloqueo += 0.05f;
    }

    //Cuando llamamos al m�todo ResetearValores() tambi�n reseteamos los valores de Fuerza, Intelligencia y Destreza.
    public void ResetearValores()
    {
        Da�o = 5f;
        Defensa = 2f;
        Velocidad = 5f;
        Nivel = 1f;
        ExpActual = 0f;
        ExpRequeridaSiguienteNivel = 0f;
        PorcentajeBloqueo = 0f;
        PorcentajeCritico = 0f;

        Fuerza = 0;
        Inteligencia = 0;
        Destreza = 0;
            
        PuntosDisponibles = 0;
    }
}
