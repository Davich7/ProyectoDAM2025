using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    // Queremos que varias clases tengan su propia instancia y singleton, esta debe de ser heredada por cualquier clase, lo hacemos a�adiendo <T>
    //T hace referencia a la clase que est� heredando.
    private static T _instance;
    //Regresamos la instancia de la clase que est� heredando
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    GameObject nuevoGO = new GameObject();
                    _instance = nuevoGO.AddComponent<T>();
                }
            }
            //Devolvemos la instancia que hemos encontrado. Esta instancia ser� inicializada a trav�s del m�todo Awake()
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        _instance = this as T;
    }
}
