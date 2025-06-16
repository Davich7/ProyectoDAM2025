using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteraccionExtraNPC
{
    Quests,
    Tienda,
    Crafting
}

//Para crear el NPCDialogo en carpetas hay que añadir el atributo CreateAssetMenu
[CreateAssetMenu]

//Todo lo relacionado al contenido en el diálogo del NPC.
public class NPCDialogo : ScriptableObject
{
    [Header("Info")]
    public string Nombre;
    public Sprite Icono;
    public bool ContieneInteraccionExtra;
    public InteraccionExtraNPC InteraccionExtra;

    [Header("Saludo")]
    [TextArea] public string Saludo;

    [Header("Chat")]
    public DialogoTexto[] Conversacion;

    [Header("Despedida")]
    [TextArea] public string Despedida;
}
//Para ver esta clase en el inspector, añadimos Serializable
[Serializable]
//Dialogo de oraciones cada una con su text area.
public class DialogoTexto
{
    [TextArea] public string Oracion;
}