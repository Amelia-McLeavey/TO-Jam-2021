using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // Var for pausing via button click
    private bool pauseClicked = false;

    // Bool for pausing / unpausing
    private bool gamePaused = false;

    private void Update()
    {
        // Check if the game needs to be paused
        PauseGame();
    }

    private void PauseGame()
    {
        // If esc is pressed or the pause button is clicked,
        if (Input.GetKeyDown(KeyCode.Escape) || pauseClicked == true)
        {
            // If game is not currently paused
            if (gamePaused == false)
            {
                // Pause the game
                Time.timeScale = 0;

                // Game becomes paused
                gamePaused = true;
            }
            // If game is currently paused
            else
            {
                // Unpause the game
                Time.timeScale = 1;

                // Game becomes unpaused
                gamePaused = false;
            }
        }
    }

    public void PauseButtonDown()
    {
        // When the pause button is clicked, the game is paused
        pauseClicked = true;
    }

    public void PauseButtonUp()
    {
        // Tells PauseGame() that the pause button is no longer being clicked
        pauseClicked = false;
    }
}
