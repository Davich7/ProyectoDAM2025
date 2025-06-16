using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraccion : MonoBehaviour
{
    [SerializeField] private GameObject npcButtonInteractuar;
    //Referencia del ScriptableObject que contiene este NPC en referencia a su dialogo.
    [SerializeField] private NPCDialogo npcDialogo;

    public NPCDialogo Dialogo => npcDialogo;

    //Para saber si estamos cerca del personaje o colisionando con �l a�adimos los m�todos

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Asociamos que este NPC es el que est� disponible cuando nos aproximamos y del que queremos cargar su informaci�n.
            DialogoManager.Instance.NPCDisponible = this;
            npcButtonInteractuar.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Cuando salimos del �rea del NPC decimos que ya no hay un NPC disponible.
            DialogoManager.Instance.NPCDisponible = null;
            npcButtonInteractuar.SetActive(false);
        }
    }
}
