using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonajeFX : MonoBehaviour
{

    [SerializeField] private GameObject canvasTextoAnimacionPrefab;
    [SerializeField] private Transform canvasTextoPosicion;

    private ObjectPooler pooler;

    //Método para poder obtener la referencia del Pooler.

    private void Awake()
    {
        pooler = GetComponent<ObjectPooler>();  
    }
    private void Start()
    {
        pooler.CrearPooler(canvasTextoAnimacionPrefab);
    }

    private IEnumerator IEMostrarTexto(float cantidad)
    {
        GameObject nuevoTextoGO = pooler.ObtenerInstancia();
        TextoAnimacion texto = nuevoTextoGO.GetComponent<TextoAnimacion>();
        // El texto mostrará la cantidad de vida que está perdiendo el personaje.
        texto.EstablecerTexto(cantidad);
        // El texto sigue la posición del personaje al moverse.
        nuevoTextoGO.transform.SetParent(canvasTextoPosicion);
        nuevoTextoGO.transform.position = canvasTextoPosicion.position;
        nuevoTextoGO.SetActive(true);

        // Tiempo establecido para esperar.
        yield return new WaitForSeconds(1f);
        nuevoTextoGO.SetActive(false);
        nuevoTextoGO.transform.SetParent(pooler.listaContenedor.transform);
    }

    // Llamamos a MostrarTexto.
    private void RespuestaDañoRecibido(float daño)
    {
        StartCoroutine(IEMostrarTexto(daño));
    }

    private void OnEnable()
    {
        IAController.EventoDañoRealizado += RespuestaDañoRecibido;
    }

    private void OnDisable()
    {
        IAController.EventoDañoRealizado -= RespuestaDañoRecibido;
    }
}
