using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    // Queremos que varias clases tengan su propia instancia y singleton, esta debe de ser heredada por cualquier clase, lo hacemos añadiendo <T>
    //T hace referencia a la clase que está heredando.
    private static T _instance;
    //Regresamos la instancia de la clase que está heredando
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
            //Devolvemos la instancia que hemos encontrado. Esta instancia será inicializada a través del método Awake()
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        _instance = this as T;
    }
}
