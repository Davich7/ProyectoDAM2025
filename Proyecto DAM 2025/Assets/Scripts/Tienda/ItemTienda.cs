using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTienda : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private Image itemIcono;
    [SerializeField] private TextMeshProUGUI itemNombre;
    [SerializeField] private TextMeshProUGUI itemCosto;
    [SerializeField] private TextMeshProUGUI cantidadPorComprar;

    // Guardamos referencia del item cargado en panel.
    public ItemVenta ItemCargado { get; private set; }

    private int cantidad;
    // Costo de comprar el item.
    private int costoInicial;
    private int costoActual;

    private void Update()
    {
        cantidadPorComprar.text = cantidad.ToString();
        itemCosto.text = costoActual.ToString();
    }


    public void ConfigurarItemVenta(ItemVenta itemVenta)
    {
        ItemCargado = itemVenta;
        // Actualizamos información panel.
        itemIcono.sprite = itemVenta.Item.Icono;
        itemNombre.text = itemVenta.Nombre;
        itemCosto.text = itemVenta.Costo.ToString();
        cantidad = 1;
        costoInicial = itemVenta.Costo;
        costoActual = itemVenta.Costo;
    }

    public void ComprarItem()
    {
        // Verificamos que tenemos monedas suficientes para gastar el costoActual del item.
        if (MonedasManager.Instance.MonedasTotales >= costoActual)
        {
            Inventario.Instance.AñadirItem(ItemCargado.Item, cantidad);
            // Compramos el item
            MonedasManager.Instance.RemoverMonedas(costoActual);
            cantidad = 1;
            // Actualizamos las monedas.
            costoActual = costoInicial;
        }
    }

    public void SumarItemPorComprar()
    {
        // Guardamos lo que supone comprar el item + 1.
        int costoDeCompra = costoInicial * (cantidad + 1);
        // Verificamos si tenemos el dinero suficiente para comprar esa cantidad.
        if (MonedasManager.Instance.MonedasTotales >= costoDeCompra)
        {
            // Actualizamos la cantidad.
            cantidad++;
            costoActual = costoInicial * cantidad;
        }
    }

    // Restamos la cantidad del item.
    public void RestarItemPorComprar()
    {
        if (cantidad == 1)
        {
            return;
        }

        cantidad--;
        costoActual = costoInicial * cantidad;
    }
}
