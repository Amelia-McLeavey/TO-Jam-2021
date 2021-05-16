using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordDisplayHandler : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(delayTime());
    }
    private IEnumerator delayTime()
    {
        yield return new WaitForSeconds(4f);
        this.gameObject.SetActive(false);
    }

}
