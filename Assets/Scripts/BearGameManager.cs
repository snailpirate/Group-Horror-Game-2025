using UnityEngine;
using TMPro;
using UnityEngine.UI; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BearGameManager : MonoBehaviour
{
    public static BearGameManager instance;

    [Header("UI References")]
    public TextMeshProUGUI counterText;
    public GameObject hugPrompt;
    public Image whiteFadePanel; 
    public TypewriterIntro typewriter; 

    [Header("Cinematic Settings")]
    public GameObject hugCinematicObject;
    public float animationDuration = 2.0f;

    // DATA
    public List<string> caughtBearIDs = new List<string>();
    public int totalBears = 20; 
    private bool isHugging = false;
    
    public bool isFinalSkullUnlocked = false; 

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find Standard UI
        GameObject textObj = GameObject.FindGameObjectWithTag("ScoreText");
        if (textObj != null) counterText = textObj.GetComponent<TextMeshProUGUI>();

        GameObject promptObj = GameObject.FindGameObjectWithTag("HugPrompt");
        if (promptObj != null) { hugPrompt = promptObj; hugPrompt.SetActive(false); }

        // Find Typewriter
        GameObject typeObj = GameObject.FindGameObjectWithTag("IntroText");
        if (typeObj != null) typewriter = typeObj.GetComponent<TypewriterIntro>();

        // Find White Screen
        GameObject whiteObj = GameObject.FindGameObjectWithTag("WhiteScreen");
        if (whiteObj != null) whiteFadePanel = whiteObj.GetComponent<Image>();

        // Find Hidden Arms
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Transform[] allChildren = player.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in allChildren)
            {
                if (child.CompareTag("CinematicArms"))
                {
                    hugCinematicObject = child.gameObject;
                    break;
                }
            }
        }
        UpdateCounterUI();
    }

    public bool TriggerHugSequence(string bearID)
    {
        if (isHugging) return false;

        if (!caughtBearIDs.Contains(bearID)) caughtBearIDs.Add(bearID);

        UpdateCounterUI();
        ToggleHugPrompt(false);
        StartCoroutine(PlayHugCinematic());
        
        // CHECK FOR END GAME
        if (caughtBearIDs.Count >= totalBears && !isFinalSkullUnlocked)
        {
            isFinalSkullUnlocked = true;
            if (typewriter != null)
            {
                typewriter.PlayMessage("Now Find Your Final Friend...");
            }
            else
            {
                Debug.LogWarning("Manager could not find Typewriter script. Did you add IntroText to the HUD Prefab?");
            }
        }

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

    public void StartWhiteFadeSequence()
    {
        StartCoroutine(WhiteFadeRoutine());
    }

    IEnumerator WhiteFadeRoutine()
    {
        if (whiteFadePanel == null) yield break;

        // 1. Fade to White AND Fade Audio Out
        float duration = 3.0f;
        float timer = 0f;
        float startVolume = AudioListener.volume;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = timer / duration;
            
            // Fade Screen to White
            whiteFadePanel.color = new Color(1, 1, 1, progress);

            // Fade Audio to 0 (Linear fade)
            AudioListener.volume = Mathf.Lerp(startVolume, 0f, progress);

            yield return null;
        }
        
        // Ensure strictly White and Silent
        whiteFadePanel.color = Color.white;
        AudioListener.volume = 0f;

        // 2. Wait on white screen
        yield return new WaitForSeconds(2.0f);

        // 3. RESTORE AUDIO & LOAD MENU
        AudioListener.volume = 1f; // Reset volume so the menu isn't silent!
        SceneManager.LoadScene("MainMenu");
        
        Destroy(gameObject); 
    }

    void UpdateCounterUI()
    {
        if (counterText != null) counterText.text = caughtBearIDs.Count + "/" + totalBears + " Teddy Bears";
    }

    public void ToggleHugPrompt(bool show, string overrideText = "")
    {
        if (isHugging || hugPrompt == null) return;
        
        hugPrompt.SetActive(show);
        
        if (show && overrideText != "")
            hugPrompt.GetComponent<TextMeshProUGUI>().text = overrideText;
        else if (show)
            hugPrompt.GetComponent<TextMeshProUGUI>().text = "Press E to Hug";
    }
}