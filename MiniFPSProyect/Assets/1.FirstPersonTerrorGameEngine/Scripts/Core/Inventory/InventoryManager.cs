using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    [System.Serializable]
    public class InventoryItem
    {
        public string itemName;
        public string description;
        public Sprite icon;
        public GameObject itemPrefab;
        public bool isUsable;
        public bool isConsumable;
        public int maxStack = 1;
        public float useDuration = 1f;
        public AudioClip useSound;
    }
    
    [System.Serializable]
    public class InventorySlot
    {
        public InventoryItem item;
        public GameObject itemPrefab;
        public int quantity;
        public bool isEquipped;
    }
    
    [Header("Configuración")]
    [SerializeField] private int maxSlots = 10;
    [SerializeField] private float itemUseCooldown = 1f;
    
    [Header("Referencias")]
    [SerializeField] private Transform itemHolder;
    [SerializeField] private AudioSource audioSource;
    
    private List<InventorySlot> inventory = new List<InventorySlot>();
    private Dictionary<string, float> lastUseTimes = new Dictionary<string, float>();
    private GameObject currentEquippedItem;
    
    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Inicializar inventario vacío
        for (int i = 0; i < maxSlots; i++)
        {
            inventory.Add(new InventorySlot());
        }
    }
    
    public bool AddItem(InventoryItem item, int quantity = 1)
    {
        // Buscar slot existente con el mismo item
        if (item.maxStack > 1)
        {
            InventorySlot existingSlot = inventory.Find(slot => 
                slot.item != null && 
                slot.item.itemName == item.itemName && 
                slot.quantity < item.maxStack
            );
            
            if (existingSlot != null)
            {
                int spaceInStack = item.maxStack - existingSlot.quantity;
                int amountToAdd = Mathf.Min(quantity, spaceInStack);
                
                existingSlot.quantity += amountToAdd;
                quantity -= amountToAdd;
                
                if (quantity <= 0)
                {
                    return true;
                }
            }
        }
        
        // Buscar slot vacío
        InventorySlot emptySlot = inventory.Find(slot => slot.item == null);
        if (emptySlot != null)
        {
            emptySlot.item = item;
            emptySlot.itemPrefab = item.itemPrefab;
            emptySlot.quantity = quantity;
            return true;
        }
        
        // No hay espacio en el inventario
        Debug.LogWarning("Inventario lleno");
        return false;
    }
    
    public bool RemoveItem(string itemName, int quantity = 1)
    {
        InventorySlot slot = inventory.Find(s => 
            s.item != null && s.item.itemName == itemName
        );
        
        if (slot != null)
        {
            if (slot.quantity <= quantity)
            {
                if (slot.isEquipped)
                {
                    UnequipItem(slot);
                }
                
                slot.item = null;
                slot.itemPrefab = null;
                slot.quantity = 0;
                slot.isEquipped = false;
                return true;
            }
            else
            {
                slot.quantity -= quantity;
                return true;
            }
        }
        
        return false;
    }
    
    public bool UseItem(string itemName)
    {
        InventorySlot slot = inventory.Find(s => 
            s.item != null && s.item.itemName == itemName
        );
        
        if (slot == null || !slot.item.isUsable)
        {
            return false;
        }
        
        // Verificar cooldown
        if (lastUseTimes.ContainsKey(itemName))
        {
            if (Time.time - lastUseTimes[itemName] < itemUseCooldown)
            {
                return false;
            }
        }
        
        // Reproducir sonido si existe
        if (slot.item.useSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(slot.item.useSound);
        }
        
        // Actualizar cooldown
        lastUseTimes[itemName] = Time.time;
        
        // Si es consumible, reducir cantidad
        if (slot.item.isConsumable)
        {
            slot.quantity--;
            if (slot.quantity <= 0)
            {
                slot.item = null;
                slot.itemPrefab = null;
                slot.quantity = 0;
                slot.isEquipped = false;
            }
        }
        
        return true;
    }
    
    public bool EquipItem(string itemName)
    {
        InventorySlot slot = inventory.Find(s => 
            s.item != null && s.item.itemName == itemName
        );
        
        if (slot == null || slot.item.itemPrefab == null)
        {
            return false;
        }
        
        // Desequipar item actual si existe
        if (currentEquippedItem != null)
        {
            UnequipItem(slot);
        }
        
        // Equipar nuevo item
        currentEquippedItem = Instantiate(slot.item.itemPrefab, itemHolder);
        slot.isEquipped = true;
        return true;
    }
    
    private void UnequipItem(InventorySlot slot)
    {
        if (currentEquippedItem != null)
        {
            Destroy(currentEquippedItem);
            currentEquippedItem = null;
        }
        
        if (slot != null)
        {
            slot.isEquipped = false;
        }
    }
    
    public bool HasItem(string itemName)
    {
        return inventory.Exists(slot => 
            slot.item != null && slot.item.itemName == itemName
        );
    }
    
    public int GetItemQuantity(string itemName)
    {
        InventorySlot slot = inventory.Find(s => 
            s.item != null && s.item.itemName == itemName
        );
        
        return slot != null ? slot.quantity : 0;
    }
    
    public List<InventorySlot> GetInventory()
    {
        return inventory;
    }
    
    public GameObject GetEquippedItem()
    {
        return currentEquippedItem;
    }
    
    public bool IsItemEquipped(string itemName)
    {
        InventorySlot slot = inventory.Find(s => 
            s.item != null && s.item.itemName == itemName
        );
        
        return slot != null && slot.isEquipped;
    }
} 
