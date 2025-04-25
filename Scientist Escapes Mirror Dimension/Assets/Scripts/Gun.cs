using UnityEngine;

// Make sure the gun GameObject has a Collider component (set to IsTrigger=false if you want physics, or true if you just want detection)
[RequireComponent(typeof(Collider))]
public class Gun : MonoBehaviour, IInteractable // Assuming you might want an IInteractable interface later
{
    [Header("Gun Settings")]
    public string gunName = "Pistol"; // Example name
    // Add other gun-specific properties like ammo, damage, etc. here if needed

    [Header("Interaction")]
    [SerializeField] private string interactionPrompt = "Press F to pick up ";

    // This method will be called by the player's interaction script
    public void Interact()
    {
        Debug.Log($"Player interacted with {gunName}");

        // Add the gun to the inventory
        if (InventoryManager.instance != null)
        {
            InventoryManager.instance.AddItem(gameObject); // Pass this specific gun instance
            // The InventoryManager will handle disabling/equipping it
        }
        else
        {
            Debug.LogError("InventoryManager instance not found!");
        }

        // Optional: Add feedback like playing a pickup sound
        // AudioSource.PlayClipAtPoint(pickupSound, transform.position);
    }

    // This is part of the IInteractable pattern, allowing the player interaction script to get the prompt
    public string GetInteractionPrompt()
    {
        return interactionPrompt + gunName;
    }
}

// Optional: Define an interface for all interactable objects
public interface IInteractable
{
    void Interact();
    string GetInteractionPrompt();
} 