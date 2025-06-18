using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventario : Singleton<Inventario>
{
    //El tama�o del array ser� igual a la cantidad de slots que tendremos en nuestro inventario.
    [Header("Items")]
    [SerializeField] private InventarioItem[] itemsInventario;
    //Referencia de Personaje.
    [SerializeField] private Personaje personaje;
    [SerializeField] private int numeroDeSlots;

    //Creamos propiedad de personaje.
    public Personaje Personaje => personaje;
    //Devuelve el valor de la variable.
    public int NumeroDeSlots => numeroDeSlots;
    //Propiedad que regresa el array de itemsInventario.
    public InventarioItem[] ItemsInventario => itemsInventario;

    private void Start()
    {
        itemsInventario = new InventarioItem[numeroDeSlots];
    }

    //Cuando intentamos a�adir un item lo que hacemos es verificar si en el inventario existen items similares para aumentarlos, si no lo a�adiremos en slot vac�o.
    public void A�adirItem(InventarioItem itemPorA�adir, int cantidad)
    {
        if (itemPorA�adir == null) 
        {
            return;
        }
        /*En esta lista guardamos todos los �ndices de los items que son similares de los que estamos tratando a�adir.
        VERIFICACION EN CASO DE TENER ITEM SIMILAR EN INVENTARIO*/
        List<int> indexes = VerificarExistencias(itemPorA�adir.ID);
        if (itemPorA�adir.EsAcumulable)
        {
            if (indexes.Count > 0) 
            {
                for (int i = 0; i < indexes.Count; i++) 
                {
                    //Si cualquier slot por ejemplo el 0 su cantidad no ha superado la acumulaci�n m�xima hay que ir a�adiendo la cantidad recogida.
                    if (itemsInventario[indexes[i]].Cantidad < itemPorA�adir.AcumulacionMax) 
                    {
                       //A�adimos la cantidad a dicho item.
                       itemsInventario[indexes[i]].Cantidad += cantidad;
                       //Si hemos superado la acumulaci�n m�xima de un item obtenemos la diferencia para a�adir el item en otro slot.
                       if (itemsInventario[indexes[i]].Cantidad > itemPorA�adir.AcumulacionMax) 
                       {    
                            //Una vez el item ha superado la cantidad m�xima, obtenemos la diferencia que es lo restante que no tiene que estar en ese slot.
                            int diferencia = itemsInventario[indexes[i]].Cantidad - itemPorA�adir.AcumulacionMax;
                            //Establecemos su cantidad a su acumulaci�n m�xima para que no pase de ah�.
                            itemsInventario[indexes[i]].Cantidad = itemPorA�adir.AcumulacionMax;
                            //Volvemos a llamar al m�todo ahora con la diferencia.
                            A�adirItem(itemPorA�adir, diferencia);
                       }
                        InventarioUI.Instance.DibujarItemEnInventario(itemPorA�adir,
                            itemsInventario[indexes[i]].Cantidad, indexes[i]);
                        return;
                    }   
                }
            }
        }

        if (cantidad <= 0)
        {
            return;
        }

        if (cantidad > itemPorA�adir.AcumulacionMax)
        {
            A�adirItemEnSlotDisponible(itemPorA�adir, itemPorA�adir.AcumulacionMax);
            //Actualizamos la cantidad que falta al actualizar en slot vac�o.
            cantidad -= itemPorA�adir.AcumulacionMax;
            //Se a�ade la cantidad restante.
            A�adirItem(itemPorA�adir, cantidad);
        }
        else
        {
            //Cuando la cantidad que se quiere a�adir es superior a la cantidad m�xima del item, se a�ade la cantidad restante en slot vac�o disponible. 
            A�adirItemEnSlotDisponible(itemPorA�adir, cantidad);
        }
    }
    //M�todo para obtener la referencia de los items. El index del item es el mismo que el n�mero de slot en el que se encuentra.
    private List<int> VerificarExistencias(string itemID) 
    {
        List<int> indexesDelItem = new List<int>();
        //Poder recorrer inventario y encontrar item con espec�fico ID.
        for (int i = 0; i < itemsInventario.Length; i++) 
        {
            if (itemsInventario[i] != null)
            {
                if (itemsInventario[i].ID == itemID)
                {
                    indexesDelItem.Add(i);
                }
            }    
        }
        return indexesDelItem;  
    }

    public int ObtenerCantidadDeItems(string itemID)
    {
        List<int> indexes = VerificarExistencias (itemID);
        // Recorremos todos los index y sumamos la cantidad de itemID a la total.
        int cantidadTotal = 0;
        foreach (int index in indexes)
        {
            // Check para verificar que el item guardado en la lista tien el ID.
            if (itemsInventario[index].ID == itemID)
            {
                cantidadTotal += itemsInventario[index].Cantidad;
            }
        }
        return cantidadTotal;
    }

    public void ConsumirItem(string itemID)
    {
        // Se necesita saber de que item se trata.
        List<int> indexes = VerificarExistencias(itemID);
        if (indexes.Count > 0)
        {
            EliminarItem(indexes[indexes.Count - 1]);
        }
    }

    //M�todo para a�adir un item en un slot vac�o.
    private void A�adirItemEnSlotDisponible(InventarioItem item, int cantidad)
    {
        //Recorremos todo el inventario
        for (int i = 0;i < itemsInventario.Length; i++)
        {
            if (itemsInventario[i] == null) 
            {
                //Crear� nueva instancia del item en lugar de solo poner la referencia.
                itemsInventario[i] = item.CopiarItem();
                itemsInventario[i].Cantidad = cantidad;
                InventarioUI.Instance.DibujarItemEnInventario(item, cantidad, i);
                //Una vez que a�adimos un item salimos del bucle para no recorrerlo.
                return;
            }
        }
    }

    
    private void EliminarItem(int index)
    {
        //Reducimos la cantidad del item en 1.
        ItemsInventario[index].Cantidad--;

        //Verificaci�n si la cantidad est� por debajo de 0.
        if (itemsInventario[index].Cantidad <= 0)
        { 
            itemsInventario[index].Cantidad = 0;
            //Hacemos que el item no exista al no tener cantidades.
            itemsInventario[index] = null;
            //Actualizar el slot donde se indica que ya no tenemos el item si ha llegado a 0 o menos.
            InventarioUI.Instance.DibujarItemEnInventario(null, 0, index);
        }
        //Si la cantidad del item que estamos eliminando no es menor o igual que 0, actualizamos su informaci�n dentro del inventario.
        else
        {
            InventarioUI.Instance.DibujarItemEnInventario(itemsInventario[index],
                itemsInventario[index].Cantidad, index);
        }
    }

    public void MoverItem(int indexInicial, int indexFinal)
    {
        //Verificamos si en el slot inicial de donde moveremos el item no es nulo. Tambi�n verificaremos que el
        //Slot a donde se mover� no est� ocupado.
        if (itemsInventario[indexInicial] == null || itemsInventario[indexFinal] != null)
        {
            return;
        }

        //Copiar item en slot final
        InventarioItem itemPorMover = itemsInventario[indexInicial].CopiarItem();
        ItemsInventario[indexFinal] = itemPorMover;
        InventarioUI.Instance.DibujarItemEnInventario(itemPorMover, itemPorMover.Cantidad, indexFinal);

        //Borramos Item de Slot Inicial.
        itemsInventario[indexInicial] = null;
        InventarioUI.Instance.DibujarItemEnInventario(null, 0, indexInicial);
    }

    //Para poder usar un item, necesitamos conocer el index en el que se encuentra.
    private void UsarItem(int index)
    {
        if (itemsInventario[index] == null)
        {
            return;
        }
        //Si se llega a utilizar el item, lo eliminamos.
        if (itemsInventario[index].UsarItem())
        {
            EliminarItem(index);
        }
    }

    private void EquiparItem(int index)
    {
        //Antes de equipar un objeto hay que verificar que existe el objeto.
        if (itemsInventario[index] == null)
        {
            return;
        }

        //Verificamos que el tipo de item que lanzamos el evento es de tipo Arma.
        //Ya que solo se pueden equipar items que son armas.

        if (itemsInventario[index].Tipo != TiposdeItem.Armas)
        {
            return;
        }
        //Si no se cumplen las condiciones anteriores, equipamos item como arma.
        itemsInventario[index].EquiparItem();
    }

    private void RemoverItem(int index)
    {
        //Para poder eliminar un item, hay que verificar que hay un item en ese slot.
        if (itemsInventario[index] == null)
        {
            return;
        }
        //Cuando eliminamos el item, este tiene que ser de tipo Armas.
        if (itemsInventario[index].Tipo != TiposdeItem.Armas)
        {
            return;
        }
        //Si es un arma
        itemsInventario[index].RemoverItem();
    }

    #region Eventos

    //Respuesta de cada bot�n habilitado en el inventario.
    private void SlotInteraccionRespuesta(TipoDeInteraccion tipo, int index)
    {
        switch (tipo)
        {
            case TipoDeInteraccion.Usar:
                UsarItem(index);
                break;
            case TipoDeInteraccion.Equipar:
                EquiparItem(index);
                break;
            case TipoDeInteraccion.Remover:
                RemoverItem(index);
                break;
        }
    }

    //Escuchar evento
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
