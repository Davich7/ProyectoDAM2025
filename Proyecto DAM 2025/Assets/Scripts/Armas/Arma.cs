using UnityEngine;

//Definimos el tipo de arma.
public enum TipoArma
{
    Magia,
    Melee
}

//Creamos asset en carpetas
[CreateAssetMenu(menuName = "Personaje/Arma")]
public class Arma : ScriptableObject
{
    [Header("Config")]
    public Sprite ArmaIcono;
    public Sprite IconoSkill;
    public TipoArma Tipo;
    public float Daño;

    [Header("Arma Magica")]
    //Referencia de prefab.
    public Proyectil ProyectiPrefab;
    public float ManaRequerida;

    [Header("Stats")]
    public float ChanceCritico;
    public float ChanceBloqueo;




}
