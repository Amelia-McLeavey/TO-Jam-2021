using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentRestart : MonoBehaviour
{
    // Player restart script reference
    [SerializeField] PlayerRestart playerRestart;

    // Var to store intial position of object
    private Vector2 initialPos;

    void Start()
    {
        // Save initial position of object
        initialPos = gameObject.transform.position;
    }

    void Update()
    {
        Restart();
    }

    private void Restart()
    {
        // If the player has reached the end point
        if (playerRestart.restart == true)
        {
            // Reset position back to the intial position
            gameObject.transform.position = initialPos;
        }
    }
}
