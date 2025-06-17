using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonajeExperiencia : MonoBehaviour
{
    //Generamos referencia de las stats contenida en otra clase.
    [Header("Stats")]
    [SerializeField] private PersonajeStats stats;

    [Header("Config")]
    [SerializeField] private int nivelMax;
    [SerializeField] private int expBase;
    [SerializeField] private int valorIncremental;


    private float expActual;
    private float expRequeridaSiguienteNivel;
    private void Start()
    {
        stats.Nivel = 1;
        expRequeridaSiguienteNivel = expBase;
        stats.ExpRequeridaSiguienteNivel = expRequeridaSiguienteNivel;
        ActualizarBarraExp();
    }

    //M�todo para a�adir experiencia para test manualmente.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            A�adirExperiencia(2f);
        }
    }
    public void A�adirExperiencia(float expObtenida)
    {
        if (expObtenida <= 0) return;
        expActual += expObtenida;
        stats.ExpActual = expActual;

        if (expActual == expRequeridaSiguienteNivel)
        {
            ActualizarNivel();
        }
        else if (expActual > expRequeridaSiguienteNivel)
        {
            float dif = expActual - expRequeridaSiguienteNivel;
            ActualizarNivel();
            A�adirExperiencia(dif);
        }

        stats.ExpTotal += expObtenida;
        ActualizarBarraExp();
    }

    /*Actualizaremos la experiencia requerida para el siguiente nivel. Actualizaremos el nivel y la experiencia actual temporal siempre que nuestro nivel sea menor que el 
     nivel m�ximo*/
    private void ActualizarNivel()
    {
        if (stats.Nivel < nivelMax) 
        {
            stats.Nivel++;
            stats.ExpActual = 0;
            expActual = 0;
            expRequeridaSiguienteNivel *= valorIncremental;
            stats.ExpRequeridaSiguienteNivel = expRequeridaSiguienteNivel;
            stats.PuntosDisponibles += 3;
        }
    }
    private void ActualizarBarraExp()
    {
        //El valor ser� la experencia

        UIManager.Instance.ActualizarExpPersonaje(expActual, expRequeridaSiguienteNivel);
 
    }

    private void RespuestaEnemigoDerrotado(float exp)
    {
        A�adirExperiencia(exp);
    }
    private void OnEnable()
    {
        EnemigoVida.EventoEnemigoDerrotado += RespuestaEnemigoDerrotado;
    }

    private void OnDisable()
    {
        EnemigoVida.EventoEnemigoDerrotado -= RespuestaEnemigoDerrotado;
    }
}
