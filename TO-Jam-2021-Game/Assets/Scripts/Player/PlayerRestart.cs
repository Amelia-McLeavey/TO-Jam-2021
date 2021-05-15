using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRestart : MonoBehaviour
{
    // Player spawn (wherever the player has been placed in the world)
    private Vector3 playerSpawn;

    // Player rigidbody
    private Rigidbody rb;

    // Bool to flip when player needs to resart
    public bool restart = false;

    // How much to move the player vertaically
    private float liftHeight = 7f;

    // Movement speed
    private float liftSpeed = 5f;

    // Bools for restart transition states
    private bool lifting = false;
    private bool moving = false;


    void Start()
    {
        // Save the players intitial position
        playerSpawn = gameObject.transform.position;

        // Get player's rigidbody
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Restart player, if need be
        Restart();
    }

    private void Restart()
    {
        if (restart == true)
        {
            // Player is first lifted into the air
            if (lifting == true)
            {
                // Pick up player (flys into the air)
                gameObject.transform.Translate(0, liftSpeed, 0);

                // If the player has reached the lift height,
                if (gameObject.transform.position.y == liftHeight)
                {
                    // Player goes into the moving state
                    lifting = false;
                    moving = true;
                }
            }
            // Player then moves above their spawn point
            else if (moving == true)
            {
                // Move towards spawn point
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector2(playerSpawn.x, playerSpawn.y + liftHeight), 0f);

                // If above spawn point,
                if (gameObject.transform.position == new Vector3(playerSpawn.x, playerSpawn.y + liftHeight))
                {
                    // Player falls onto spawn
                    moving = false;

                    // Gravity turns back on
                    rb.useGravity = true;

                    // Unlock player movement
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // When the player collides with the end point (where the balloons are)
        if (collision.gameObject.tag == "endPoint")
        {
            // Tell player and other objects that they need to be restarted
            restart = true;

            // Lock player movement

            // Turn off gravity
            rb.useGravity = false;

            // Player is in the lifting state
            lifting = true;
        }
    }

    // When player reaches an end point (balloons)
    // Player goes back to beginning of the level
    // Any moved blocks/platforms go back to where they originated
}
