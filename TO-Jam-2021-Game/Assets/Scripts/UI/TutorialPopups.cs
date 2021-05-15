using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPopups : MonoBehaviour
{
    //[SerializeField]
    //private GameObject player;

    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private string keyCodeToDismiss;

    //private SpriteRenderer inputPrompt;

    //private bool triggered = false;

    private int[] values;
    private bool[] keys;

    public enum GameKeyCode
    {
        SPACE,W,A,D,J,K,L
    }

    private void Start()
    {
        //inputPrompt = GetComponentInChildren<SpriteRenderer>();
        //inputPrompt.enabled = false;

        values = (int[])System.Enum.GetValues(typeof(GameKeyCode));
        keys = new bool[values.Length];
        // DUBUG LINE //foreach (int v in values) {Debug.Log($"enum value: {values.GetValue(v)}, enum index integer: {v}");}
    }

    //private void Update()
    //{
    //    if (triggered)
    //    {
    //        for(int i = 0; i < values.Length; i++)
    //        {
    //            keys[i] = Input.GetKey((KeyCode)values[i]);
    //            if (keys[i])
    //            {

    //            }
    //        }

    //        // Disable the trigger zone that holds this script
    //        gameObject.SetActive(false);
    //    }

    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        triggered = true;
    //        inputPrompt.enabled = true;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        inputPrompt.enabled = false;
    //    }
    //}
}
