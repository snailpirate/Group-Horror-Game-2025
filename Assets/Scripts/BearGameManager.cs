using UnityEngine;
using TMPro;
using System.Collections; // Required for Coroutines

public class BearGameManager : MonoBehaviour
{
    public static BearGameManager instance;

    [Header("UI References")]
    public TextMeshProUGUI counterText;
    public GameObject hugPrompt;

    [Header("Cinematic Settings")]
    public GameObject hugCinematicObject; // Drag your Arms/Bear model here
    public float animationDuration = 2.0f; // How long is your animation in seconds?

    private int bearsCaught = 0;
    private int totalBears = 6;
    private bool isHugging = false; // Prevents spamming E while hugging

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        UpdateCounterUI();
        hugPrompt.SetActive(false);

        // Ensure the cinematic is hidden at start
        if (hugCinematicObject != null)
            hugCinematicObject.SetActive(false);
    }

    // Call this from the Bear script
    public void TriggerHugSequence()
    {
        if (isHugging) return; // Don't allow double hugs

        // 1. Update logic
        bearsCaught++;
        UpdateCounterUI();
        ToggleHugPrompt(false);

        // 2. Start the cinematic routine
        StartCoroutine(PlayHugCinematic());
    }

    IEnumerator PlayHugCinematic()
    {
        isHugging = true;

        // Turn on the arms/bear model
        hugCinematicObject.SetActive(true);

        // Wait for the animation to finish
        yield return new WaitForSeconds(animationDuration);

        // Turn off the arms/bear model
        hugCinematicObject.SetActive(false);

        isHugging = false;
    }

    void UpdateCounterUI()
    {
        counterText.text = bearsCaught + "/" + totalBears + " Teddy Bears";
    }

    public void ToggleHugPrompt(bool show)
    {
        // Don't show the prompt if we are currently hugging
        if (isHugging)
        {
            hugPrompt.SetActive(false);
            return;
        }
        hugPrompt.SetActive(show);
    }
}