using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReleaseBalloons : MonoBehaviour
{
    public float delay=2f;
    public Slider scoreSlider;
    public GameObject finalText;
    private Text finalTextText;
    private Animator textAnim;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            PlayerController pc = collision.GetComponent<PlayerController>();
            // pc.inputActive = false;
            StartCoroutine(release());
            
        }
    }

    private IEnumerator release()
    {
        yield return new WaitForSeconds(delay);

        GameObject[] Balloons = GameObject.FindGameObjectsWithTag("Balloon");

        foreach (GameObject balloon in Balloons)
        {
            balloon.GetComponent<BallonMovement>().floatingAway = true;
        }

        yield return new WaitForSeconds(0.5f);

        finalTextText = finalText.GetComponent<Text>();

        if (scoreSlider.value > 0.55f)
        {
            finalTextText.text = "Looking into the future, you see that the best is yet to come.";
        }
        else if (scoreSlider.value < 0.45f)
        {
            finalTextText.text = "Looking into the past, you decide that it would best if you moved on.";
        }
        else
        {
            finalTextText.text = "You are finally at peace.";           
        }

        textAnim = finalText.GetComponent<Animator>();
        textAnim.SetTrigger("Play");

    }
}
