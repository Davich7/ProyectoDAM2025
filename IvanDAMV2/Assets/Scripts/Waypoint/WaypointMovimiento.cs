using UnityEngine;
public enum DireccionMovimiento
{
    Horizontal,
    Vertical
}

public class WaypointMovimiento : MonoBehaviour
{
    [SerializeField] protected float velocidad;

    //En caso de querer actualizar el movimiento del personaje a otro punto de la ruta, deberemos actualizar el puntoActualIndex.
    public Vector3 PuntoPorMoverse => _waypoint.ObtenerPosicionMovimiento(puntoActualIndex);

    protected Waypoint _waypoint;
    protected Animator _animator;
    protected int puntoActualIndex;
    /*Necesitamos saber si el personaje se mueve a la izquierda o hacia la derecha.
    Esta variable ultima posición tiene que ser actualizada cada vez que llega a un punto.*/
    protected Vector3 ultimaPosicion;

    //El personaje se moverá al punto original de la ruta.
    private void Start()
    {
        puntoActualIndex = 0;
        _animator = GetComponent<Animator>();
        _waypoint = GetComponent<Waypoint>();
    }

    // Update is called once per frame
    private void Update()
    {
        MoverPersonaje();
        RotarPersonaje();
        RotarVertical();
        if (ComprobarPuntoActualAlcanzado())
        {
            ActualizarIndexMovimiento();
        }
    }

    private void MoverPersonaje()
    {
        //De este modo el personaje se mueve al punto que debe ir. 
        transform.position = Vector3.MoveTowards(transform.position, PuntoPorMoverse
            , velocidad * Time.deltaTime);
    }

    //Este método nos devuelve si ya hemos llegado al punto al que nos estamos moviendo.
    private bool ComprobarPuntoActualAlcanzado()
    {
        //Accedemos a la magnitud que es la longitud del vector el que nos da la distancia.
        float distanciaHaciaPuntoActual = (transform.position - PuntoPorMoverse).magnitude;
        /*Si la distancia es menor a 0.1 que significa que estamos cerca al punto que nos estamos moviendo
        devolvemos verdadero indicando que alcanzado el punto actual.*/
        if (distanciaHaciaPuntoActual < 0.1f)
        {
            //La última posición es igual a la posición actual.
            ultimaPosicion = transform.position;
            return true;
        }
        //Si no hemos alcanzado el punto actual devolvemos falso.
        return false;
    }

    private void ActualizarIndexMovimiento()
    {
        /*Si nuestro punto actual llega al último punto de la ruta, hay que resetearlo a 0.
        Esto hace que se muevan en loop*/
        if (puntoActualIndex == _waypoint.Puntos.Length - 1)
        {
            puntoActualIndex = 0;
        }
        else
        {
            //Actualizamos el index sin pasarnos del último punto del array.
            if (puntoActualIndex < _waypoint.Puntos.Length - 1)
            {
                puntoActualIndex++;
            }
        }
    }

    //Hay que verificar si el personaje utiliza una dirección en horizontal.
    protected virtual void RotarPersonaje()
    {

    }

    protected virtual void RotarVertical()
    {

    }
}
