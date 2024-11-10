using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CClimbing : MonoBehaviour
{
  public float climbDistance = 2f; // Distancia máxima para detectar el borde
    public float climbAngle = 90f; // Ángulo máximo para considerar el borde escalable
    public float climbSpeed = 2f; // Velocidad de la animación de escalada

    private bool isClimbing = false;
    private Vector3 targetPosition;
    public Color gizmoColor = Color.yellow;
    public Transform ObjectPlayer;

    void Start()
    {
       // ObjectPlayer = transform.parent.Find("GameObject");
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.E)) // Cambia 'E' por la tecla que desees
         {   Debug.Log("Before" + isClimbing);
            CheckForClimbableEdge();
            Debug.Log("After"+ isClimbing);
       }

        if (isClimbing)
        {
            Climb();
        }
    }
    void CheckForClimbableEdge()
    {
        RaycastHit hit;
        if (Physics.Raycast(ObjectPlayer.position + ObjectPlayer.forward * 0.5f, ObjectPlayer.forward, out hit, climbDistance))
        {
            if (hit.collider.CompareTag("Escalable"))
            {   
                Debug.Log("Entra?");
                // Calcula el ángulo entre la normal del impacto y el vector hacia arriba
                float angle = Vector3.Angle(hit.normal, Vector3.up);

                if (angle <= climbAngle)
                {
                    // El personaje está cerca de un borde escalable
                    StartClimbing(hit);
                }
            }
        }
    }
    // Método para dibujar el Gizmo
     void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor; // Establece el color del Gizmo
        Gizmos.DrawLine(ObjectPlayer.position + ObjectPlayer.forward * 0.5f, ObjectPlayer.position + ObjectPlayer.forward * climbDistance); // Línea hacia adelante
        Gizmos.DrawSphere(ObjectPlayer.position + ObjectPlayer.forward * climbDistance, 0.2f); // Esfera al final de la línea
    }
    

    void StartClimbing(RaycastHit hit)
    {
        isClimbing = true;
        // Calcula la posición objetivo en la parte superior del borde
        Vector3 forwardDirection = ObjectPlayer.forward; 

        // Ajusta la altura y la distancia hacia adelante según sea necesario
        targetPosition = hit.point + Vector3.up * 1.9f + forwardDirection * 5f; 
    }

    void Climb()
    {
        // Mueve el personaje hacia la posición objetivo
        ObjectPlayer.position = Vector3.Lerp(ObjectPlayer.position, targetPosition, Time.deltaTime * climbSpeed);

        // Verifica si el personaje ha llegado a la posición objetivo
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isClimbing = false;
        }
    }
}
