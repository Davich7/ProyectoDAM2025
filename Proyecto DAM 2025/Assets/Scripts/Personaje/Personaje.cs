using UnityEngine;


public class Personaje : MonoBehaviour
{
    //Referencia a los Stats para poder agregar Atributo.
    [SerializeField] private PersonajeStats stats;

    //Creamos propiedades de otras clases para poder acceder a ellas.
    //Solamente obtenemos el valor en estas clases (priv).
    public PersonajeAtaque PersonajeAtaque { get; private set; }
    public PersonajeExperiencia PersonajeExperiencia {  get; private set; }
    public PersonajeVida PersonajeVida { get; private set; }
    public PersonajeAnimaciones PersonajeAnimaciones { get; private set; }
    public PersonajeMana PersonajeMana { get; private set; }
    private void Awake()
    {
        PersonajeAtaque = GetComponent<PersonajeAtaque>();
        PersonajeVida = GetComponent<PersonajeVida>();
        PersonajeAnimaciones = GetComponent<PersonajeAnimaciones>();
        PersonajeMana = GetComponent<PersonajeMana>();
        PersonajeExperiencia = GetComponent<PersonajeExperiencia>();
    }

    public void RestaurarPersonaje()
    {
        PersonajeVida.RestaurarPersonaje();
        PersonajeAnimaciones.RevivirPersonaje();
        PersonajeMana.RestablecerMana();
    }

    /*Dentro del método se deberá de incluir el parámetro del tipo de atributo que se está referenciando. La referencia del evento concuerda con el método 
    respuesta creado*/
    private void AtributoRespuesta(TipoAtributo tipo)
    {
        //Solo queremos añadir los atributos si tenemos los puntos suficientes.
        if (stats.PuntosDisponibles <= 0)
        {
            return;
        }
        //Usamos switch para poder llamar algún método en específico en cada tipo de atributo.
        switch(tipo)
        {
            //Aumentamos en función del atributo seleccionado por cantidad de veces mejorada y el valor contenido de los atributos.
            case TipoAtributo.Fuerza:
                stats.Fuerza++;
                stats.AñadirBonusPorAtributoFuerza();
                break;
            case TipoAtributo.Inteligencia:
                stats.Inteligencia++;
                stats.AñadirBonusPorAtributoInteligencia();
                break;
            case TipoAtributo.Destreza:
                stats.Destreza++;
                stats.AñadirBonusPorAtributoDestreza();
                break;
        }
        stats.PuntosDisponibles -= 1;

    }
    //Escuchamos el evento AgregarAtributo.
    private void OnEnable()
    {
        AtributoButton.EventoAgregarAtributo += AtributoRespuesta;
    }
    private void OnDisable()
    {
        AtributoButton.EventoAgregarAtributo -= AtributoRespuesta;
    }
}

