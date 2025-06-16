using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Otorgamos abstracto para que toda clase que heredere de IAAccion implemente su m�todo principal
//Para poder crear una acci�n tenemos que heredar de ScriptableObject.
public abstract class IAAccion : ScriptableObject
{
    //Para ejecutar una acci�n tenemos que tener la referencia de la clase principal de un enemigo (IAController).
    public abstract void Ejecutar(IAController controller);
    

    
}
