using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterIntro : MonoBehaviour
{
    public static bool hasPlayed = false; 

    [Header("Settings")]
    public float typeSpeed = 0.05f; 
    public float timeToRead = 3.0f;
    public float fadeSpeed = 2.0f; 

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip typeSound;
    [Range(0f, 1f)] public float soundVolume = 0.5f;

    private TextMeshProUGUI myText;

    void Awake()
    {
        myText = GetComponent<TextMeshProUGUI>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        // Only run the "Intro" logic if it hasn't played yet
        if (!hasPlayed)
        {
            hasPlayed = true;
            PlayMessage(myText.text); // Play whatever is typed in the box
        }
        else
        {
            // Don't disable object! Just hide text so we can find it later.
            myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, 0f);
        }
    }

    // Call this function from other scripts to type new text
    public void PlayMessage(string newMessage)
    {
        // Ensure we are visible again
        gameObject.SetActive(true);
        myText.text = newMessage;
        
        // Reset Alpha to full
        Color c = myText.color;
        c.a = 1f;
        myText.color = c;

        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        myText.ForceMeshUpdate();
        int totalChars = myText.textInfo.characterCount; 
        myText.maxVisibleCharacters = 0; 

        for (int i = 0; i <= totalChars; i++)
        {
            myText.maxVisibleCharacters = i;
            if (audioSource != null && typeSound != null)
            {
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.PlayOneShot(typeSound, soundVolume);
            }
            yield return new WaitForSeconds(typeSpeed);
        }

        yield return new WaitForSeconds(timeToRead);

        // FADE OUT
        float currentAlpha = 1.0f;
        while (currentAlpha > 0.0f)
        {
            currentAlpha -= Time.deltaTime * fadeSpeed;
            myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, currentAlpha);
            yield return null; 
        }

        // IMPORTANT FIX:
        // We do NOT disable the object anymore. We just leave it invisible.
        // This allows the Manager to find it later for the 20th bear.
        myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, 0f);
    }
}