using System;
using UnityEngine;

public class SeleccionManager : MonoBehaviour
{
    //Se quiere notificar al enemigo en particular seleccionado. 
    public static Action<EnemigoInteraccion> EventoEnemigoSeleccionado;
    public static Action EventoObjetoNoSeleccionado;

    //Propiedad donde se guarda al enemigo seleccionado.

    public EnemigoInteraccion EnemigoSeleccionado { get; set; }

    //Para seleccionar al enemigo usaremos Ray Casting. Se lanza rayo desde la camara hasta la posicion del mouse
    //para saber si se hizo clic en algún enemigo.

    //Obtenemos una referencia de la camara
    private Camera camara;
    private void Start()
    {
        camara = Camera.main;
    }

    private void Update()
    {
        SeleccionarEnemigo();
    }

    private void SeleccionarEnemigo()
    {
        // Para seleccionar un enemigo hay que verificar si estamos haciendo clic.
        // Si se apreta el botón izquierdo del ratón continuamos lógica de selección.
        if (Input.GetMouseButtonDown(0))
        {
            // El método Raycast, nos pide de que origen se quiere lanzar el rayo (posición del ratón), hacia que dirección
            // distancia y cual es el layer mask que interesa conocer.
            RaycastHit2D hit = Physics2D.Raycast(camara.ScreenToWorldPoint(Input.mousePosition), 
                Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Enemigo"));
            //Si en la variable hit hemos guardado la colisión con el enemigo con el Raycast haciendo clic.
            if (hit.collider != null)
            {
                //Obtenemos el componente de Enemigo Interacción del objeto colisionado (hit).
                //Referencia del enemigo que hemos seleccionado.
                EnemigoSeleccionado = hit.collider.GetComponent<EnemigoInteraccion>();
                EnemigoVida enemigoVida = EnemigoSeleccionado.GetComponent<EnemigoVida>();
                if (enemigoVida.Salud > 0f)
                {
                    // Notificamos evento.
                    EventoEnemigoSeleccionado?.Invoke(EnemigoSeleccionado);
                }
                else
                {
                    EnemigoLoot loot = EnemigoSeleccionado.GetComponent<EnemigoLoot>();
                    LootManager.Instance.MostrarLoot(loot);
                }
            } 
            else
                // Si no se ha seleccionado con el Raycast a un enemigo, llamamos a objeto no seleccionado.
            {
                EventoObjetoNoSeleccionado?.Invoke();
            }
        }
    }
    // Evento que notifique a que enemigo se ha seleccionado para que el enemigo
    // pueda mostrar su marca de selección.

    // Evento que notifique que no se ha seleccionado a ningún enemigo. Se seleccionó
    // alguna otra parte de la escena. Este evento nos servirá para perder la referencia
    // de enemigo seleccionado.
}
