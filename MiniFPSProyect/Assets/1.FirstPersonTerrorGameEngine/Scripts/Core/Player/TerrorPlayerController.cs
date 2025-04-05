using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrorPlayerController : MonoBehaviour
{
    // Movement variables
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    // Look variables
    public float mouseSensitivity = 2f;
    public float clampAngle = 80f;

    // Private variables
    private CharacterController _controller;
    private Vector3 _moveDirection;
    private Vector3 _velocity;
    private float _verticalRotation;
    private float _horizontalRotation;
    private bool _isGrounded;

    [SerializeField]
    private Transform direction_Transform;

    public float interactionDistance = 3f; // Distancia de interacción
    public Color gizmoColor = Color.yellow; // Color del Gizmo
 
    [SerializeField]
    private Transform CameraTransform;

    private Camera mainCamera;
    private Transform playerParent;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _verticalRotation = transform.localEulerAngles.y;
        mainCamera = Camera.main;
        playerParent = transform.parent;
    }

    private void Update()
    {
        // Verificar si está en el suelo
        _isGrounded = _controller.isGrounded;
        
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f; // Pequeña fuerza hacia abajo para mantener al jugador pegado al suelo
        }

        float xHorizontal = Input.GetAxis("Horizontal");
        float zVertical = Input.GetAxis("Vertical");

        // Calcular la dirección del movimiento basada en la rotación del padre
        _moveDirection = playerParent.forward * zVertical;
        _moveDirection += playerParent.right * xHorizontal;
        _moveDirection = Vector3.ClampMagnitude(_moveDirection, 1f); // Normalizar el movimiento

        if (CGameManager.Inst.GetPuzzleMode() == false)
        {
            // Aplicar movimiento horizontal
            _velocity.x = _moveDirection.x * moveSpeed;
            _velocity.z = _moveDirection.z * moveSpeed;
            
            // Manejar el salto
            if (_isGrounded && Input.GetButtonDown("Jump"))
            {
                _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * -1;

            _verticalRotation += mouseX;
            _horizontalRotation -= mouseY;
            _horizontalRotation = Mathf.Clamp(_horizontalRotation, -clampAngle, clampAngle);

            // Rotar el objeto padre en Y
            playerParent.localEulerAngles = new Vector3(0f, _verticalRotation, 0f);
        }
        else 
        {
            _velocity.x = 0f;
            _velocity.z = 0f;
        }

        // Aplicar gravedad
        if (!_isGrounded)
        {
            _velocity.y += gravity * Time.deltaTime;
        }

        // Mover el controlador
        _controller.Move(_velocity * Time.deltaTime);

        // Actualizar la rotación de la cámara
        CameraTransform.localEulerAngles = new Vector3(_horizontalRotation, 0f, 0f);

        // Interacción
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, interactionDistance))
            {
                Iinteract interactable = hit.collider.GetComponent<Iinteract>();
                if (interactable != null)
                {
                    interactable.Oninteract();
                }
            }
        }
    }

    public Transform getDirectionTransform()
    {
        return direction_Transform;
    }
}
