using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonajeFX : MonoBehaviour
{

    [SerializeField] private GameObject canvasTextoAnimacionPrefab;
    [SerializeField] private Transform canvasTextoPosicion;

    private ObjectPooler pooler;

    //M�todo para poder obtener la referencia del Pooler.

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
        // El texto mostrar� la cantidad de vida que est� perdiendo el personaje.
        texto.EstablecerTexto(cantidad);
        // El texto sigue la posici�n del personaje al moverse.
        nuevoTextoGO.transform.SetParent(canvasTextoPosicion);
        nuevoTextoGO.transform.position = canvasTextoPosicion.position;
        nuevoTextoGO.SetActive(true);

        // Tiempo establecido para esperar.
        yield return new WaitForSeconds(1f);
        nuevoTextoGO.SetActive(false);
        nuevoTextoGO.transform.SetParent(pooler.listaContenedor.transform);
    }

    // Llamamos a MostrarTexto.
    private void RespuestaDa�oRecibido(float da�o)
    {
        StartCoroutine(IEMostrarTexto(da�o));
    }

    private void OnEnable()
    {
        IAController.EventoDa�oRealizado += RespuestaDa�oRecibido;
    }

    private void OnDisable()
    {
        IAController.EventoDa�oRealizado -= RespuestaDa�oRecibido;
    }
}
