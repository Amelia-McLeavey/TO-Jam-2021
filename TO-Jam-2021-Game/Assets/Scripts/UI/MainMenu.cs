using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Access to all canvases
    [SerializeField] GameObject mainMenuCanvas;
    [SerializeField] GameObject controlsCanvas;
    [SerializeField] GameObject settingsCanvas;
    [SerializeField] GameObject creditsCanvas;

    // Var to save the current loaded canvas
    private static GameObject currentCanvas;

    // Var to save the last loaded canvas
    private static GameObject previousCanvas;

    private void Start()
    {
        // Main menu is the prevoius canvas by default
        previousCanvas = mainMenuCanvas;
    }

    public void PlayButton()
    {
        // When play is clicked, load game
        //SceneManager.LoadScene("Zoe");
        print("Where game scene will be loaded");
    }

    public void ControlsButton()
    {
        // Turn on controls canvas
        controlsCanvas.SetActive(true);

        // Save as current canvas
        currentCanvas = controlsCanvas;

        // Turn off previously loaded canvas
        previousCanvas.SetActive(false);
    }

    public void SettingsButton()
    {
        // Turn on settings canvas
        settingsCanvas.SetActive(true);

        // Save as current canvas
        currentCanvas = settingsCanvas;

        // Turn off previously loaded canvas
        previousCanvas.SetActive(false);
    }
    
    public void CreditsButton()
    {
        // Turn on credits canvas
        creditsCanvas.SetActive(true);

        // Save as current canvas
        currentCanvas = creditsCanvas;

        // Turn off previously loaded canvas
        previousCanvas.SetActive(false);
    }

    public void BackButtom()
    {
        // Turn on last loaded canvas
        previousCanvas.SetActive(true);

        // Turn off currently loaded canvas
        currentCanvas.SetActive(false);
    }

    public void ExitButton()
    {
        // Close the game
        Application.Quit();
    }
}
