using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordDisplayHandler : MonoBehaviour
{
    public GameObject anger, sadness, joy;


    public void playAnger()
    {
        StartCoroutine(angerTime());
    }

    public void playSad()
    {
        StartCoroutine(sadTime());
    }

    public void playJoy()
    {
        StartCoroutine(joyTime());
    }


    private IEnumerator angerTime()
    {
        anger.SetActive(true);
        yield return new WaitForSeconds(2f);
        anger.SetActive(false);
    }

    private IEnumerator sadTime()
    {
        sadness.SetActive(true);
        yield return new WaitForSeconds(4f);
        sadness.SetActive(false);
    }

    private IEnumerator joyTime()
    {
        joy.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        joy.SetActive(false);
    }
}
