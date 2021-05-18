using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalloonPop : MonoBehaviour
{
    // Reference to a back button
    [SerializeField] GameObject backButton;

    void Start()
    {
        // Set colour to the same as the back button
        Color backColour = backButton.GetComponent<Image>().color;
        gameObject.GetComponent<Image>().color = backColour;


        // Set the balloon to be popped's posion to where the back button is located
        gameObject.transform.position = backButton.transform.position;

        // Anim should play automatically
    }

    public void Deactivate()
    {
        // Turn off gameobject when anim has finished playing
        gameObject.SetActive(false);
    }
}
