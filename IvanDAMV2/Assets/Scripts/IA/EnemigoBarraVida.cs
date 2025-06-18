using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemigoBarraVida : MonoBehaviour
{
    // Referencia de la barra de vida.
    [SerializeField] private Image barraVida;

    private float saludActual;
    private float saludMax;

    private void Update()
    {
        // Actualizamos la salud del enemigo.
        barraVida.fillAmount = Mathf.Lerp(barraVida.fillAmount,
            saludActual / saludMax, 10f * Time.deltaTime);
    }

    public void ModificarSalud(float pSaludActual, float pSaludMax) 
    {
        saludActual = pSaludActual;
        saludMax = pSaludMax;
    }
}
