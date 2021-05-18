using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChange : MonoBehaviour
{
    // Start is called before the first frame updat

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene("Credits");
    }
}
