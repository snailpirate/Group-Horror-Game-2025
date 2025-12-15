using UnityEngine;
using UnityEngine.SceneManagement; // Required to change scenes

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject menuPanel;
    public GameObject creditsPanel;
    public GameObject howToPlayPanel;

    [Header("Settings")]
    public string firstLevelName = "Hallway"; // Type the EXACT name of your game scene here

    void Start()
    {
        // Reset the cursor just in case it was hidden in the game
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        // Ensure we start at the main menu
        ShowMenu();
    }

    public void PlayGame()
    {
        // Loads the game scene
        SceneManager.LoadScene(firstLevelName);
    }

    public void ShowCredits()
    {
        menuPanel.SetActive(false);
        creditsPanel.SetActive(true);
        howToPlayPanel.SetActive(false);
    }

    public void ShowHowToPlay()
    {
        menuPanel.SetActive(false);
        creditsPanel.SetActive(false);
        howToPlayPanel.SetActive(true);
    }

    public void ShowMenu()
    {
        menuPanel.SetActive(true);
        creditsPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting...");
        Application.Quit();
    }
}