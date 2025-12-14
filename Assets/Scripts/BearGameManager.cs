using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic; // Needed for Lists
using UnityEngine.SceneManagement; // Needed to detect scene changes

public class BearGameManager : MonoBehaviour
{
    public static BearGameManager instance;

    [Header("UI References")]
    // We don't drag these in anymore. The script finds them automatically.
    public TextMeshProUGUI counterText;
    public GameObject hugPrompt;

    [Header("Cinematic Settings")]
    public GameObject hugCinematicObject;
    public float animationDuration = 2.0f;

    // DATA: Keep track of specific bears caught
    public List<string> caughtBearIDs = new List<string>();
    private int totalBears = 6;
    private bool isHugging = false;

    void Awake()
    {
        // SINGLETON PATTERN WITH PERSISTENCE
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Don't die when scene changes!
        }
        else
        {
            // If we go back to the hallway, a new Manager will try to start.
            // We destroy the NEW one so the OLD one (with the score) stays in charge.
            Destroy(gameObject);
        }
    }

    // Automatically find UI when a new scene loads
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 1. Find the Text by TAG (We will set this tag in the editor later)
        GameObject textObj = GameObject.FindGameObjectWithTag("ScoreText");
        if (textObj != null)
            counterText = textObj.GetComponent<TextMeshProUGUI>();

        // 2. Find the Prompt by TAG
        GameObject promptObj = GameObject.FindGameObjectWithTag("HugPrompt");
        if (promptObj != null)
        {
            hugPrompt = promptObj;
            hugPrompt.SetActive(false);
        }

        // 3. Find the Cinematic Object (Assuming it's attached to the camera in every scene)
        // If your cinematic object is only in the hallway, this might need adjustment, 
        // but finding it by Tag is safest.
        GameObject cineObj = GameObject.FindGameObjectWithTag("CinematicArms");
        if (cineObj != null)
        {
            hugCinematicObject = cineObj;
            hugCinematicObject.SetActive(false);
        }

        // 4. Update the visual score immediately
        UpdateCounterUI();
    }

    public bool TriggerHugSequence(string bearID)
    {
        if (isHugging) return false;

        // Add this specific bear's ID to our memory list
        if (!caughtBearIDs.Contains(bearID))
        {
            caughtBearIDs.Add(bearID);
        }

        UpdateCounterUI();
        ToggleHugPrompt(false);
        StartCoroutine(PlayHugCinematic());
        return true;
    }

    IEnumerator PlayHugCinematic()
    {
        isHugging = true;
        if (hugCinematicObject != null) hugCinematicObject.SetActive(true);

        yield return new WaitForSeconds(animationDuration);

        if (hugCinematicObject != null) hugCinematicObject.SetActive(false);
        isHugging = false;
    }

    void UpdateCounterUI()
    {
        // Only update text if the text object actually exists in this scene
        if (counterText != null)
        {
            counterText.text = caughtBearIDs.Count + "/" + totalBears + " Teddy Bears";
        }
    }

    public void ToggleHugPrompt(bool show)
    {
        if (isHugging || hugPrompt == null) return;
        hugPrompt.SetActive(show);
    }
}