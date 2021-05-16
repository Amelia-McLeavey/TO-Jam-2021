using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    public GameObject lastActiveCheckpoint;

    // swaps all childrens active states
    void swapActive()
    {
        if(lastActiveCheckpoint.transform.childCount > 0)
        {
            foreach (GameObject child in transform)
            {
                child.SetActive(!child.activeSelf);
            }
        }
    }

    // on entering a checkpoint store the checkpoints location
    // on entering a death zone teleport to the stored checkpoint
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.tag)
        {
            case "KillZone":
                transform.position = lastActiveCheckpoint.transform.position;
                break;
            case "Checkpoint":
                // check if this is the first time we are runnning this code and last active checkpoint is not assigned yet
                if (lastActiveCheckpoint != null)
                {
                    swapActive();
                }
                lastActiveCheckpoint = collision.gameObject;
                swapActive();
                break;
        }
    }





}
