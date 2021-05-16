using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float lerpSpeed;
    private Vector3 cameraOffset;
    public float maxHeightBeforeMoving;
    public bool cameraMove = true;

    public Transform secondaryTarget;
    public bool constrainX=false;
    public bool constrainY=false;

    private float prevY;
    private Vector3 targetPos;

    void Start()
    {
        prevY = player.position.y;
        cameraOffset = this.transform.position - player.position; //get offset
    }

    void FixedUpdate()
    {
        if (cameraMove)
        {
            calculateMovement();
        }
    }

    private void calculateMovement()
    {               
        if(constrainX)
        {
            targetPos = new Vector3(secondaryTarget.position.x, 0f, 0f);
        }
        else
        {
            targetPos = new Vector3(player.position.x, 0f, 0f);
        }

        if (constrainY)
        {
            targetPos = new Vector3(targetPos.x, secondaryTarget.position.y, 0f);
        }
        else
        {
            if (player.position.y + cameraOffset.y > transform.position.y + maxHeightBeforeMoving || player.position.y + cameraOffset.y < transform.position.y)
            {
                targetPos = new Vector3(targetPos.x, player.position.y, 0f);
            }
            else
            {
                targetPos = new Vector3(targetPos.x, transform.position.y-cameraOffset.y, 0f);
            }
        }

        this.transform.position = Vector3.Lerp(transform.position, targetPos + cameraOffset, lerpSpeed);
        prevY = player.position.y;
    }

    public void cameraDeathReset()
    {
        secondaryTarget = null;
        constrainX = false;
        constrainY = false;
        this.transform.position = player.position + cameraOffset;
    }
}
