using UnityEngine;

public class ItemPorAgregar : MonoBehaviour
{
    //Para visualizar en inspector de Unity.
    [Header("Config")]
    [SerializeField] private InventarioItem inventarioItemReferencia;
    [SerializeField] private int cantidadPorAgregar;

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Si colisionamos con el jugador llamamos a Inventario y m�todo A�adirItem.
        if (other.CompareTag("Player")) 
        {
            Inventario.Instance.A�adirItem(inventarioItemReferencia, cantidadPorAgregar);
            //Una vez se recoge el item se destruye.
            Destroy(gameObject);
        }
    }
}
