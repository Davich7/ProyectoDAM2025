using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Para poder llamar la clase en otras lo hacemos Singleton.
public class DialogoManager : Singleton<DialogoManager>
{
    //Obtener referencia del panel dialogo y del icono nombre para actualizarlo
    //Con TMP indicamos que son textos.
    [SerializeField] private GameObject panelDialogo;
    [SerializeField] private Image npcIcono;
    [SerializeField] private TextMeshProUGUI npcNombreTMP;
    [SerializeField] private TextMeshProUGUI npcConversacionTMP;

    //Propiedad que guarde informaci�n de qu� NPC queremos mostrar su di�logo.

    public NPCInteraccion NPCDisponible { get; set; }

    //Almacenamiento de dialogos en secuencia.
    private Queue<string> dialogosSecuencia;
    private bool dialogoAnimado;
    private bool despedidaMostrada;

    private void Start()
    {
        //Inicializaremos el Queue creado.
        dialogosSecuencia = new Queue<string>();
    }

    private void Update()
    {
        //Verificamos si tenemos un NPC disponible que cargar informaci�n.
        if (NPCDisponible == null)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ConfigurarPanel(NPCDisponible.Dialogo);
        }

        //Antes de continuar el di�logo hay que verificar si ya se mostr� la despedida.
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (despedidaMostrada)
            {
                AbrirCerrarPanelDialogo(false);
                despedidaMostrada = false;
                return;
            }

            //Verificamos si el NPC tiene alg�n tipo de interacci�n extra

            if (NPCDisponible.Dialogo.ContieneInteraccionExtra)
            {
                //Abriremos el panel correspondiente a la interacci�n que tiene el NPC.
                UIManager.Instance.AbrirPanelInteraccion(NPCDisponible.Dialogo.InteraccionExtra);
                //Cerramos el panel di�logo correspondiente.
                AbrirCerrarPanelDialogo(false);
                return;
            }

            if (dialogoAnimado)
            {
                ContinuarDialogo();
            }
        }
    }

    //L�gica de mostrar el panel y actualizar el icono y nombre del NPC.

    //M�todo que permite abrir y cerrar el panel de di�logo.
    public void AbrirCerrarPanelDialogo(bool estado)
    {
        //En caso de que sea verdadero se activa y si es falso se desactiva.
        panelDialogo.SetActive(estado);
    }

    //Para configurar el panel pasaremos como par�metro el NPC Dialogo y as� podremos acceder al nombre e icono del NPC.
    private void ConfigurarPanel(NPCDialogo nPCDialogo)
    {
        AbrirCerrarPanelDialogo(true);
        CargarDialogosSecuencia(nPCDialogo);

        npcIcono.sprite = nPCDialogo.Icono;
        npcNombreTMP.text = $"{nPCDialogo.Nombre}:";
        MostrarTextoConAnimacion(nPCDialogo.Saludo);
    }

    private void CargarDialogosSecuencia(NPCDialogo nPCDialogo)
    {
        //Verificamos que tenemos conversaci�n que cargar
        if (nPCDialogo.Conversacion == null || nPCDialogo.Conversacion.Length <= 0)
        {
            return;
        }
        //Recorremos toda la conversacion

        for (int i = 0; i < nPCDialogo.Conversacion.Length; i++) 
        {
            //Llamamos an Enqueue para poder a�adir un nuevo valor.
            dialogosSecuencia.Enqueue(nPCDialogo.Conversacion[i].Oracion);
        }
    }

    private void ContinuarDialogo()
    {
        //Para poder continuar el di�logo hay que verificar si tenemos el NPC disponible. 
        //CHECK DE SEGURIDAD, verificamos si tenemos NPC disponible.
        if (NPCDisponible == null)
        {
            return;
        }

        //Si la despedida fue mostrada, volvemos.
        if (despedidaMostrada)
        {
            return;
        }

        //Si no hay m�s oraciones que mostrar.
        if (dialogosSecuencia.Count == 0)
        {
            string despedida = NPCDisponible.Dialogo.Despedida;
            MostrarTextoConAnimacion(despedida);
            despedidaMostrada = true;
            return;
        }

        //Logica para mostrar la despedida

        string siguienteDialogo = dialogosSecuencia.Dequeue();
        MostrarTextoConAnimacion(siguienteDialogo);

    }
    private IEnumerator AnimarTexto(string oracion)
    {
        dialogoAnimado = false;
        npcConversacionTMP.text = "";
        //Ponemos todos los caracteres en array.
        char[] letras = oracion.ToCharArray();
        for (int i = 0; i < letras.Length; i++)
        {
            npcConversacionTMP.text += letras[i];
            yield return new WaitForSeconds(0.03f);
        }


        dialogoAnimado = true;
    }

    private void MostrarTextoConAnimacion(string oracion)
    {
        StartCoroutine(AnimarTexto(oracion));
    }
}
