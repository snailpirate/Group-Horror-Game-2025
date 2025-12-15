using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterIntro : MonoBehaviour
{
    // STATIC variable: This survives scene changes!
    // It starts false, becomes true after the first run, and stays true until you Quit.
    public static bool hasPlayed = false; 

    [Header("Timing Settings")]
    public float typeSpeed = 0.05f; 
    public float timeToRead = 2.0f;
    public float fadeSpeed = 2.0f; 

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip typeSound;
    [Range(0f, 1f)] public float soundVolume = 0.5f;

    private TextMeshProUGUI myText;

    void Start()
    {
        // 1. MEMORY CHECK
        // If we have already played this intro in this session, turn off immediately.
        if (hasPlayed == true)
        {
            gameObject.SetActive(false);
            return; // Stop reading the code here
        }

        // 2. MARK AS PLAYED
        // Set this to true so next time we enter the hallway, the check above runs.
        hasPlayed = true;

        // 3. NORMAL SETUP
        myText = GetComponent<TextMeshProUGUI>();
        
        Color c = myText.color;
        c.a = 1f;
        myText.color = c;

        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        int totalChars = myText.textInfo.characterCount; 
        
        if (totalChars == 0)
        {
            myText.ForceMeshUpdate();
            totalChars = myText.textInfo.characterCount;
        }

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

        float currentAlpha = 1.0f;
        while (currentAlpha > 0.0f)
        {
            currentAlpha -= Time.deltaTime * fadeSpeed;
            myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, currentAlpha);
            yield return null; 
        }

        gameObject.SetActive(false);
    }
}