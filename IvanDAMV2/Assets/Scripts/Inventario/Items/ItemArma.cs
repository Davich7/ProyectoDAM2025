using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Arma")]

//Para que un arma sea un item, hacemos que herede la clase de InventarioItem.
public class ItemArma : InventarioItem
{
    [Header("Arma")]
    public Arma Arma;

    //Sobreescribimos el método EquiparItem() dentro de ItemArma para dar la lógica de
    //como equiparemos un arma.
    public override bool EquiparItem()
    {
        //Solamente podemos equipar un arma si el contenedor no tiene ya un arma equipada.
        if (ContenedorArma.Instance.ArmaEquipada != null)
        {
            return false;
        }
        //Si no hay arma equipada, indicamos la referencia de este arma equipada.
        ContenedorArma.Instance.EquiparArma(this);
        return true;
    }

    //Sobreescribimos el método de removerItem.
    public override bool RemoverItem()
    {
        //No podemos remover un arma si no hay arma equipada
        if (ContenedorArma.Instance.ArmaEquipada == null)
        {
            return false;
        }
        //Si hay arma equipada, lo eliminamos y devolvemos verdadero ya que se eliminó el arma.
        ContenedorArma.Instance.RemoverArma();
        return true;
    }

    public override string DescripcionItemCrafting()
    {
        string descripcion = $"- Prob Critico: {Arma.ChanceCritico}%\n" +
                             $"- Prob Bloqueo: {Arma.ChanceBloqueo}%";
        return descripcion;
    }

}
