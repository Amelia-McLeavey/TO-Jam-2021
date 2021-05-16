using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTriggerCheck : MonoBehaviour
{
    [HideInInspector]
    public bool triggered = false;

    private void Update()
    {
        if (triggered)
        { GetComponentInChildren<SpriteRenderer>().color = new Color(0f, 1f, 0f, 0.5f); }
        else
        { GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f); }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            triggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            triggered = false;
        }
    }
}
