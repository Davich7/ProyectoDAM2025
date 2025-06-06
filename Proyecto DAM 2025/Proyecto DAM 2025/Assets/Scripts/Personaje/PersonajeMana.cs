using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonajeMana : MonoBehaviour
{
    [SerializeField] private float manaInicial;
    [SerializeField] private float manaMax;
    [SerializeField] private float regeneracionPorSegundo;

    //Solamente modificamos dentro de esta clase.
    public float ManaActual { get; private set; }
    public bool SePuedeRestaurar => ManaActual < manaMax;

    //Obtenemos la referencia de la clase PersonajeVida.
    private PersonajeVida _personajeVida;

    private void Awake()
    {
        //Podemos saber si el personaje est� con vida para regenerar el mana.
        _personajeVida = GetComponent<PersonajeVida>(); 
    }

    private void Start()
    {
        ManaActual = manaInicial;
        ActualizarBarraMana();

        //Llamamos m�todo y lo repetimos X veces.
        InvokeRepeating(nameof(RegenerarMana), 1, 1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            //M�todo para probar uso de la cantidad de mana presionando la tecla G.
            UsarMana(10f);
        }
    }

    //Establecemos cual ser� la cantidad de mana que gastara el personaje.
    public void UsarMana(float cantidad)
    {
        //Verificamos mediante bucle si tenemos mana suficiente.
        if (ManaActual >= cantidad) 
        {
            ManaActual -= cantidad;
            ActualizarBarraMana();
        }
    }

    //M�todo restaurar Man�, en caso de que el mana actual es mayor o igual regresamos porque no se puede acumular m�s.
    public void RestaurarMana(float cantidad)
    {
        if (ManaActual >= manaMax)
        {
            return;
        }

        ManaActual += cantidad;
        if (ManaActual > manaMax)
        {
            ManaActual = manaMax;
        }

        UIManager.Instance.ActualizarManaPersonaje(ManaActual, manaMax);
    }

    private void RegenerarMana()
    {
        //Si el personaje tiene m�s de 0 puntos de vida y el mana es menor que el m�ximo podemos regenerar mana.
        if (_personajeVida.Salud > 0f && ManaActual < manaMax)
        {
            ManaActual += regeneracionPorSegundo;
            ActualizarBarraMana();
        }
    }

    public void RestablecerMana()
    {
        ManaActual = manaInicial;
        ActualizarBarraMana();
    }

    private void ActualizarBarraMana()
    {
        UIManager.Instance.ActualizarManaPersonaje(ManaActual, manaMax);
    }
}
