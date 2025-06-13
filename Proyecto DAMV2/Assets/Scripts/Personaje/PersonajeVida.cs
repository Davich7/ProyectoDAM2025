using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//PersonajeVida hereda de la clase VidaBase.

public class PersonajeVida : VidaBase
    {
    //Creación de evento
    public static Action EventoPersonajeDerrotado;
    public bool Derrotado { get; private set; }
    public bool PuedeSerCurado => Salud < saludMax;

    //Colisionador del personaje
    private BoxCollider2D _boxCollider2D;

    private void Awake()
    {
       _boxCollider2D = GetComponent<BoxCollider2D>();
    }
    protected override void Start()
    {
        //Base llama al valor contenido en el start de la clase base.
        base.Start();
        ActualizarBarraVida(Salud, saludMax);
    }

    public void Update()
    {   //Incremento de daño en 10 para test al pulsar en T.
        if (Input.GetKeyDown(KeyCode.T))
        {
            RecibirDaño(10);
        }
        //Incremento de vida en 10 para test al pulsar en Y.
        if (Input.GetKeyDown(KeyCode.Y))
        {
            RestaurarSalud(10);
        }
    }
    public void RestaurarSalud(float cantidad)
    {
        //Solamente recuperamos salud si el personaje no ha sido derrotado.
        if (Derrotado)
        {
            return;
        }

        if (PuedeSerCurado)
        {
            Salud += cantidad;
            if (Salud > saludMax)
            {
                Salud = saludMax;
            }
            ActualizarBarraVida(Salud, saludMax);
        }
    }

    protected override void PersonajeDerrotado()
    {
        //Desactivamos el boxcollider.
        _boxCollider2D.enabled = false;
        Derrotado = true;
        //Si el evento PersonajeDerrotado no es nulo, lo invocamos.
        EventoPersonajeDerrotado?.Invoke();
        //Lanzamos el evento asegurandonos que están en escucha
    }

    public void RestaurarPersonaje()
    {
        _boxCollider2D.enabled = true;
        Derrotado = false;
        Salud = saludInicial;
        ActualizarBarraVida(Salud, saludInicial);
    }

    //Sobreescribiremos el método de ActualizarBarraVida
    protected override void ActualizarBarraVida(float vidaActual, float vidaMax)
    {
        UIManager.Instance.ActualizarVidaPersonaje(vidaActual, vidaMax);
    }
}

