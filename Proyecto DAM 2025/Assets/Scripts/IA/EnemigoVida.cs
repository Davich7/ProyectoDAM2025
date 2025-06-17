// Vida base tiene la lógica para la vida de personaje y enemigos.
using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemigoVida : VidaBase
{
    public static Action<float> EventoEnemigoDerrotado;

    [Header("Vida")]
    [SerializeField] private EnemigoBarraVida barraVidaPrefab;
    [SerializeField] private Transform barraVidaPosicion;

    // Obtenemos referencia del sprite render y collider del enemigo ya que cuando
    // lo derrotemos le tenemos que desactivar el collider para no colisionar.

    // Rastro para loot cuando enemigo muere.
    [Header("Rastros")]
    [SerializeField] private GameObject rastros;

    private EnemigoBarraVida _enemigoBarraVidaCreada;
    private EnemigoInteraccion _enemigoInteraccion;
    private EnemigoMovimiento _enemigoMovimiento;
    private EnemigoLoot _enemigoLoot;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2D;
    // Cuando derrotemos al enemigo pararemos la lógica de la IA del enemigo.
    private IAController _controller;

    private void Awake()
    {
        _enemigoInteraccion = GetComponent<EnemigoInteraccion>();
        _enemigoMovimiento = GetComponent<EnemigoMovimiento>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemigoLoot = GetComponent<EnemigoLoot>();
        _boxCollider2D = GetComponent<BoxCollider2D>(); 
        _controller = GetComponent<IAController>();
    }
    protected override void Start()
    {
        base.Start();
        CrearBarraVida();
    }

    private void CrearBarraVida()
    {
        /* Instanciamos el prefab de barra de vida y lo guardamos en la variable
        _enemigoBarraVidaCreada para poder desactivarlo cuando el enemigo es
        derrotado */
        _enemigoBarraVidaCreada = Instantiate(barraVidaPrefab, barraVidaPosicion);
        ActualizarBarraVida(Salud, saludMax);
    }

    protected override void ActualizarBarraVida(float vidaActual, float vidaMax)
    {
        _enemigoBarraVidaCreada.ModificarSalud(vidaActual, vidaMax);
    }

    protected override void PersonajeDerrotado()
    {
        DesactivarEnemigo();
        EventoEnemigoDerrotado?.Invoke(_enemigoLoot.ExpGanada);
        QuestManager.Instance.AñadirProgreso("Mata10", 1);
        QuestManager.Instance.AñadirProgreso("Mata25", 1);
        QuestManager.Instance.AñadirProgreso("Mata50", 1);
    }


    /* Desactivamos del enemigo cuando muere su imagen para que no aparezca,
     su barra de vida y la clase que lo controla. */
    private void DesactivarEnemigo()
    {
        rastros.SetActive(true);
        _enemigoBarraVidaCreada.gameObject.SetActive(false);
        _spriteRenderer.enabled = false;
        _enemigoMovimiento.enabled = false;
        _controller.enabled = false;
        _boxCollider2D.isTrigger = true;
        _enemigoInteraccion.DesactivarSpriteSeleccion();
    }
}

