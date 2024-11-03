using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWeaponController : MonoBehaviour
{   
    public float range = 100f; // Rango del raycast
    public Color laserColor = Color.red; // Color del Gizmo

    void Update()
    {
        if (Input.GetButtonDown("Fire")) // Cambia "Fire" por el botón que desees
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            Debug.Log("Golpeando: " + hit.collider.gameObject.name);
            hit.collider.GetComponent<IDamage>().OnDamage();

            // Aquí puedes agregar lógica para manejar el impacto, 
            // como infligir daño a un enemigo.
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = laserColor;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * range);
    }
}


