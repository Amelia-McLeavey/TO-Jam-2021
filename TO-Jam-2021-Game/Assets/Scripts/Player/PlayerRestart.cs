using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRestart : MonoBehaviour
{
    // Player spawn (wherever the player has been placed in the world)
    private Vector3 playerSpawn;

    // Player's position when collided with end point
    private Vector3 playerEndPointPos;

    // Player rigidbody
    private Rigidbody2D rb;

    // Bool to flip when player needs to resart
    public bool restart = false;

    // How much to move the player vertaically
    private float liftHeight = 3f;

    // Movement speed
    private float liftSpeed = 0.5f;

    // Bools for restart transition states
    private bool lifting = false;
    private bool moving = false;


    void Start()
    {
        // Save the players intitial position
        playerSpawn = gameObject.transform.position;

        // Get player's rigidbody
        rb = gameObject.GetComponent<Rigidbody2D>();
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
                if (gameObject.transform.position.y >= playerEndPointPos.y + liftHeight)
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
                Vector2 moveToSpawn = Vector2.MoveTowards(gameObject.transform.position, new Vector2(playerSpawn.x, playerSpawn.y + liftHeight), liftSpeed);
                gameObject.transform.Translate(moveToSpawn, Space.World);

                // If above spawn point,
                if (gameObject.transform.position.x == playerSpawn.x)
                {
                    // Player falls onto spawn
                    moving = false;
                    restart = false;

                    // Gravity & simulated turns back on
                    rb.gravityScale = 1;
                    rb.simulated = true;

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

            // Turn off gravity & simulated to allow lift
            rb.gravityScale = 0;
            rb.simulated = false;

            // Save the player's position when they entered the end point collider
            playerEndPointPos = gameObject.transform.position;

            // Player is in the lifting state
            lifting = true;
        }
    }

    // When player reaches an end point (balloons)
    // Player goes back to beginning of the level
    // Any moved blocks/platforms go back to where they originated
}
