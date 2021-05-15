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
        {
            GetComponentInChildren<SpriteRenderer>().color = Color.green;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            triggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            triggered = false;
        }
    }
}
