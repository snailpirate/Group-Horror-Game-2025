using UnityEngine;
using TMPro; // We use TextMeshPro for better text, standard Unity Text works too
using UnityEngine.UI;

public class BearGameManager : MonoBehaviour
{
    public static BearGameManager instance; // Singleton for easy access

    [Header("UI References")]
    public TextMeshProUGUI counterText; // The "0/6 Teddy Bears" text
    public GameObject hugPrompt;        // The "Press E to Hug" text object

    private int bearsCaught = 0;
    private int totalBears = 6;

    void Awake()
    {
        // Simple singleton setup
        if (instance == null) instance = this;
    }

    void Start()
    {
        UpdateCounterUI();
        hugPrompt.SetActive(false); // Hide prompt at start
    }

    public void AddScore()
    {
        bearsCaught++;
        UpdateCounterUI();

        // Optional: Check for win condition
        if (bearsCaught >= totalBears)
        {
            Debug.Log("All bears caught!");
            // You could load a win screen here
        }
    }

    void UpdateCounterUI()
    {
        counterText.text = bearsCaught + "/" + totalBears + " Teddy Bears";
    }

    // Helper to turn the "Press E" prompt on/off
    public void ToggleHugPrompt(bool show)
    {
        hugPrompt.SetActive(show);
    }
}