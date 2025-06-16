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
    private float expActualTemp;
    private float expRequeridaSiguienteNivel;
    private void Start()
    {
        stats.Nivel = 1;
        expRequeridaSiguienteNivel = expBase;
        stats.ExpRequeridaSiguienteNivel = expRequeridaSiguienteNivel;
        ActualizarBarraExp();
    }

    //Método para añadir experiencia para test manualmente.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            AñadirExperiencia(2f);
        }
    }
    public void AñadirExperiencia(float expObtenida)
    {

        // Cuando matamos a un enemigo, si obtenemos 10 de experiencia
        if (expObtenida > 0f)
        {
            //Controlamos la experiencia
            float expRestanteNuevoNivel = expRequeridaSiguienteNivel - expActualTemp;
            if (expObtenida >= expRestanteNuevoNivel)
            {
                //A la experiencia obtenida le restamos la experiencia del nuevo nivel. Sería 8 de experiencia y actualizamos el nivel
                expObtenida -= expRestanteNuevoNivel;
                expActual += expObtenida;
                ActualizarNivel();
                //Llamamos al mismo método AñadirExperiencia dentro del mismo método como recursión
                AñadirExperiencia(expObtenida);
            }
            else
            {
                expActual += expObtenida;
                expActualTemp += expObtenida;
                if (expActualTemp == expRequeridaSiguienteNivel)
                {
                    ActualizarNivel();
                }
            }
        }
        stats.ExpActual = expActual;
        ActualizarBarraExp();
        
    }

    /*Actualizaremos la experiencia requerida para el siguiente nivel. Actualizaremos el nivel y la experiencia actual temporal siempre que nuestro nivel sea menor que el 
     nivel máximo*/
    private void ActualizarNivel()
    {
        if (stats.Nivel < nivelMax) 
        {
            stats.Nivel++;
            expActualTemp = 0f;
            expRequeridaSiguienteNivel *= valorIncremental;
            stats.ExpRequeridaSiguienteNivel = expRequeridaSiguienteNivel;
            stats.PuntosDisponibles += 3;
        }
    }
    private void ActualizarBarraExp()
    {
        //El valor será la experencia

        UIManager.Instance.ActualizarExpPersonaje(expActualTemp, expRequeridaSiguienteNivel);
 
    }
}
