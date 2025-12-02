using UnityEngine;
using UnityEngine.InputSystem;

public class TeddyBearInteract : MonoBehaviour
{
    private bool isPlayerClose = false;

    void Update()
    {
        if (isPlayerClose)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                PerformHug();
            }
        }
    }

    void PerformHug()
    {
        // 1. Trigger the cinematic and score in the Manager
        BearGameManager.instance.TriggerHugSequence();

        // 2. Destroy this specific world-bear immediately
        // (The cinematic bear will appear to "replace" it)
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