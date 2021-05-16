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

    private FMOD.Studio.EventInstance collectable;

    [SerializeField]
    int comboCount;

    [SerializeField]
    float comboMaxTime;

    [SerializeField]
    float comboTimeRemaning;

    void Start()
    {

    }

    private void Update()
    {

        comboTimeRemaning += Time.deltaTime;

        if (comboTimeRemaning >= comboMaxTime)
        {
            comboCount = 0;
        }

    }


    void ResetComboTime()
    {
        comboTimeRemaning = 0;
    }

    public void addCount(bool white)
    {

        PlayCombo();
        comboCount += 1;
        ResetComboTime();

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

    public void PlayCombo()
    {
        collectable = FMODUnity.RuntimeManager.CreateInstance("event:/Collectable");
        collectable.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));

        collectable.setParameterByName("CollectCombo", comboCount);
        


        collectable.start();
        collectable.release();
    }
}
