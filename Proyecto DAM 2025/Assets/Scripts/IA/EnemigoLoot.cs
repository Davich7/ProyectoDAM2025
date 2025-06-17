using System.Collections.Generic;
using UnityEngine;

public class EnemigoLoot : MonoBehaviour
{
    [Header("Exp")]
    [SerializeField] private float expGanada;

    // Creamos array del loot disponible que tiene el enemigo al morir.
    [Header("Loot")]
    [SerializeField] private DropItem[] lootDisponible;

    // Creamos una lista de items que pudieron ser cargados en panel.
    private List<DropItem> lootSeleccionado = new List<DropItem>();
    public List<DropItem> LootSeleccionado => lootSeleccionado;
    public float ExpGanada => expGanada;

    private void Start()
    {
        SeleccionarLoot();
    }

    private void SeleccionarLoot()
    {
        // Para seleccionar loot recorremos el array de loot disponible.
        foreach (DropItem item in lootDisponible)
        {
            // Guardamos la probabilidad del item de aparecer.
            float probabilidad = Random.Range(0, 100);
            if (probabilidad <= item.PorcentajeDrop)
            {
                lootSeleccionado.Add(item);
            }
        }
    }
}

