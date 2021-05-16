using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsHandler : MonoBehaviour
{




    public string[] creditsToReadout;
    public int currentCredit;

    public Sprite[] imagesToShow;

    public GameObject creditsObject;
    GameObject tempCredit;

    public GameObject imageObject;
    GameObject tempImage;
    public int currentImage;

    private void Awake()
    {
        DeployCredits();

        InvokeRepeating("DeployCredits", 3f, 3.75f);
        InvokeRepeating("DeployImage", 3f, 3.75f);

        InvokeRepeating("DeployCredits", 3.5f, 3.75f);

        InvokeRepeating("DeployCredits", 4, 3.75f);
    }

    public void DeployCredits()
    {
        tempCredit = Instantiate(creditsObject, this.transform);

        tempCredit.GetComponent<Text>().text = (creditsToReadout[currentCredit]);
        currentCredit += 1;
    }

    public void DeployImage()
    {
        tempImage = Instantiate(imageObject, this.transform);

        tempImage.GetComponent<Image>().sprite = (imagesToShow[currentImage]);
        currentImage += 1;
    }


}
