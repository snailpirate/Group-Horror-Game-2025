using UnityEngine;
using UnityEngine.InputSystem; // REQUIRED for the new system

public class TeddyBearInteract : MonoBehaviour
{
    private bool isPlayerClose = false;

    void Update()
    {
        if (isPlayerClose)
        {
            // NEW CODE: Checks the "E" key on the current keyboard
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                PerformHug();
            }
        }
    }

    void PerformHug()
    {
        BearGameManager.instance.AddScore();
        BearGameManager.instance.ToggleHugPrompt(false);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerClose = true;
            BearGameManager.instance.ToggleHugPrompt(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerClose = false;
            BearGameManager.instance.ToggleHugPrompt(false);
        }
    }
}