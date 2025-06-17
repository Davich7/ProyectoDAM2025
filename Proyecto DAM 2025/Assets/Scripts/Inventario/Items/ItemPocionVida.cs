using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Pocion Vida")]
public class ItemPocionVida : InventarioItem
{
    //Variable que indica la cantidad que podemos restaurar.
    [Header("Pocion info")]
    public float HPRestauracion;

    //Sobreescribimos el método de UsarItem
    public override bool UsarItem()
    {
        //Si el personaje puede ser curado le restauramos salud.
        if (Inventario.Instance.Personaje.PersonajeVida.PuedeSerCurado)
        {
            Inventario.Instance.Personaje.PersonajeVida.RestaurarSalud(HPRestauracion);
            return true;
        }
        return false;
    }

    public override string DescripcionItemCrafting()
    {
        // Cuando crafteamos una poción se mostrará.
        string descripcion = $"Restaura {HPRestauracion} de Salud";
        return descripcion;
    }
}
