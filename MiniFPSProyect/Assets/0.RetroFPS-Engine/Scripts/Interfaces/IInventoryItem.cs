using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interfaz para interacciones con objetos del inventario
public interface IInventoryItem
{
    string Name { get; }
    Sprite Icon { get; }
    void Use();
}
