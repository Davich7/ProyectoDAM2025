using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ZonaConfiner : MonoBehaviour
{
    //Creamos nuestra variable.
    [SerializeField] private CinemachineVirtualCamera camara;

    //Detectar si el personaje entra o sale del confiner.

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            camara.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            camara.gameObject.SetActive(false);
        }
    }
}
