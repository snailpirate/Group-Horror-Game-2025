using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BearGameManager : MonoBehaviour
{
    public static BearGameManager instance;

    [Header("UI References")]
    public TextMeshProUGUI counterText;
    public GameObject hugPrompt;

    [Header("Cinematic Settings")]
    public GameObject hugCinematicObject;
    public float animationDuration = 2.0f;

    // DATA
    public List<string> caughtBearIDs = new List<string>();
    private int totalBears = 20;
    private bool isHugging = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
        // 1. Find UI (These are usually active, so FindTag works)
        GameObject textObj = GameObject.FindGameObjectWithTag("ScoreText");
        if (textObj != null) counterText = textObj.GetComponent<TextMeshProUGUI>();

        GameObject promptObj = GameObject.FindGameObjectWithTag("HugPrompt");
        if (promptObj != null)
        {
            hugPrompt = promptObj;
            hugPrompt.SetActive(false);
        }

        // 2. FIND THE HIDDEN CINEMATIC ARMS (The New Fix)
        // We cannot use GameObject.FindWithTag because the object is disabled.
        // Instead, we find the Player, then look through all their children (even hidden ones).
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Get EVERY child object attached to the player (true = include hidden)
            Transform[] allChildren = player.GetComponentsInChildren<Transform>(true);

            foreach (Transform child in allChildren)
            {
                if (child.CompareTag("CinematicArms"))
                {
                    hugCinematicObject = child.gameObject;
                    break; // Found it! Stop looking.
                }
            }
        }

        UpdateCounterUI();
    }

    public bool TriggerHugSequence(string bearID)
    {
        if (isHugging) return false;

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