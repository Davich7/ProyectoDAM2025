
using UnityEngine;

public class Proyectil : MonoBehaviour
{
    [Header("Config")]
    // Velocidad del proyectil.
    [SerializeField] private float velocidad;

    // Para inicializar la propiedad PersonajeAtaque, otorgamos privado el set.
    public PersonajeAtaque PersonajeAtaque { get; private set; }

    private Rigidbody2D _rigidbody2D;
    private Vector2 direccion;
    // Referencia del enemigo que dañará el proyectil.
    private EnemigoInteraccion enemigoObjetivo;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>(); 
    }

    private void FixedUpdate()
    {
        if (enemigoObjetivo == null)
        {
            return;
        }
        MoverProyectil();
    }

    private void MoverProyectil()
    {
        direccion = enemigoObjetivo.transform.position - transform.position;
        //Angulo para la rotación.
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angulo, Vector3.forward);
        //Movemos proyectil
        _rigidbody2D.MovePosition(_rigidbody2D.position + direccion.normalized * velocidad * Time.fixedDeltaTime);
    }

    // Método que permite restablecer referencia del proyectil.
    public void InicializarProyectil(PersonajeAtaque ataque)
    {
        PersonajeAtaque = ataque;
        enemigoObjetivo = ataque.EnemigoObjetivo;
    }

    // Método para colisionar con el enemigo.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemigo"))
        {
            float daño = PersonajeAtaque.ObtenerDaño();
            EnemigoVida enemigoVida = enemigoObjetivo.GetComponent<EnemigoVida>();
            //Realizamos daño a nuestros enemigos.
            enemigoVida.RecibirDaño(daño);
            PersonajeAtaque.EventoEnemigoDañado?.Invoke(daño, enemigoVida);
            gameObject.SetActive(false);
        }
    }
}

