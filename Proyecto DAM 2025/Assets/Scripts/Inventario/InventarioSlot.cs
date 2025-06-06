using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public enum TipoDeInteraccion
{
    Click,
    Usar,
    Equipar,
    Remover
}
public class InventarioSlot : MonoBehaviour
{
    //Lanzamos el evento compartiendo una referencia del tipo de interacción que estamos haciendo y el index presionado
    public static Action <TipoDeInteraccion, int> EventoSlotInteraccion;

    [SerializeField] private Image itemIcono;
    [SerializeField] private GameObject fondoCantidad;
    [SerializeField] private TextMeshProUGUI cantidadTMP;
    //Creamos propiedad para almacenar el índice del slot.
    public int Index { get; set; }

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    //Método que permita actualizar el icono y el texto
    public void ActualizarSlot(InventarioItem item, int cantidad)
    {
        itemIcono.sprite = item.Icono;
        cantidadTMP.text = cantidad.ToString();
    }

    //En función si el valor es true o false, se activa o se desactiva.
    public void ActivarSlotUI(bool estado)
    {
        itemIcono.gameObject.SetActive(estado);
        fondoCantidad.SetActive(estado);
    }

    public void SeleccionarSlot()
    {
        _button.Select();
    }

    //Creamos método para lanzar el evento.
    public void ClickSlot()
    {
        /*Lanzamos el evento de interacción, si no es nulo se invoca.
        Este evento se escuchará en InventarioUI*/
        EventoSlotInteraccion?.Invoke(TipoDeInteraccion.Click, Index);

        
        /* Si tiene un valor diferente a -1 que es el valor predeterminado, significa
        que tenemos un slot seleccionado*/
        if (InventarioUI.Instance.IndexSlotInicialPorMover != -1)
        {
            if (InventarioUI.Instance.IndexSlotInicialPorMover != Index)
            {
                //MOVER ITEM
                Inventario.Instance.MoverItem(InventarioUI.Instance.IndexSlotInicialPorMover, Index);
            }

        }
    }
    
    public void SlotUsarItem()
    {
        //Verificamos que tenemos un item en slot para lanzar el evento.
        if (Inventario.Instance.ItemsInventario[Index] != null)
        {
            //Lanzamos el Evento si no es nulo lo invocamos y el tipo de interacción será de usar, posteriormente pasamos el index del slot.
            EventoSlotInteraccion?.Invoke(TipoDeInteraccion.Usar, Index);
        }

    }
}
