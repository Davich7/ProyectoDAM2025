using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TiposDeAtaque
{
    Melee,
    Embestida
}

//Clase principal del enemigo.
public class IAController : MonoBehaviour
{
    //Lanzamos el evento pero con la referencia de la cantidad de da�o aplicado al personaje.
    public static Action<float> EventoDa�oRealizado;

    //Antes de da�ar al personaje revisar estad�sticas de bloqueo.
    [Header("Stats")]
    [SerializeField] private PersonajeStats stats;

    //Para organizar el inspector.
    [Header("Estados")]
    [SerializeField] private IAEstado estadoInicial;
    [SerializeField] private IAEstado estadoDefault;

    
    [Header("Config")]
    [SerializeField] private float rangoDeteccion;
    [SerializeField] private float rangoDeAtaque;
    [SerializeField] private float rangoDeEmbestida;
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float velocidadDeEmbestida;
    //Layer mask de nuestro personaje, objeto que tiene que ser detectado por el enemigo.
    [SerializeField] private LayerMask personajeLayerMask;

    [Header("Ataque")]
    //Da�o que puede realizar al personaje.
    [SerializeField] private float da�o;
    [SerializeField] private float tiempoEntreAtaques;
    [SerializeField] private TiposDeAtaque tipoAtaque;

    //Mostramos el radio de detecci�n.
    [Header("Debug")]
    [SerializeField] private bool mostrarDeteccion;
    [SerializeField] private bool mostrarRangoAtaque;
    [SerializeField] private bool mostrarRangoEmbestida;

    private float tiempoParaSiguienteAtaque;
    private BoxCollider2D _boxCollider2D;

    //Creamos propiedad donde guardaremos la referencia del personaje detectado.
    public Transform PersonajeReferencia { get; set; }
    //Propiedad que controla todo sobre los estados del enemigo.
    public IAEstado EstadoActual { get; set; }
    //Obtenemos referencia de la clase de enemigoMovimiento para poder activar o desactivarlo. 
    public EnemigoMovimiento EnemigoMovimiento { get; set; }
    public float RangoDeteccion => rangoDeteccion;
    public float Da�o => da�o;
    public TiposDeAtaque TipoAtaque => tipoAtaque;
    public float VelocidadMovimiento => velocidadMovimiento;
    public LayerMask PersonajeLayerMask => personajeLayerMask;
    //Guardaremos el tipo de rango utilizado seg�n el tipo de ataque del enemigo.
    //Rango de Ataque determinado es igual si tipo de ataque es igual a Embestida,
    //A la propiedad RangoDeAtaqueDeterminado le daremos el valor [utilizando (?)] de rango de embestida.
    //Pero si tipo de ataque no es embestida sino que es melee, asignaremos el valor de rango de ataque.
    public float RangoDeAtaqueDeterminado => tipoAtaque == 
        TiposDeAtaque.Embestida ? rangoDeEmbestida : rangoDeAtaque;

