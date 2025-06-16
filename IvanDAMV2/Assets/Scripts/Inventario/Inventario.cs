using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventario : Singleton<Inventario>
{
    //El tamaño del array será igual a la cantidad de slots que tendremos en nuestro inventario.
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

    //Cuando intentamos añadir un item lo que hacemos es verificar si en el inventario existen items similares para aumentarlos, si no lo añadiremos en slot vacío.
    public void AñadirItem(InventarioItem itemPorAñadir, int cantidad)
    {
        if (itemPorAñadir == null) 
        {
            return;
        }
        /*En esta lista guardamos todos los índices de los items que son similares de los que estamos tratando añadir.
        VERIFICACION EN CASO DE TENER ITEM SIMILAR EN INVENTARIO*/
        List<int> indexes = VerificarExistencias(itemPorAñadir.ID);
        if (itemPorAñadir.EsAcumulable)
        {
            if (indexes.Count > 0) 
            {
                for (int i = 0; i < indexes.Count; i++) 
                {
                    //Si cualquier slot por ejemplo el 0 su cantidad no ha superado la acumulación máxima hay que ir añadiendo la cantidad recogida.
                    if (itemsInventario[indexes[i]].Cantidad < itemPorAñadir.AcumulacionMax) 
                    {
                       //Añadimos la cantidad a dicho item.
                       itemsInventario[indexes[i]].Cantidad += cantidad;
                       //Si hemos superado la acumulación máxima de un item obtenemos la diferencia para añadir el item en otro slot.
                       if (itemsInventario[indexes[i]].Cantidad > itemPorAñadir.AcumulacionMax) 
                       {    
                            //Una vez el item ha superado la cantidad máxima, obtenemos la diferencia que es lo restante que no tiene que estar en ese slot.
                            int diferencia = itemsInventario[indexes[i]].Cantidad - itemPorAñadir.AcumulacionMax;
                            //Establecemos su cantidad a su acumulación máxima para que no pase de ahí.
                            itemsInventario[indexes[i]].Cantidad = itemPorAñadir.AcumulacionMax;
                            //Volvemos a llamar al método ahora con la diferencia.
                            AñadirItem(itemPorAñadir, diferencia);
                       }
                        InventarioUI.Instance.DibujarItemEnInventario(itemPorAñadir,
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

        if (cantidad > itemPorAñadir.AcumulacionMax)
        {
            AñadirItemEnSlotDisponible(itemPorAñadir, itemPorAñadir.AcumulacionMax);
            //Actualizamos la cantidad que falta al actualizar en slot vacío.
            cantidad -= itemPorAñadir.AcumulacionMax;
            //Se añade la cantidad restante.
            AñadirItem(itemPorAñadir, cantidad);
        }
        else
        {
            //Cuando la cantidad que se quiere añadir es superior a la cantidad máxima del item, se añade la cantidad restante en slot vacío disponible. 
            AñadirItemEnSlotDisponible(itemPorAñadir, cantidad);
        }
    }
    //Método para obtener la referencia de los items. El index del item es el mismo que el número de slot en el que se encuentra.
    private List<int> VerificarExistencias(string itemID) 
    {
        List<int> indexesDelItem = new List<int>();
        //Poder recorrer inventario y encontrar item con específico ID.
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

    //Método para añadir un item en un slot vacío.
    private void AñadirItemEnSlotDisponible(InventarioItem item, int cantidad)
    {
        //Recorremos todo el inventario
        for (int i = 0;i < itemsInventario.Length; i++)
        {
            if (itemsInventario[i] == null) 
            {
                //Creará nueva instancia del item en lugar de solo poner la referencia.
                itemsInventario[i] = item.CopiarItem();
                itemsInventario[i].Cantidad = cantidad;
                InventarioUI.Instance.DibujarItemEnInventario(item, cantidad, i);
                //Una vez que añadimos un item salimos del bucle para no recorrerlo.
                return;
            }
        }
    }

    
    private void EliminarItem(int index)
    {
        //Reducimos la cantidad del item en 1.
        ItemsInventario[index].Cantidad--;

        //Verificación si la cantidad está por debajo de 0.
        if (itemsInventario[index].Cantidad <= 0)
        { 
            itemsInventario[index].Cantidad = 0;
            //Hacemos que el item no exista al no tener cantidades.
            itemsInventario[index] = null;
            //Actualizar el slot donde se indica que ya no tenemos el item si ha llegado a 0 o menos.
            InventarioUI.Instance.DibujarItemEnInventario(null, 0, index);
        }
        //Si la cantidad del item que estamos eliminando no es menor o igual que 0, actualizamos su información dentro del inventario.
        else
        {
            InventarioUI.Instance.DibujarItemEnInventario(itemsInventario[index],
                itemsInventario[index].Cantidad, index);
        }
    }

    public void MoverItem(int indexInicial, int indexFinal)
    {
        //Verificamos si en el slot inicial de donde moveremos el item no es nulo. También verificaremos que el
        //Slot a donde se moverá no está ocupado.
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

    #region Eventos

    //Respuesta de cada botón habilitado en el inventario.
    private void SlotInteraccionRespuesta(TipoDeInteraccion tipo, int index)
    {
        switch (tipo)
        {
            case TipoDeInteraccion.Usar:
                UsarItem(index);
                break;
            case TipoDeInteraccion.Equipar:
                break;
            case TipoDeInteraccion.Remover:
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
