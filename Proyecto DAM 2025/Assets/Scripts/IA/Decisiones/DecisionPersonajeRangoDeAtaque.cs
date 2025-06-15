using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "IA/Decisiones/Personaje en Rango de Ataque")]
//Decision que indica si podemos hacer la transición de un estado al estado de atacarpersonaje.
public class DecisionPersonajeRangoDeAtaque : IADecision
{
    public override bool Decidir(IAController controller)
    {
        return EnRangoDeAtaque(controller);
    }

    private bool EnRangoDeAtaque(IAController controller)
    {
        if (controller.PersonajeReferencia == null)
        {
            return false;
        }

        float distancia = (controller.PersonajeReferencia.position - 
                           controller.transform.position).sqrMagnitude;

        if (distancia < Mathf.Pow(controller.RangoDeAtaqueDeterminado, 2))
        {
            return true;
        }

        return false;
    }
}
