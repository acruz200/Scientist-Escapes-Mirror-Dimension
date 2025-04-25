using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    [Header("Inventory")]
    [SerializeField] private List<GameObject> items = new List<GameObject>(); // Stores collected items
    [SerializeField] private GameObject equippedItem = null; // Currently equipped item
    [SerializeField] private Transform handTransform; // Where the equipped item will be held

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keep inventory across scenes
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Adds an item to the inventory and potentially equips it.
    /// </summary>
    /// <param name="itemPrefab">The prefab or object to add.</param>
    public void AddItem(GameObject itemPrefab)
    {
        if (itemPrefab == null) return;

        Debug.Log($"Adding {itemPrefab.name} to inventory.");
        items.Add(itemPrefab);

        // For simplicity, automatically equip the first gun picked up
        // You might want more complex logic here later (e.g., weapon slots)
        if (equippedItem == null && itemPrefab.CompareTag("Gun")) // Assuming the gun prefab has the tag "Gun"
        {
            EquipItem(itemPrefab);
        }
        else
        {
            // If not equipping, just keep track (or disable the added item if it was an instance)
             if(itemPrefab.scene.IsValid()) // Check if it's an instance in the scene
             {
                 itemPrefab.SetActive(false); // Hide the picked-up item
             }
        }
    }

    /// <summary>
    /// Equips a specific item from the inventory.
    /// </summary>
    /// <param name="itemPrefab">The prefab of the item to equip.</param>
    private void EquipItem(GameObject itemPrefab)
    {
         if (handTransform == null)
         {
             Debug.LogError("Hand Transform is not assigned in InventoryManager!");
             // Optionally, try to find the player's hand dynamically
             // handTransform = FindObjectOfType<PlayerController>()?.transform.Find("Hand"); // Example path
             // if (handTransform == null) return;
             return;
         }

        // If already holding something, unequip it first (or handle swapping)
        if (equippedItem != null)
        {
            Destroy(equippedItem); // Destroy the currently held instance
        }

        // Instantiate the item prefab as a child of the hand transform
        equippedItem = Instantiate(itemPrefab, handTransform.position, handTransform.rotation, handTransform);
        
        // Reset local position/rotation if needed, depending on the prefab setup
        equippedItem.transform.localPosition = Vector3.zero;
        equippedItem.transform.localRotation = Quaternion.identity;

        Debug.Log($"Equipped {equippedItem.name}.");
        
        // Ensure the original prefab/scene object used for pickup is disabled
        if(itemPrefab.scene.IsValid())
        {
            itemPrefab.SetActive(false);
        }
    }

    // --- Potential Future Methods ---
    // public void DropItem(GameObject item) { ... }
    // public void SwitchWeapon(int index) { ... }
    // public GameObject GetEquippedItem() { return equippedItem; }
    // public List<GameObject> GetItems() { return items; }
} 