using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Otorgamos abstracto para que toda clase que heredere de IAAccion implemente su método principal
//Para poder crear una acción tenemos que heredar de ScriptableObject.
public abstract class IAAccion : ScriptableObject
{
    //Para ejecutar una acción tenemos que tener la referencia de la clase principal de un enemigo (IAController).
    public abstract void Ejecutar(IAController controller);
    

    
}
