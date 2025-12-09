using UnityEngine;

public class looking : MonoBehaviour
{
    // Drag your Player/Camera object to this field in the Inspector
    public Transform target; 

    // This value controls the eye's rotation speed (higher value means faster rotation)
    public float rotationSpeed = 5f; 

    void Update()
    {
        // 1. Ensure the target is not null
        if (target != null)
        {
            // --- Implement "Follow Rotation" ---
            
            // Calculate the rotation needed for the eye to look at the target
            Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);

            // Use Quaternion.Slerp to smoothly transition to the target rotation
            // Slerp (Spherical Linear Interpolation) makes the rotation look more natural and smooth
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // --- Implement "Follow Movement" (Optional) ---
            // Add this section if you need the eye to move and maintain a fixed distance from the player.
            // 
            // Example: Keep the eye's Y-axis height aligned with the player's Y-axis height, but keep X/Z fixed.
            // transform.position = new Vector3(transform.position.x, target.position.y, transform.position.z);
            
            // Example: Make the eye stay at a fixed offset position relative to the player (e.g., above the player's head).
            // transform.position = target.position + new Vector3(0, 2, 0); 

            // If you only want the eye to remain at a fixed point in the world (like on a wall or floor)
            // and only rotate to "look" at the player, you do not need to add any movement code.
        }
    }
}