using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class VidaBase : MonoBehaviour
{
    [SerializeField] protected float saludInicial;
    [SerializeField] protected float saludMax;
    // Start is called before the first frame update

    //Propiedad que puede ser regresada y modificada. Solamente ser� modificada desde la clase Vida (prot)
    public float Salud { get; protected set; }

    //Sobreescribimos (Prot) para poder utilizar el m�todo Start en la clase PersonajeVida y as� actualizar la vida del personaje al comienzo de la partida.
    protected virtual void Start()
    {
        Salud = saludInicial;
    }

    // Update is called once per frame
    public void RecibirDa�o(float cantidad)
    {
        if (cantidad <= 0)
        {
            return;  
        }
        if (Salud > 0f) 
        {
            Salud -= cantidad;
            ActualizarBarraVida(Salud, saludMax);   
            if (Salud <= 0f)
            {
                Salud = 0f;
                ActualizarBarraVida(Salud, saludMax);
                PersonajeDerrotado();
            }
        }
    }
    // M�todos que pueden ser sobrescritos (virtual)
    protected virtual void ActualizarBarraVida(float vidaActual, float vidaMax)
    {

    }
    protected virtual void PersonajeDerrotado()
    {

    }
    
}
