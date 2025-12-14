using UnityEngine;
using UnityEngine.InputSystem;

public class TeddyBearInteract : MonoBehaviour
{
    [Header("Identity")]
    public string bearID; // NAME THIS UNIQUE FOR EVERY BEAR (e.g. Bear1, Bear2)

    private bool isPlayerClose = false;

    void Start()
    {
        // MEMORY CHECK:
        // When the scene loads, check if the Manager already has my ID in the list.
        if (BearGameManager.instance != null)
        {
            if (BearGameManager.instance.caughtBearIDs.Contains(bearID))
            {
                // I have already been caught! Destroy myself immediately.
                Destroy(gameObject);
            }
        }
    }

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
        // Pass my unique ID to the manager
        bool permissionGranted = BearGameManager.instance.TriggerHugSequence(bearID);

        if (permissionGranted)
        {
            Destroy(gameObject);
        }
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