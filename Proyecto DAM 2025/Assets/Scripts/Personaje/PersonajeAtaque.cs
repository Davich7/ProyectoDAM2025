using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class PersonajeAtaque : MonoBehaviour
{
    public static Action<float, EnemigoVida> EventoEnemigoDañado;

    // Accedemos a las stats del personaje.
    [Header("Stats")]
    [SerializeField] private PersonajeStats stats;

    [Header("Pooler")]
    [SerializeField] private ObjectPooler pooler;

    [Header("Ataque")]
    //Cada cuantos segundos se puede atacar.
    [SerializeField] private float tiempoEntreAtaques;
    [SerializeField] private Transform[] posicionesDisparo;

    // Para cargar arma en personaje, creamos propiedad donde guardaremos esa referencia.
    // Set como privado ya que solamente nos interesa equiparnos un arma en esta clase.
    public Arma ArmaEquipada { get; private set; }
    //Guardamos la referencia del enemigo cuando lo seleccionamos.
    //Solamente establecemos esta referencia en la clase personaje ataque (priv set)
    public EnemigoInteraccion EnemigoObjetivo { get; private set; }

    public bool Atacando { get; set; }
    //Creamos propiedad que nos devuelva que estamos atacando.

    private PersonajeMana _personajeMana;
    private int indexDireccionDisparo;
    private float tiempoParaSiguienteAtaque;

    private void Awake()
    {
        _personajeMana = GetComponent<PersonajeMana>();
    }

    private void Update()
    {
        ObtenerDireccionDisparo();

        if (Time.time > tiempoParaSiguienteAtaque)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Si apretamos espacio hay que verificar si hay arma equipada y si hay enemigo antes de usar el arma.
                if (ArmaEquipada == null || EnemigoObjetivo == null)
                {
                    return;
                }
                UsarArma();
                //Actualizar tiempo para el siguiente ataque.
                tiempoParaSiguienteAtaque = Time.time + tiempoEntreAtaques;
                StartCoroutine(IEEstablecerCondicionAtaque());
            }
        }
    }

    private void UsarArma()
    {
        // Verificamos que el arma equipada es de tipo magia.
        if (ArmaEquipada.Tipo == TipoArma.Magia)
        {
            // Si no tenemos mana para usar el arma, regresamos.
            if (_personajeMana.ManaActual < ArmaEquipada.ManaRequerida)
            {
                return;
            }
            // Si no continuamos usando el arma.
            // Obtenemos una instancia del proyectil del pooler.
            GameObject nuevoProyectil = pooler.ObtenerInstancia();
            nuevoProyectil.transform.localPosition = posicionesDisparo[indexDireccionDisparo].position;
    
            // Obtenemos referencia de la clase proyectil, que está en el Gameobject Proyectil.
            Proyectil proyectil = nuevoProyectil.GetComponent<Proyectil>();
            proyectil.InicializarProyectil(this);

            // Activamos proyectil que está desactivado por defecto.
            nuevoProyectil.SetActive(true);

            //Descontar maná utilizado.
            _personajeMana.UsarMana(ArmaEquipada.ManaRequerida);
        }
        else
        {
            float daño = ObtenerDaño();
            EnemigoVida enemigoVida = EnemigoObjetivo.GetComponent<EnemigoVida>();
            enemigoVida.RecibirDaño(daño);
            EventoEnemigoDañado?.Invoke(daño, enemigoVida);
        }
    }

    public float ObtenerDaño()
    {
        // Referencia daño del personaje.
        float cantidad = stats.Daño;
        // Si el valor random obtenido entre 1 y 0, está entre los
        // parámetros del porcentaje de crítico aumentamos el daño que hacemos al enemigo.
        if (UnityEngine.Random.value < stats.PorcentajeCritico / 100)
        {
            cantidad *= 2;
        }
        // Regresamos el daño que haremos al enemigo.
        return cantidad;
    }

    private IEnumerator IEEstablecerCondicionAtaque()
    {
        //Mediante la duración de atacando, vamos a mostrar esa animación de ataque.
        Atacando = true;
        yield return new WaitForSeconds(0.3f);
        //Cuando pasen 0.3 segundos reseteamos el valor de atacando a falso para llevarlo a posición de caminar o idle.
        Atacando = false;
    }

    // Método que permite poder equipar un arma.
    public void EquiparArma(ItemArma armaPorEquipar)
    {
        ArmaEquipada = armaPorEquipar.Arma;
        // Nos aseguramos que el arma equipada sea de tipo magia, ya que solamente
        // las armas de magia disparan proyectiles.
        if (ArmaEquipada.Tipo == TipoArma.Magia)
        {
            pooler.CrearPooler(ArmaEquipada.ProyectiPrefab.gameObject);
        }
        stats.AñadirBonusPorArma(ArmaEquipada);
    }

    //Para eliminar un arma hay que verificar que tenemos un arma equipada.
    public void RemoverArma()
    {
        if (ArmaEquipada == null)
        {
            return;
        }
        // Para destruir el pooler verificamos que el arma equipada sea de magia.
        if (ArmaEquipada.Tipo == TipoArma.Magia)
        {
            pooler.DestruirPooler();
        }

        // Eliminamos el bonus del arma desequipada.
        stats.RemoverBonusPorArma(ArmaEquipada);
        ArmaEquipada = null;
    }

    public void ObtenerDireccionDisparo()
    {
        // Necesitamos conocer a que dirección se mueve el personaje.
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (input.x > 0.1f)
        {
            // Dentro del array la posición de ataque del personaje a la izquierda tiene indice 1. (En Unity)
            indexDireccionDisparo = 1;
        }
        else if (input.x < 0f)
        {
            // Ataque personaje derecha.
            indexDireccionDisparo = 3;
        }
        else if (input.y > 0.1f)
        {
            // Ataque personaje arriba.
            indexDireccionDisparo = 0;
        }
        else if (input.y < 0f)
        {
            // Ataque personaje abajo.
            indexDireccionDisparo = 2;
        }
    }

    private void EnemigoRangoSeleccionado(EnemigoInteraccion enemigoSeleccionado)
    {
        // Establecemos un enemigo que sea seleccionado si tenemos un arma equipada
        // si el arma equipada es de tipo magia.
        if (ArmaEquipada == null) { return; }
        if (ArmaEquipada.Tipo != TipoArma.Magia) { return; }
        // Si el enemigo objetivo que tenemos como referencia (prop) es igual al enemigo seleccionado
        // volvemos también. Ya que si el enemigo ya está seleccionado no se le puede seleccionar
        // de nuevo.
        if (EnemigoObjetivo == enemigoSeleccionado) { return; }
        //Si ninguno es el caso, establecemos la referencia de enemigo objetivo.
        EnemigoObjetivo = enemigoSeleccionado;
        EnemigoObjetivo.MostrarEnemigoSeleccionado(true, TipoDeteccion.Rango);
    }

    private void EnemigoNoSeleccionado()
    {
        // Para perder la referencia (target) debemos tener un enemigo previamente seleccionado
        // Si no hay un enemigo seleccionado, volvemos.
        if (EnemigoObjetivo == null) { return; }
        // Si hay un enemigo seleccionado ocultamos la selección (target)
        EnemigoObjetivo.MostrarEnemigoSeleccionado(false, TipoDeteccion.Rango);
        // Perdemos la referencia de selección.
        EnemigoObjetivo = null;
    }
    private void EnemigoMeleeDetectado(EnemigoInteraccion enemigoDetectado)
    {
        if (ArmaEquipada == null) { return; }
        if (ArmaEquipada.Tipo != TipoArma.Melee) { return; }
        // Como usamos detección por medio de colisiones, no hace falta verificar si habíamos seleccionado al mismo enemigo.
        EnemigoObjetivo = enemigoDetectado;
        EnemigoObjetivo.MostrarEnemigoSeleccionado(true, TipoDeteccion.Melee);
    }

    private void EnemigoMeleePerdido()
    {
        if (ArmaEquipada == null) { return; }
        if (EnemigoObjetivo == null) { return; }
        if (ArmaEquipada.Tipo != TipoArma.Melee) { return; }
        // Dejamos de mostrar la marca de target de melee.
        EnemigoObjetivo.MostrarEnemigoSeleccionado(false, TipoDeteccion.Melee);
        // Perdemos la referencia de selección.
        EnemigoObjetivo = null;
    }

    // Nos subscribimos a los eventos.
    private void OnEnable()
    {
        SeleccionManager.EventoEnemigoSeleccionado += EnemigoRangoSeleccionado;
        SeleccionManager.EventoObjetoNoSeleccionado += EnemigoNoSeleccionado;
        PersonajeDetector.EventoEnemigoDetectado += EnemigoMeleeDetectado;
        PersonajeDetector.EventoEnemigoPerdido += EnemigoMeleePerdido;
    }

    // Nos desubscribimos de los eventos subscritos.
    private void OnDisable()
    {
        SeleccionManager.EventoEnemigoSeleccionado -= EnemigoRangoSeleccionado;
        SeleccionManager.EventoObjetoNoSeleccionado -= EnemigoNoSeleccionado;
        PersonajeDetector.EventoEnemigoDetectado -= EnemigoMeleeDetectado;
        PersonajeDetector.EventoEnemigoPerdido -= EnemigoMeleePerdido;
    }
}
