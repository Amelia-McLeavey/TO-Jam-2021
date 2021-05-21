using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBinding : MonoBehaviour
{
    // previous x position
    float pXPos;
    // direction that this platform is moving
    int direction;

    [Header("Bind Settings")]
    // distance from the edge when moving the platform will bind to a new platform
    public float edgeBindingColliderDistancePercentage = 0.1f;
    // objects that are bount have a multiplier added to their edge binding distance
    bool bound = false;
    public float edgeBindingBoundMultiplier = 3.0f;
    public float collisionThreshold = 0.05f;
    public float effectiveZero = 0.001f;

    [Header("Reference")]
    public DynamicPlatformController dynamicScript;


    private void Start()
    {
        pXPos = transform.position.x;

        dynamicScript = GetComponent<DynamicPlatformController>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckDirection();

        CheckForBinding();

        pXPos = transform.position.x;
    }

    // returns an integer representing the x transformation of the object from one frame to another
    int CheckDirection()
    {


        float tempValue = (transform.position.x - pXPos);

        if (tempValue != 0)
        {
            tempValue = tempValue / Mathf.Abs(tempValue);
            direction = Mathf.FloorToInt(tempValue);
        } else

        {
            direction = 0; 
        }


        return direction; 
    }

    void CheckForBinding()
    {
        float tempEdgeBindingBoundMultplier = (bound ? 1 : edgeBindingBoundMultiplier);

        float rayCastOffset = GetComponent<BoxCollider2D>().size.x;

        rayCastOffset = rayCastOffset / 2;

        rayCastOffset = (rayCastOffset - (rayCastOffset * (edgeBindingColliderDistancePercentage * tempEdgeBindingBoundMultplier))) * direction;

        Vector3 tempRayCastOrigin = transform.position;

        tempRayCastOrigin.x = tempRayCastOrigin.x + rayCastOffset;

        tempRayCastOrigin.y = tempRayCastOrigin.y - ((GetComponent<BoxCollider2D>().size.y / 2) + effectiveZero);

        RaycastHit2D hit = Physics2D.Raycast(tempRayCastOrigin, Vector2.down, collisionThreshold, LayerMask.GetMask("Platform"));

        //Debug.DrawRay(tempRayCastOrigin, Vector2.down, Color.white, 1);

        // if we hit something bind to it
        if (hit)
        {
            transform.SetParent(hit.collider.gameObject.transform, true);

            if(!dynamicScript._atRest)
            {
                dynamicScript.ToggleAtRest();
            }

        } else
        {
            if (dynamicScript._atRest)
            {
                dynamicScript.ToggleAtRest();
            }
        }
    }

}
