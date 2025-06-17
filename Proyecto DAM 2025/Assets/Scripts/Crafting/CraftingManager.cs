
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : Singleton<CraftingManager>   
{
    [Header("Config")]
    [SerializeField] private RecetaTarjeta recetaTarjetaPrefab;
    [SerializeField] private Transform recetaContenedor;

    [Header("Receta Info")]
    [SerializeField] private Image primerMaterialIcono;
    [SerializeField] private Image segundoMaterialIcono;
    [SerializeField] private TextMeshProUGUI primerMaterialNombre;
    [SerializeField] private TextMeshProUGUI segundoMaterialNombre;
    [SerializeField] private TextMeshProUGUI primerMaterialCantidad;
    [SerializeField] private TextMeshProUGUI segundoMaterialCantidad;
    [SerializeField] private TextMeshProUGUI recetaMensaje;
    [SerializeField] private Button buttonCraftear;

    [Header("Item Resultado")]
    //Referencias.
    [SerializeField] private Image itemResultadoIcono;
    [SerializeField] private TextMeshProUGUI itemResultadoNombre;
    [SerializeField] private TextMeshProUGUI itemResultadoDescripcion;


    [Header("Recetas")]
    [SerializeField] private RecetaLista recetas;

    // Guardamos información de la receta seleccionada.
    public Receta RecetaSeleccionada { get; set; }

    private void Start()
    {
        CargarRecetas();
    }
    private void CargarRecetas()
    {
        // Recorremos lista de recetas.
        for (int i = 0; i < recetas.Recetas.Length; i++)
        {
            RecetaTarjeta receta = Instantiate(recetaTarjetaPrefab, recetaContenedor);
            receta.ConfigurarRecetaTarjeta(recetas.Recetas[i]); 
        }
    }

    public void MostrarReceta(Receta receta)
    {
        RecetaSeleccionada = receta;
        // Actualizamos información de la receta.
        primerMaterialIcono.sprite = receta.Item1.Icono;
        segundoMaterialIcono.sprite = receta.Item2.Icono;
        primerMaterialNombre.text = receta.Item1.Nombre;
        segundoMaterialNombre.text = receta.Item2.Nombre;
        primerMaterialCantidad.text = 
            $"{Inventario.Instance.ObtenerCantidadDeItems(receta.Item1.ID)}/{receta.Item1CantidadRequerida}";
        segundoMaterialCantidad.text =
            $"{Inventario.Instance.ObtenerCantidadDeItems(receta.Item2.ID)}/{receta.Item2CantidadRequerida}";

        if (SePuedeCraftear(receta))
        {
            recetaMensaje.text = "Creacion Disponible";
            buttonCraftear.interactable = true;
        }
        else
        {
            recetaMensaje.text = "Se necesitan mas materiales";
            buttonCraftear.interactable = false;
        }
        // Actualizamos información del objeto que se creará.
        itemResultadoIcono.sprite = receta.ItemResultado.Icono;
        itemResultadoNombre.text = receta.ItemResultado.Nombre;
        itemResultadoDescripcion.text = receta.ItemResultado.DescripcionItemCrafting();
    }



    public bool SePuedeCraftear(Receta receta)
    {
        // Devuelve la cantidad de items con ID indicado del inventario.
        if (Inventario.Instance.ObtenerCantidadDeItems(receta.Item1.ID) >= receta.Item1CantidadRequerida 
        && Inventario.Instance.ObtenerCantidadDeItems(receta.Item2.ID) >= receta.Item2CantidadRequerida)
        {
            // Si tenemos materiales suficientes devolvemos true ya que se puede craftear.
            return true;
        }
        return false;
    }

    public void Craftear()
    {
        for (int i = 0; i < RecetaSeleccionada.Item1CantidadRequerida; i++)
        {
            Inventario.Instance.ConsumirItem(RecetaSeleccionada.Item1.ID);
        }
        for (int i = 0; i < RecetaSeleccionada.Item2CantidadRequerida; i++)
        {
            Inventario.Instance.ConsumirItem(RecetaSeleccionada.Item2.ID);
        }

        Inventario.Instance.AñadirItem(RecetaSeleccionada.ItemResultado, RecetaSeleccionada.ItemResultadoCantidad);
        MostrarReceta(RecetaSeleccionada);
    }
}

