using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{

    public float totalWhite=0;
    public float totalBlack=0;

    public Text whiteText;
    public Text blackText;

    public Slider counterSlider;

    void Start()
    {
        
    }


    public void addCount(bool white)
    {
        if (white)
        {
            totalWhite++;
            whiteText.text = totalWhite.ToString();
        }
        else
        {
            totalBlack++;
            blackText.text = totalBlack.ToString();
        }

        counterSlider.value = totalWhite / (totalWhite + totalBlack);

    }
}
