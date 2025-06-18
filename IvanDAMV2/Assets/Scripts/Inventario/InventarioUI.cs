using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventarioUI : Singleton<InventarioUI>
{
    [Header("Panel Inventario Descripcion")]
    //Referencia de panel dentro del inventario.
    [SerializeField] private GameObject panelInventarioDescripcion;
    //Referencia del icono del item.
    [SerializeField] private Image itemIcono;
    [SerializeField] private TextMeshProUGUI itemNombre;
    [SerializeField] private TextMeshProUGUI itemDescripcion;
    
    [SerializeField] private InventarioSlot slotPrefab;
    [SerializeField] private Transform contenedor;

    public int IndexSlotInicialPorMover { get; private set; }

    //Obtener referencia del slot que seleccionamos, creamos propiedad que contenga esa informaci�n.
    public InventarioSlot SlotSeleccionado { get; private set; }

    //Guardar informaci�n de todos los slots que hemos creado.
    List<InventarioSlot> slotsDisponibles = new List<InventarioSlot>();
    private void Start()
    {
        InicializarInventario();
        IndexSlotInicialPorMover = -1;
    }
    private void Update()
    {
        ActualizarSlotSeleccionado();
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (SlotSeleccionado != null)
            {
                IndexSlotInicialPorMover = SlotSeleccionado.Index;
            }
            
        }
    }

    //M�todo para poder instanciar todos los slots en el contenedor.
    private void InicializarInventario()
    {
        for (int i = 0; i < Inventario.Instance.NumeroDeSlots; i++)
        {
            //Debido a que slotPrefab es de tipo inventario podemos referenciar el slot creado con Instantiate en la variable nuevoSlot y a�adirlo a slotsDisponibles.
            InventarioSlot nuevoSlot = Instantiate(slotPrefab, contenedor);
            //De este modo otorgaremos el �ndice a cada slot creado.
            nuevoSlot.Index = i;
            slotsDisponibles.Add(nuevoSlot);
       }
    }

    public void ActualizarSlotSeleccionado()
    {
        //Clase que regresa el objecto actual seleccionado en el editor.
        GameObject goSeleccionado = EventSystem.current.currentSelectedGameObject;
        if (goSeleccionado == null)
        {
            return;
        }
        //Si hay un objeto seleccionado hay que verificar si ese objeto tiene la clase de inventario slot.
        InventarioSlot slot = goSeleccionado.GetComponent<InventarioSlot>();
        if (slot != null)
        {
            SlotSeleccionado = slot;
        }
    }
    public void DibujarItemEnInventario(InventarioItem itemPorA�adir, int cantidad, int itemIndex)
    {
        //Se iguala el index al slot y viceversa.
        InventarioSlot slot = slotsDisponibles[itemIndex];
        if (itemPorA�adir != null) 
        {
            //Activamos el icono y el texto que tiene.
            slot.ActivarSlotUI(true);
            slot.ActualizarSlot(itemPorA�adir, cantidad);
        }
        //Si no, se desactiva la interfaz para ese slot.
        else
        {
            slot.ActivarSlotUI(false);
        }
    }

    private void ActualizarInventarioDescripcion(int index)
    {
        //Si hay alg�n item en el Index del Slot podremos actualizar el panel de descripci�n del inventario.
        if (Inventario.Instance.ItemsInventario[index] != null)
        {
            itemIcono.sprite = Inventario.Instance.ItemsInventario[index].Icono;
            itemNombre.text = Inventario.Instance.ItemsInventario[index].Nombre;
            itemDescripcion.text = Inventario.Instance.ItemsInventario[index].Descripcion;
            //Activamos el panel de inventario descripci�n que por defecto est� desactivado.
            panelInventarioDescripcion.SetActive(true);
        }
        else
        {
            //Si no hay item desactivamos el panel de inventario descripci�n.
            panelInventarioDescripcion.SetActive(false);
        }
    }

    //M�todo que ser� llamado con el bot�n usar del inventario.
    public void UsarItem()
    {
        if (SlotSeleccionado != null)
        {
            SlotSeleccionado.SlotUsarItem();
            SlotSeleccionado.SeleccionarSlot();
        }
    }

    public void EquiparItem()
    {
        //Verificamos que tenemos seleccionado un slot en el inventario.
        if (SlotSeleccionado != null)
        {
            SlotSeleccionado.SlotEquiparItem();
            //Si le hemos dado al bot�n equipar hay que seleccionar el slot que ten�amos
            //seleccionado antes.
            SlotSeleccionado.SeleccionarSlot();
        }

    }
    public void RemoverItem()
    {
        //Verificamos que tenemos seleccionado un slot en el inventario.
        if (SlotSeleccionado != null)
        {
            SlotSeleccionado.SlotRemoverItem();
            //Si le hemos dado al bot�n eliminar hay que seleccionar el slot que ten�amos
            //seleccionado antes.
            SlotSeleccionado.SeleccionarSlot();
        }

    }

    #region Evento

    //M�todo que acutualiza el inventario en funci�n del index que ha sido seleccionado.
    private void SlotInteraccionRespuesta(TipoDeInteraccion tipo, int index)
    {
        if (tipo == TipoDeInteraccion.Click)
        {
            ActualizarInventarioDescripcion(index);

        }
    }
    private void OnEnable()
    {
        InventarioSlot.EventoSlotInteraccion += SlotInteraccionRespuesta;
    }

    private void OnDisable()
    {
        InventarioSlot.EventoSlotInteraccion -= SlotInteraccionRespuesta;
    }

    #endregion
}
