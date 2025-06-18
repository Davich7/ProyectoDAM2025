using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform nuevaPos;

    // Verificaci�n que estamos colisionando con el personaje.
    private void OnTriggerEnter2D(Collider2D other)
    {
         if (other.CompareTag("Player"))
        {
            // Actualizamos posici�n personaje.
            other.transform.localPosition = nuevaPos.position;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
