using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : Singleton<LootManager>
{
    [Header("Config")]
    [SerializeField] private GameObject panelLoot;
    [SerializeField] private LootButton lootButtonPrefab;
    [SerializeField] private Transform lootContenedor;

    public void MostrarLoot(EnemigoLoot enemigoLoot)
    {
        panelLoot.SetActive(true);
        // Destruimos el panel si ya se había abierto anteriormente.
        if (ContenedorOcupado())
        {
            // Si contenedor está ocupado, destruiremos cada item que está esperando por ser loteado.
            foreach (Transform hijo in lootContenedor.transform)
            {
                Destroy(hijo.gameObject);
            }
        }
        // Cargamos el loot al panel.
        for (int i = 0; i < enemigoLoot.LootSeleccionado.Count; i++)
        {
            CargarLootPanel(enemigoLoot.LootSeleccionado[i]);
        }
    }

    public void CerrarPanel()
    {
        panelLoot.SetActive(false);
    }
    private void CargarLootPanel(DropItem dropItem)
    {
        if (dropItem.ItemRecogido)
        {
            return;
        }
        LootButton loot = Instantiate(lootButtonPrefab, lootContenedor);
        loot.ConfigurarLootItem(dropItem);
        loot.transform.SetParent(lootContenedor);
    }

    private bool ContenedorOcupado()
    {
        //Obtenemos la cantidad de hijos que hay en el contenedor.
        LootButton[] hijos = lootContenedor.GetComponentsInChildren<LootButton>();
        if (hijos.Length > 0)
        {
            return true;
        }
        return false;
    }
}
