using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Obtenemos referencia del Arma que vamos a equipar, para actualizar los
// valores cuando equipamos o eliminamos un Arma del panel.

public class ContenedorArma : Singleton<ContenedorArma>
{
    [SerializeField] private Image armaIcono;
    [SerializeField] private Image armaSkillIcono;

    //Propiedad que guarde referencia del arma equipada.
    public ItemArma ArmaEquipada { get; set; }
    public void EquiparArma(ItemArma itemArma)
    {
        //Referencia arma.
        ArmaEquipada = itemArma;
        //Actualizamos la imágenes del arma.
        armaIcono.sprite = itemArma.Arma.ArmaIcono;
        armaIcono.gameObject.SetActive(true);

        if (itemArma.Arma.Tipo == TipoArma.Magia)
        {
            //Para el arma de tipo rango activamos el icono de skill.
            armaSkillIcono.sprite = itemArma.Arma.IconoSkill;
            armaSkillIcono.gameObject.SetActive(true);
        }
        //Equipamos un arma a nuestro personaje.
        Inventario.Instance.Personaje.PersonajeAtaque.EquiparArma(itemArma);
    }

    public void RemoverArma()
    {
        armaIcono.gameObject.SetActive(false);
        armaSkillIcono.gameObject.SetActive(false);
        //Perdemos referencia del arma equipada al perderla.
        ArmaEquipada = null;
        Inventario.Instance.Personaje.PersonajeAtaque.RemoverArma();
    }
}
