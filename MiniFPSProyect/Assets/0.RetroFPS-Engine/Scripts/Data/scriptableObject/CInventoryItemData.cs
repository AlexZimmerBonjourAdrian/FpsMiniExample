using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Clase base para objetos de inventario como Scriptable Objects
[CreateAssetMenu(fileName = "New Inventory Item", menuName = "PointToClickEngine/Inventory/Item")]
public class CInventoryItemData : ScriptableObject, IInventoryItem
{
    public string Name => itemName;
    public Sprite Icon => icon;

    [SerializeField] private string itemName;
    [SerializeField] private Sprite icon;

    public virtual void Use()
    {
        Debug.Log($"Usando {itemName}");
        // Implementa la lógica de uso del objeto aquí
    }
}