    //Para inicializar el estado que queremos iniciar.
    private void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        EstadoActual = estadoInicial;
        //Obtenemos una referencia del movimiento del enemigo.
        EnemigoMovimiento = GetComponent<EnemigoMovimiento>();
    }

    //Ejecutamos las acciones del estado y sus transiciones.
    private void Update()
    {
        //Especificamos que la referencia del controller es este.
        EstadoActual.EjecutarEstado(this);
    }

    //Para cambiar el estado del enemigo pasamos como par�metro al estado al que queremos ir.
    public void CambiarEstado(IAEstado nuevoEstado)
    {
        //Si el estado al que estamos apuntando es diferente de estadoDefault hacemos la transici�n.
        if (nuevoEstado != estadoDefault)
        {
            EstadoActual = nuevoEstado;
        }
    }

    public void AtaqueMelee(float cantidad)
    {
        //Check adicional.
        if (PersonajeReferencia != null)
        {
            //Pasamos cantidad como da�o a realizar.
            AplicarDa�oAlPersonaje(cantidad);
        }
    }

    //M�todo que permite hacer ataque embestida.
    public void AtaqueEmbestida(float cantidad)
    {
        StartCoroutine(IEEmbestida(cantidad));
    }

    private IEnumerator IEEmbestida(float cantidad)
    {
        //Obtenemos la posici�n del personaje.
        Vector3 personajePosicion = PersonajeReferencia.position;
        //Posici�n inicial del enemigo desde donde embiste.
        Vector3 posicionInicial = transform.position;
        //Direcci�n a la que se har� la embestida.
        Vector3 direccionHaciaPersonaje = (personajePosicion - posicionInicial).normalized;
        //Embestida al personaje pero un poco alejado para no entrar en la posici�n del personaje.
        Vector3 posicionDeAtaque = personajePosicion - direccionHaciaPersonaje * 0.5f;
        //Desactivamos box collider.
        _boxCollider2D.enabled = false;

        float transicionDeAtaque = 0f;
        while (transicionDeAtaque <= 1f)
        {
            //Actualizamos la transici�n de ataque mediante la embestida.
            transicionDeAtaque += Time.deltaTime * velocidadMovimiento;
            //F�rmula de la par�bola utilizada. Har� que el enemigo vaya de su posici�n inicial a la posici�n del perrsonaje y luego regrese a su posici�n original.
            float interpolacion = (-Mathf.Pow(transicionDeAtaque, 2) + transicionDeAtaque) * 4f;
            //Actualizamos la posici�n del enemigo.
            transform.position = Vector3.Lerp(posicionInicial, posicionDeAtaque, interpolacion);
            //Esperamos un frame ya que estamos llamando a current time.
            yield return null;
        }

        //Verificamos si aun tenemos la referencia del personaje.
        if (PersonajeReferencia != null)
        {
            AplicarDa�oAlPersonaje(cantidad);
        }
        //Cuando ha aplicado da�o volvemos a activar su box collider.
        _boxCollider2D.enabled = true;
    }
    public void AplicarDa�oAlPersonaje(float cantidad)
    {
        float da�oPorRealizar = 0;
        //Como porcentaje de bloque se encuentra en un valor entre 0-100 dividimos entre 100 para obtener
        //un valor entre 0-1 que pueda ser comparado a Random.value.
        if (UnityEngine.Random.value < stats.PorcentajeBloqueo / 100)
        {
            return;
        }

        //Nos devuelve el valor mayor entre los par�metros indicados.
        //Indicamos que como m�nimo el enemigo realizar� 1 de da�o.
        da�oPorRealizar = Mathf.Max(cantidad - stats.Defensa, 1f);
        //Referencia de Personaje Vida, para acceder a RecibirDa�o y pasarle la cantidad.
        PersonajeReferencia.GetComponent<PersonajeVida>().RecibirDa�o(da�oPorRealizar);
        //Si EventoDa�oRealizado no es nulo, lo invocamos.
        EventoDa�oRealizado?.Invoke(da�oPorRealizar);
    }

    //Si podemos atacar al personaje, verificamos si estamos en rango de ataque al personaje.
    public bool PersonajeEnRangoDeAtaque(float rango)
    {
        //Para saber si estamos en rango hay que obtener la distancia del personaje, a lo que restamos la posici�n del enemigo.
        //Como nos interesa la distancia que queremos obtener indicamos .sqrmagnitude que es la longitud al cuadrado para comparar 2 posiciones.
        float distanciaHaciaPersonaje = (PersonajeReferencia.position - transform.position).sqrMagnitude;
        //Si la distancia al personaje es menor al rango (Personaje en rango) devolvemos true.
        if (distanciaHaciaPersonaje < Mathf.Pow(rango, 2))
        {
            return true;
        }
        return false;
    }


    public bool EsTiempoDeAtacar()
    {
        //Si el tiempo actual del juego es mayor al tiempo para el siguiente ataque, regresamos verdadero ya que es tiempo de atacar.
        if(Time.time > tiempoParaSiguienteAtaque)
        {
            return true;
        }
        return false;
    }

    //El tiempo para el siguiente ataque es igual al tiempo actual + tiempo entre ataques, as� actualizaremos la variable de tiempo para el siguiente ataque.
    public void ActualizarTiempoEntreAtaques()
    {
        tiempoParaSiguienteAtaque = Time.time + tiempoEntreAtaques;
    }

    //M�todo para pintar en debug y ver los l�mites de area para la detecci�n del enemigo.
    private void OnDrawGizmos()
    {
        if (mostrarDeteccion)
        {
            Gizmos.color = Color.green;
            //Radio marcado para detecci�n en esfera.
            Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
        }
        if (mostrarRangoAtaque)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, rangoDeAtaque);
        }
        if (mostrarRangoEmbestida)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, rangoDeEmbestida);
        }
    }
}
