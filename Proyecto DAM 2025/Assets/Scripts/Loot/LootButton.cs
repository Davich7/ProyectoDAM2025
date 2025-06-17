using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootButton : MonoBehaviour
{
    [SerializeField] private Image itemIcono;
    [SerializeField] private TextMeshProUGUI itemNombre;

    // Se guarda la referencia del item que cargar� de informaci�n en LootButton
    public DropItem ItemPorRecoger {  get; set; }

    // Actualizaremos el nombre y el icono cargado en el panel de loot.
    public void ConfigurarLootItem(DropItem dropItem)
    {
        // Definimos la propiedad
        ItemPorRecoger = dropItem;
        itemIcono.sprite = dropItem.Item.Icono;
        // Describimos del item el nombre y la cantidad.
        itemNombre.text = $"{dropItem.Item.Nombre}";
    }

    public void RecogerItem()
    {
        if (ItemPorRecoger == null)
        {
            return;
        }

        Inventario.Instance.A�adirItem(ItemPorRecoger.Item, ItemPorRecoger.Cantidad);
        ItemPorRecoger.ItemRecogido = true;
        // Destruimos tarjeta que contiene informaci�n del item.
        Destroy(gameObject);
    }
}
