using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonedasManager : Singleton<MonedasManager>
{
    [SerializeField] private int monedasTest;
    //Creamos propiedad que almacene la cantidad de monedas totales que tenemos.
    public int MonedasTotales { get; set; }

    private string KEY_MONEDAS = "MYJUEGO_MONEDAS";

    private void Start()
    {
        PlayerPrefs.DeleteKey(KEY_MONEDAS);
        //Para evitar guardar un valor diferente cada vez que se llama a los métodos Añadir o Eliminar monedas.
        CargarMonedas();
    }

    //Para cargar monedas hay que inicializar monedas al valor guardado en la KEY.
    private void CargarMonedas()
    {
        MonedasTotales = PlayerPrefs.GetInt(KEY_MONEDAS, monedasTest);
    }

    public void AñadirMonedas(int cantidad)
    {
        MonedasTotales += cantidad;
        //Como hemos modificado el valor de MonedasTotales hay que guardarlo.
        //Para poder guardar el valor de SetInt debemos de dar una contraseña
        PlayerPrefs.SetInt(KEY_MONEDAS, MonedasTotales);    
        PlayerPrefs.Save();
    }

    //Mandamos como parámetro la cantidad de monedas que queremos eliminar.
    public void RemoverMonedas(int cantidad)
    {
        //Antes de eliminar monedas debemos verificar que tenemos suficientes monedas para quitar.
        //Si la cantidad que queremos eliminar es mayor que las monedas que tenemos, regresamos.
        if (cantidad > MonedasTotales)
        {
            return;  
        }
        //Si tenemos más monedas continuamos.
        MonedasTotales -= cantidad;
        PlayerPrefs.SetInt(KEY_MONEDAS, MonedasTotales);
        PlayerPrefs.Save();
    }
}
