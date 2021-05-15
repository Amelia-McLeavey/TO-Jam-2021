using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TutorialPopups : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private GameKeyCode keyCodeToDismiss;

    private SpriteRenderer inputPrompt;

    private bool triggered = false;

    private int[] values;
    private bool[] keys;

    private enum GameKeyCode { Space, W, A, D, J, K, L } //Warning: do not rename these

    private void Start()
    {
        inputPrompt = GetComponentInChildren<SpriteRenderer>();
        inputPrompt.enabled = false;
        
        // Initialize array of custom enums, cast to int
        values = (int[])System.Enum.GetValues(typeof(GameKeyCode));
        // Initialize array with bools that will hold the state of the key inputs
        keys = new bool[values.Length];
        // DUBUG LINE //foreach (int v in values) {Debug.Log($"enum value: {values.GetValue(v)}, enum integral value: {v}");}
    }

    private void Update()
    {
        CheckForCorrectInputIfTriggered();
    }

    private void CheckForCorrectInputIfTriggered()
    {
        if (triggered)
        {
            for (int i = 0; i < values.Length; i++)
            {
                // Set the state of the key in this iteration to the state of the corresponding key on the keyboard
                keys[i] = Input.GetKey((KeyCode)values[i]);
                // Check if the state of the key is true. If the key has been pressed, the state is true.
                if (keys[i])
                {
                    // Check if the integral value of the enum in this iteration matches the integral value of the set keycode to dismiss
                    if (values[i] == (int)keyCodeToDismiss)
                    {
                        // Disable the trigger zone that holds this script, graphic will disappear
                        gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            triggered = true;
            inputPrompt.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inputPrompt.enabled = false;
        }
    }
}
