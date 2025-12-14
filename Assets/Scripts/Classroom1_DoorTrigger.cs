using UnityEngine;
using UnityEngine.SceneManagement; // Required to change scenes

public class DoorTrigger : MonoBehaviour
{
    [Header("Settings")]
    public string sceneToLoad = "Classroom_1"; // Type the EXACT scene name here

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object hitting the door is the Player
        if (other.CompareTag("Player"))
        {
            // Optional: Save game state here if needed
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}