using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopCameraMoveZone : MonoBehaviour
{
    public bool constrainX=false;
    public bool constrainY=false;

    private CameraFollow cf;

    private void Start()
    {
        cf = Camera.main.GetComponentInParent<CameraFollow>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            cf.constrainX = constrainX;
            cf.constrainY = constrainY;
            cf.secondaryTarget = this.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            cf.constrainX = false;
            cf.constrainY = false;
            cf.secondaryTarget = null;
        }
    }
}
