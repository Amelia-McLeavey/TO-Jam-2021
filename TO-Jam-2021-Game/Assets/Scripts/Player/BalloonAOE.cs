using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonAOE : MonoBehaviour
{
    [Range(0, 10)]
    [SerializeField]
    private float AOERadius = 1.0f;
    [Range(0, 10)]
    [SerializeField]
    private float effectDuration = 1.0f;
    [Range(0, 10)]
    [SerializeField]
    private float slowMultiplier = 2.0f;

    public void PopBalloon(string keyPressed)
    {
        DebugDrawAOE(keyPressed);

        // Get a reference to the dynamic platform layer mask
        LayerMask dynamicPlatformLayerMask = LayerMask.GetMask("Platform");

        // Determine targets that have been hit, store in an array
        Collider[] hitDynamicPlatforms = Physics.OverlapSphere(gameObject.transform.position, AOERadius, dynamicPlatformLayerMask);
        Debug.Log($"Number of hit dynamic platforms = {hitDynamicPlatforms.Length}");

        // Send info to the hit dynamic platforms
        foreach (Collider platform in hitDynamicPlatforms)
        {
            // Call method in platform's controller to set the passed variables depending on keyPressed
            switch(keyPressed)
            {
                // JOY | FLOAT
                case "J":
                    dynamicPlatform.gameObject.GetComponent<DynamicPlatformController>().QueueMovement(gameObject.transform.position, AOERadius, effectDuration, keyPressed);
                    break;
                // ANGER | SHOVE
                case "K":
                    dynamicPlatform.gameObject.GetComponent<DynamicPlatformController>().QueueMovement(gameObject.transform.position, AOERadius, effectDuration, keyPressed);
                    break;
                // SAD | SLOW
                case "L":
                    dynamicPlatform.gameObject.GetComponent<DynamicPlatformController>().SetSlowMultiplier(slowMultiplier, effectDuration);
                    break;
                default:
                    Debug.LogError("No match for keyPressed");
                    break;
            }
        }
    }

    // A temporary Debug Draw method to show us where the area of effect is until we have a proper VFX
    private void DebugDrawAOE(string keyPressed)
    {
        float duration = 2f;
        float quality = 32;
        float singleSegmentAngle = 2 * Mathf.PI / quality;

        // Set the color of the line for basic legibility
        Color color;
        if (keyPressed == "J")
        {
            color = Color.yellow;
        } else if (keyPressed == "K")
        {
            color = Color.red;
        } else if (keyPressed == "L")
        {
            color = Color.blue;
        } else
        {
            color = Color.white;
        }

        // Use drawlines to draw circle
        for (int segment = 0; segment < quality; ++segment)
        {
            float angleOne = segment * singleSegmentAngle;
            Vector3 startpoint = new Vector3(Mathf.Cos(angleOne), Mathf.Sin(angleOne), 0);

            startpoint *= AOERadius;

            float angleTwo = (segment + 1) * singleSegmentAngle;
            Vector3 endpoint = new Vector3(Mathf.Cos(angleTwo), Mathf.Sin(angleTwo), 0);

            endpoint *= AOERadius;

            Debug.DrawLine(this.gameObject.transform.position + startpoint, this.gameObject.transform.position + endpoint, color, duration);
        }
    }
}