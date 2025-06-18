using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    // Creamos variable que indique la cantidad de instancias de un objeto que
    // queremos crear en el booler.
    [SerializeField] private int cantidadPorCrear;

    //Creamos lista donde guardamos todas las instancias que crearemos.

    private List<GameObject> lista;

    //Gameobject que crearemos en tiempo real en nuestra jerarquía para poder contener
    //todas las instancias que estamos creando.
    public GameObject ListaContenedor { get; private set; }

    public void CrearPooler(GameObject objetoPorCrear)
    {
        //Inicializamos lista.
        lista = new List<GameObject>();
        //Especificamos el nombre del GameObject, indicamos que es un Pool junto con el nombre del objeto que estamos creando.
        ListaContenedor = new GameObject($"Pool - {objetoPorCrear.name}");

        //Recorremos la cantidad de instancias que vamos a crear.
        for (int i = 0; i < cantidadPorCrear; i++)
        {
            lista.Add(AñadirInstancia(objetoPorCrear));
        }
    }
        
    private GameObject AñadirInstancia(GameObject objetoPorCrear)
    {
        // Instanciamos objetoPorCrear en el contenedor en nuestra jerarquía y lo guardamos dentro de nuevoObjeto.
        GameObject nuevoObjeto = Instantiate(objetoPorCrear, 
            ListaContenedor.transform);
        // Cada instancia dentro del pooler debe de estar desactivada. Solo se activa cuando se utiliza.
        nuevoObjeto.SetActive(false);
        return nuevoObjeto;
    }

    public GameObject ObtenerInstancia()
    {
        // Recorremos la lista.
        for (int i = 0; i < lista.Count; i++)
        {
            // Buscamos el primer objeto que encontremos que no esté siendo utilizado.
            if (lista[i].activeSelf == false)
            {
                return lista[i];
            }
        }
        return null;
    }

    // Método cuando el arma se desequipe se necesita destruir el pooler con los proyectiles del arma.
    public void DestruirPooler()
    {
        Destroy(ListaContenedor);
        lista.Clear();
    }
}
