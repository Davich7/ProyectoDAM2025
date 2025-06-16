using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstracto para que toda decision que creemos heredere de la clase base IADecision y pueda implementar el método principal.
public abstract class IADecision : ScriptableObject
{
    //Decision devuelve verdadero o falso.
    public abstract bool Decidir(IAController controller);
}
