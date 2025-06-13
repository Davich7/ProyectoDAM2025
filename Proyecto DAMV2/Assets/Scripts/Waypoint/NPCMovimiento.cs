using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovimiento : WaypointMovimiento
{
    //Necesitamos establecer hacia donde se mueve el NPC.
    [SerializeField] private DireccionMovimiento direccion;

    private readonly int caminarAbajo = Animator.StringToHash("CaminarAbajo");

    protected override void RotarPersonaje()
    {
        if (direccion != DireccionMovimiento.Horizontal)
        {
            return;
        }
        //Comparamos última posición con punto por mover. coordenada x hace referencia a movimiento a la derecha.

        if (PuntoPorMoverse.x > ultimaPosicion.x)
        {
            //Girar derecha.
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            //Girar izquierda.
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    protected override void RotarVertical()
    {
        if (direccion != DireccionMovimiento.Vertical)
        {
            return;
        }
        //NPC se mueve hacia arriba.
        if (PuntoPorMoverse.y > ultimaPosicion.y)
        {
            _animator.SetBool(caminarAbajo, false);
        }
        //NPC se mueve hacia abajo.
        else
        {
            _animator.SetBool(caminarAbajo, true);
        }
    }
}
