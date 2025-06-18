using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//El ScriptableObject ya puede ser creado en el menú, ya que es un Asset.
[CreateAssetMenu (menuName = "Stats")]

//En ScriptableObject es seguro añadir variables de tipo público, esto se debe a que debe compartir su información.
public class PersonajeStats : ScriptableObject
{
    [Header("Stats")]
    //Para resetear valores tenemos que definir el valor de cada.
    public float Daño = 5f;
    public float Defensa = 2f;
    public float Velocidad = 5f;
    public float Nivel;
    public float ExpActual;
    public float ExpRequeridaSiguienteNivel;
    public float ExpTotal;
    //Modificar valores en rango de 0-100
    [Range(0f, 100f)] public float PorcentajeCritico;
    [Range(0f, 100f)] public float PorcentajeBloqueo;

    [Header("Atributos")]

    public int Fuerza;
    public int Inteligencia;
    public int Destreza;

    //Ocultamos en el inspector
    [HideInInspector] public int PuntosDisponibles;

    //Cuando aumentamos un atributo de fuerza estos son los puntos que se añaden al personaje.
    public void AñadirBonusPorAtributoFuerza()
    {
        Daño += 2f;
        Defensa += 1f;
        PorcentajeBloqueo += 0.03f;
    }
    public void AñadirBonusPorAtributoInteligencia()
    {
        Daño += 3f;
        PorcentajeCritico += 0.2f;
    }

    public void AñadirBonusPorAtributoDestreza()
    {
        Velocidad += 0.1f;
        PorcentajeBloqueo += 0.05f;
    }

    // Equipar arma aumenta estadísticas del personaje.
    public void AñadirBonusPorArma(Arma arma)
    {
        Daño += arma.Daño;
        PorcentajeCritico += arma.ChanceCritico;
        PorcentajeBloqueo += arma.ChanceBloqueo;
    }

    // Desequipar el arma elimina estadísticas al personaje.
    public void RemoverBonusPorArma(Arma arma)
    {
        Daño += arma.Daño;
        PorcentajeCritico -= arma.ChanceCritico;
        PorcentajeBloqueo -= arma.ChanceBloqueo;
    }

    //Cuando llamamos al método ResetearValores() también reseteamos los valores de Fuerza, Intelligencia y Destreza.
    public void ResetearValores()
    {
        Daño = 5f;
        Defensa = 2f;
        Velocidad = 5f;
        Nivel = 1f;
        ExpActual = 0f;
        ExpRequeridaSiguienteNivel = 0f;
        ExpTotal = 0;
        PorcentajeBloqueo = 0f;
        PorcentajeCritico = 0f;

        Fuerza = 0;
        Inteligencia = 0;
        Destreza = 0;
            
        PuntosDisponibles = 0;
    }
}
