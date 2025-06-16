using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PersonajeAnimaciones : MonoBehaviour
{
    [SerializeField] private string layerIdle;
    [SerializeField] private string layerCaminar;

    private Animator _animator;
    private PersonajeMovimiento _personajeMovimiento;

    private readonly int direccionX = Animator.StringToHash("X");
    private readonly int direccionY = Animator.StringToHash("Y");
    private readonly int derrotado = Animator.StringToHash("Derrotado");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _personajeMovimiento = GetComponent<PersonajeMovimiento>();
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        ActualizarLayers();

        if (_personajeMovimiento.EnMovimiento == false) 
        {
            return;
        }
        _animator.SetFloat(direccionX, _personajeMovimiento.DireccionMovimiento.x);
        _animator.SetFloat(direccionY, _personajeMovimiento.DireccionMovimiento.y);
    }

    private void ActivarLayer(string nombreLayer)
    {
        for (int i = 0; i < _animator.layerCount; i++)
        {
            //Desactivaci�n de todos los Layers.
            _animator.SetLayerWeight(i, 0);
        }

        _animator.SetLayerWeight(_animator.GetLayerIndex(nombreLayer), 1);

    }

    private void ActualizarLayers()
    {
        if (_personajeMovimiento.EnMovimiento)
        {
            ActivarLayer(layerCaminar);
        }
        else
        {
            ActivarLayer(layerIdle);
        }
    }
    public void RevivirPersonaje()
    {
        ActivarLayer(layerIdle);
        _animator.SetBool(derrotado, false);
    }
    private void PersonajeDerrotadoRespuesta() 
    { 
        if (_animator.GetLayerWeight(_animator.GetLayerIndex(layerIdle)) == 1) 
        {
            _animator.SetBool(derrotado, true);   

            // Espera un momento y luego carga la escena Game Over
            StartCoroutine(CargarEscenaGameOverConRetraso());
        }
    }

    private IEnumerator CargarEscenaGameOverConRetraso()
    {
        yield return new WaitForSeconds(2f); // espera 2 segundos para que la animación se vea
        SceneManager.LoadScene("GameOver"); // usa el nombre exacto de la escena
    }

    //Llamado cuando la clase est� activada.
    private void OnEnable()
    {
        PersonajeVida.EventoPersonajeDerrotado += PersonajeDerrotadoRespuesta; 
    }

    //Llamado cuando la clase se desactiva.
    private void OnDisable()
    {
        //L�gica para sobreescribir la barra de vida del personaje.
        PersonajeVida.EventoPersonajeDerrotado -= PersonajeDerrotadoRespuesta;
    }

}
