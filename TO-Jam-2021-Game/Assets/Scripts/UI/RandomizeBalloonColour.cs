using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomizeBalloonColour : MonoBehaviour
{
    // Array of all the balloons in the scene
    [SerializeField] Image[] balloons;

    // Var to store colour
    Color colour;

    void Start()
    {
        // Loop through the array and assign each balloon a random colour
        for (int i = 0; i < balloons.Length; i++)
        {
            // Select random colour
            int colourNum = Random.Range(0, 3);

            if (colourNum == 0)
            {
                colour = Color.red;
            }
            else if (colourNum == 1)
            {
                colour = Color.blue;
            }
            else
            {
                colour = Color.yellow;
            }

            // Set colour to balloon
            balloons[i].color = colour;
        }
    }
}
