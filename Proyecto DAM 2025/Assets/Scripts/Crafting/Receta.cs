using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Receta 
{
    // Materiales que se necesitan para crear la receta.
    public string Nombre;
    [Header("1er Material")]
    public InventarioItem Item1;
    public int Item1CantidadRequerida;
    [Header("2do Material")]
    public InventarioItem Item2;
    public int Item2CantidadRequerida;

    // Items que se crean como resultado.
    [Header("Resultado")]
    public InventarioItem ItemResultado;
    public int ItemResultadoCantidad;
}
