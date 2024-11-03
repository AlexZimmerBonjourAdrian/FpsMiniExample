using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CInteractMechanics : MonoBehaviour
{
    public float interactionDistance = 5f; // Distancia de interacción


    void Update()
    {
        // Solo revisa la interacción si se presiona la tecla "E"
       
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // Rayo desde el centro de la pantalla

            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                // Obtener el componente Iinteract del objeto impactado
                Iinteract interactable = hit.collider.GetComponent<Iinteract>();

                if (interactable != null)
                {
                    interactable.Oninteract(); // Ejecutar Oninteract()
                }
             }
        }
    }

    
}
