using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    // Text to change volume %
    [SerializeField] Text VolText;

    // Volume % control
    public void Volume(float sliderValue)
    {
        // Convert slider value to appropriate output (from 0-2% to 0-100%)
        sliderValue *= 100 / 2;
        sliderValue = Mathf.Round(sliderValue);

        // Update text
        VolText.text = sliderValue.ToString() + "%";

    }
}
