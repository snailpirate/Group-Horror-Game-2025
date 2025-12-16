using UnityEngine;
using UnityEngine.InputSystem;

public class FinalSkull : MonoBehaviour
{
    [Header("Settings")]
    [TextArea] public string promptText = "Press E to Give Yourself One Final Hug..."; // Type your custom text here in the Inspector!

    private bool isPlayerClose = false;
    private bool isEnding = false;

    void Start()
    {
        // HIDE MYSELF if the player hasn't collected 20 bears yet
        if (BearGameManager.instance != null)
        {
            if (!BearGameManager.instance.isFinalSkullUnlocked)
            {
                gameObject.SetActive(false); // Disappear immediately
            }
        }
    }

    void Update()
    {
        if (isPlayerClose && !isEnding)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                StartCoroutine(AscendSequence());
            }
        }
    }

    System.Collections.IEnumerator AscendSequence()
    {
        isEnding = true;
        BearGameManager.instance.ToggleHugPrompt(false);

        // 1. Find Player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        // 2. Disable Player Controls (So they can't walk away)
        if (player.GetComponent<CharacterController>())
            player.GetComponent<CharacterController>().enabled = false;
        
        // 3. Start the White Fade
        BearGameManager.instance.StartWhiteFadeSequence();

        // 4. Float Upwards loop
        float timer = 0;
        while (timer < 5.0f) // Float/Wait for 5 seconds total
        {
            // Move player up smoothly
            player.transform.position += Vector3.up * 2.0f * Time.deltaTime; 
            timer += Time.deltaTime;
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerClose = true;
            // Send our custom text to the prompt
            BearGameManager.instance.ToggleHugPrompt(true, promptText); 
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