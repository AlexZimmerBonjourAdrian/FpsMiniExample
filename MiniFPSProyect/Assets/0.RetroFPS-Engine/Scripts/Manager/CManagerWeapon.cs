using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace Fps
{
    public class CManagerWeapon : MonoBehaviour //IDataPersistence
    {

          [SerializeField] private List<GameObject> weapons;
            private int currentWeaponIndex = 0;
        private void Start()
        {
            // Get all children GameObjects that have a Weapon component
            weapons = GetComponentsInChildren<CWeaponController>(true) // Include inactive children
                                  .Select(weaponComponent => weaponComponent.gameObject) // Select the GameObject
                                  .ToList();
         SwitchWeapon(0);
        }
    
        
    private void Update()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (scroll > 0f)
            {
                NextWeapon();
            }
            else if (scroll < 0f)
            {
                PreviousWeapon();
            }
        }

        private void NextWeapon()
        {
            currentWeaponIndex++;
            if (currentWeaponIndex >= weapons.Count)
            {
                currentWeaponIndex = 0; // Vuelta al inicio
            }

            SwitchWeapon(currentWeaponIndex);
        }

        private void PreviousWeapon()
        {
            currentWeaponIndex--;
            if (currentWeaponIndex < 0)
            {
                currentWeaponIndex = weapons.Count - 1; // Vuelta al final
            }

            SwitchWeapon(currentWeaponIndex);
        }

        private void SwitchWeapon(int newWeaponIndex)
        {
            // Desactivar todas las armas
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].SetActive(false);
            }

            // Activar la nueva arma seleccionada
            weapons[newWeaponIndex].SetActive(true);
        }
    }
      
}


