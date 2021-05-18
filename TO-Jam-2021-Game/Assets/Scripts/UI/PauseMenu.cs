using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // Bool for pausing / unpausing
    private bool gamePaused = false;

    // Access to canvases
    [SerializeField] GameObject pauseCanvas;
    [SerializeField] GameObject controlsCanvas;
    [SerializeField] GameObject settingsCanvas;

    // Pause Background (Darken)
    [SerializeField] GameObject background;

    private void Update()
    {
        // Check if the game needs to be paused via esc key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        // If game is not currently paused
        if (gamePaused == false)
        {
            // Pause the game
            Time.timeScale = 0;

            // Game becomes paused
            gamePaused = true;

            // Enable / disable canvases
            pauseCanvas.SetActive(true);

            // Turn on background
            background.SetActive(true);
        }
        // If game is currently paused
        else
        {
            // Unpause the game
            Time.timeScale = 1;

            // Game becomes unpaused
            gamePaused = false;

            // Enable / disable canvases
            pauseCanvas.SetActive(false);
            controlsCanvas.SetActive(false);
            settingsCanvas.SetActive(false);

            // Turn off background
            background.SetActive(false);
        }
    }
}
