using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TiposdeItem
{
    Armas,
    Pociones,
    Pergaminos,
    Ingredientes,
    Tesoros
}
public class InventarioItem : ScriptableObject
{
    [Header("Parametros")]
    public string ID;
    public string Nombre;
    public Sprite Icono;
    //Atributo TextArea permite tener un área para poder añadir más texto.
    [TextArea] public string Descripcion;

    [Header("Informacion")]
    public TiposdeItem Tipo;
    //Los items consumibles podrán utilizar el botón "Usar" del inventario.
    public bool EsConsumible;
    //Indica si el item puede ser stackeable o se puedo acumular en el personaje.
    public bool EsAcumulable;
    //Unidades que podemos acumular en un slot.
    public int AcumulacionMax;

    //Atributo oculto en el inspector. Es la variable que controla la cantidad que le queda.
    [HideInInspector] public int Cantidad;

    //Método que nos devuelva una nueva instancia del item.
    public InventarioItem CopiarItem() 
    {
        InventarioItem nuevaInstancia = Instantiate(this);
        return nuevaInstancia;
    } 
    public virtual bool UsarItem()
    {
        return true;
    }

    public virtual bool EquiparItem()
    {
        return true;
    }
    
    public virtual bool RemoverItem()
    {
        return true;
    }

    public virtual string DescripcionItemCrafting()
    {
        return "";
    }
}
