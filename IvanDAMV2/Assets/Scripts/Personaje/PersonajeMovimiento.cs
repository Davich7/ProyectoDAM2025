using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PersonajeMovimiento : MonoBehaviour
{
    public Joystick joystick;
    //Usamos este atributo para poder modificar el valor haciendo que sea semejante a público.
    [SerializeField] private float velocidad;

    public bool EnMovimiento => DireccionMovimiento.magnitude > 0f;
    public Vector2 DireccionMovimiento => _direccionMovimiento;
    private PersonajeVida personajeVida;
    private Rigidbody2D _rigidbody2D;
    private Vector2 _direccionMovimiento;
    private Vector2 _input;
    

    private void Awake()
    {
        personajeVida = GetComponent<PersonajeVida>();
           _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        Debug.Log($"Joystick input: {_input}");
        if (personajeVida.Derrotado)
        {
            _direccionMovimiento = Vector2.zero;
            return;
        }

#if UNITY_EDITOR
        _input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
#else
    _input = new Vector2(joystick.Horizontal, joystick.Vertical);
#endif
        
        _direccionMovimiento = _input.normalized;
        //_direccionMovimiento = Vector2.right;
    }

    private void FixedUpdate()
    { 
        _rigidbody2D.MovePosition(_rigidbody2D.position + _direccionMovimiento * velocidad * Time.fixedDeltaTime);
    }

}
