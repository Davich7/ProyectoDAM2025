using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TipoPersonaje
{
    Player,
    IA
}

public class PersonajeFX : MonoBehaviour
{
    [Header("Pooler")]
    [SerializeField] private ObjectPooler pooler;

    [Header("Pooler")]
    [SerializeField] private GameObject canvasTextoAnimacionPrefab;
    [SerializeField] private Transform canvasTextoPosicion;

    [Header("Tipo")]
    [SerializeField] private TipoPersonaje tipoPersonaje;

    private EnemigoVida _enemigoVida;

    private void Awake()
    {
        _enemigoVida = GetComponent<EnemigoVida>();
    }

    private void Start()
    {
        pooler.CrearPooler(canvasTextoAnimacionPrefab);
    }

    private IEnumerator IEMostrarTexto(float cantidad, Color color)
    {
        GameObject nuevoTextoGO = pooler.ObtenerInstancia();
        TextoAnimacion texto = nuevoTextoGO.GetComponent<TextoAnimacion>();
        // El texto mostrará la cantidad de vida que está perdiendo el personaje.
        texto.EstablecerTexto(cantidad, color);
        // El texto sigue la posición del personaje al moverse.
        nuevoTextoGO.transform.SetParent(canvasTextoPosicion);
        nuevoTextoGO.transform.position = canvasTextoPosicion.position;
        nuevoTextoGO.SetActive(true);

        // Tiempo establecido para esperar.
        yield return new WaitForSeconds(1f);
        nuevoTextoGO.SetActive(false);
        nuevoTextoGO.transform.SetParent(pooler.ListaContenedor.transform);
    }

    // Llamamos a MostrarTexto.
    private void RespuestaDañoRecibidoHaciaPlayer(float daño)
    {
        if (tipoPersonaje == TipoPersonaje.Player)
        {
            // Cuando hacemos daño al jugador, mostramos texto de color negro.
            StartCoroutine(IEMostrarTexto(daño, Color.black));
        }
    }

    private void RespuestaDañoHaciaEnemigo(float daño, EnemigoVida enemigoVida)
    {
        if (tipoPersonaje == TipoPersonaje.IA && _enemigoVida == enemigoVida)
        {
            // Cuando hacemos daño al enemigo, mostramos texto de color rojo.
            StartCoroutine(IEMostrarTexto(daño, Color.red));
        }
    }

    private void OnEnable()
    {
        IAController.EventoDañoRealizado += RespuestaDañoRecibidoHaciaPlayer;
        PersonajeAtaque.EventoEnemigoDañado += RespuestaDañoHaciaEnemigo;
    }

    private void OnDisable()
    {
        IAController.EventoDañoRealizado -= RespuestaDañoRecibidoHaciaPlayer;
        PersonajeAtaque.EventoEnemigoDañado -= RespuestaDañoHaciaEnemigo;
    }
}
