using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSlidingController : MonoBehaviour
{
   [Header("Sliding Parameters")]
    public float slideSpeed = 10f; // Velocidad inicial del deslizamiento
    public float slideFriction = 5f; // Fricción del deslizamiento

    private CharacterController _controller;
    private Vector3 _moveDirection;
    private bool _isSliding = false;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && _controller.isGrounded)
        {
            StartSliding();
        }

        if (_isSliding)
        {
            ApplySlide();
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) || !_controller.isGrounded)
        {
            StopSliding();
        }
    }

    void StartSliding()
    {
        _isSliding = true;
        _moveDirection = transform.forward * slideSpeed; // Iniciar deslizamiento en la dirección actual
    }

    void ApplySlide()
    {
        // Aplicar fricción al deslizamiento
        _moveDirection = Vector3.Lerp(_moveDirection, Vector3.zero, slideFriction * Time.deltaTime);

        // Mover el personaje
        _controller.Move(_moveDirection * Time.deltaTime);
    }

    void StopSliding()
    {
        _isSliding = false;
    }
}
