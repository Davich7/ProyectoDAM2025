using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

// Se utiliza como clase que defina lo que tiene un item que es dropeado por el enemigo.
public class DropItem 
{
    [Header("Info")]
    public string Nombre;
    // Referencia al item añadido al inventario cuando recoges loot.
    public InventarioItem Item;
    // Cantidad del item que se va a recoger.
    public int Cantidad;

    [Header("Drop")]
    [Range(0, 100)]public float PorcentajeDrop;

    public bool ItemRecogido { get; set; }
}
