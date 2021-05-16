using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TutorialPopups : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameKeyCode keyCodeToDismiss;

    private bool triggered;

    private GameKeyCode[] codes;

    private SpriteRenderer inputGraphic;

    private enum GameKeyCode { w, a, d, j, k, l } //Warning: do not rename these

    private void Start()
    {
        inputGraphic = GetComponentInChildren<SpriteRenderer>();
        inputGraphic.enabled = false;

        // Initialize array of custom enums
        codes = (GameKeyCode[])System.Enum.GetValues(typeof(GameKeyCode));
    }

    private void Update()
    {
        // The BoxCollider2D for this system is located on the child "Trigger" of the main parent object "Teach Volume"
        triggered = GetComponentInChildren<TutorialTriggerCheck>().triggered;

        UpdateGraphicVisibility();
        CheckForCorrectInput();
    }

    private void UpdateGraphicVisibility()
    {
        // Flip the contextual input button prompts on or off depending on trigger
        if (triggered)
        { inputGraphic.enabled = true; } 
        else { inputGraphic.enabled = false; }
    }

    private void CheckForCorrectInput()
    {
        if (triggered)
        {
            // Iterate through enum array
            foreach (GameKeyCode code in codes)
            {
                // Convert enum to string
                string c = code.ToString();
                // If if Input matches the enum of the current iteration
                if (Input.GetKeyDown(c))
                {
                    // Check if key being pressed down matches the key that will dismiss the contextual input button prompts
                    if (c == keyCodeToDismiss.ToString())
                    {
                        // Disable the trigger zone that holds this script, graphic will disappear
                        gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
