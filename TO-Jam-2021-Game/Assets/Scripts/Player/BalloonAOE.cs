    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonAOE : MonoBehaviour
{
    [HideInInspector]
    public int lastEmotionAsInt;

    [SerializeField]
    private GameObject JoyPopVFX;
    [SerializeField]
    private GameObject AngerPopVFX;
    [SerializeField]
    private GameObject SadPopVFX;

    [Range(0, 10)]
    [SerializeField]
    private float AOERadius = 2.0f;
    [Range(0, 10)]
    [SerializeField]
    private float timeToMaxRadius = 1.5f;
    [Range(0, 10)]
    [SerializeField]
    private float joyEffectDuration = 1.0f;
    [Range(0, 10)]
    [SerializeField]
    private float angerEffectDuration = 1.0f;
    [Range(0, 10)]
    [SerializeField]
    private float sadEffectDuration = 1.0f;
    [Range(0, 10)]
    [SerializeField]
    private float joyEffectCooldown = 1.0f;
    [Range(0, 10)]
    [SerializeField]
    private float angerEffectCooldown = 1.0f;
    [Range(0, 10)]
    [SerializeField]
    private float sadEffectCooldown = 1.0f;
    [Range(0, 10)]
    [SerializeField]
    private float slowMultiplier = 2.0f;

    private float effectDuration;
    private float effectCooldown;

    private bool inputIsCool = true;

    private enum LastEmotion { None, Anger, Sad, Joy }

    public void PopBalloon(string keyPressed)
    {
        if (inputIsCool)
        {
            inputIsCool = false;
            DebugDrawAOE(keyPressed);

            // Collect all the Balloons the player has aquired
            GameObject[] Balloons = GameObject.FindGameObjectsWithTag("Balloon");

            // Set variables for emotion-effect types, send command to pop balloon, instantiate corresponding VFX
            foreach (GameObject balloon in Balloons)
            {
                if (balloon.name == "Balloon 1" && keyPressed == "J")
                {
                    effectCooldown = joyEffectCooldown;
                    effectDuration = joyEffectDuration;
                    lastEmotionAsInt = (int)LastEmotion.Joy;
                    balloon.GetComponent<BallonMovement>().burstNow = true;
                    Instantiate(JoyPopVFX, transform.position, Quaternion.identity, this.gameObject.transform);
                }
                else if (balloon.name == "Balloon 2" && keyPressed == "L")
                {
                    effectCooldown = sadEffectCooldown;
                    effectDuration = sadEffectDuration;
                    lastEmotionAsInt = (int)LastEmotion.Sad;
                    balloon.GetComponent<BallonMovement>().burstNow = true;
                    Instantiate(SadPopVFX, transform.position, Quaternion.identity, this.gameObject.transform);
                }
                else if (balloon.name == "Balloon 3" && keyPressed == "K")
                {
                    effectCooldown = angerEffectCooldown;
                    effectDuration = angerEffectDuration;
                    lastEmotionAsInt = (int)LastEmotion.Anger;
                    balloon.GetComponent<BallonMovement>().burstNow = true;
                    Instantiate(AngerPopVFX, transform.position, Quaternion.identity, this.gameObject.transform);
                }
                else
                {
                    Debug.Log("NO MATCH BETWEEN BALLOONS HELD AND KEYPRESSED");
                }
            }

            StartCoroutine(EffectDelay(keyPressed));
        }
    }

    private IEnumerator EffectDelay(string keyPressed)
    {
        yield return new WaitForSeconds(timeToMaxRadius);
        EffectArea(keyPressed);
        StartCoroutine(EffectUsageCooldown());
    }

    public void EffectArea(string keyPressed)
    {
        // Get a reference to the platform layer mask
        LayerMask platformLayerMask = LayerMask.GetMask("Platform");

        // Determine targets that have been hit, store in an array
        Collider2D[] hitPlatforms = Physics2D.OverlapCircleAll(gameObject.transform.position, AOERadius, platformLayerMask);
        Debug.Log($"Number of hit platforms = {hitPlatforms.Length}");

        // Send info to the hit dynamic platforms
        foreach (Collider2D platform in hitPlatforms)
        {
            // Filter the hit platforms by attached script
            if (platform.GetComponent<DynamicPlatformController>())
            {
                Debug.Log($"Filtered Platform Name: {platform.name}");

                // Call method in platform's controller to set the passed variables depending on keyPressed
                switch (keyPressed)
                {
                    // JOY | FLOAT
                    case "J":
                        //Debug.Log("J Reached");
                        platform.GetComponent<DynamicPlatformController>().QueueMovement(gameObject.transform.position, AOERadius, joyEffectDuration, "Float");
                        break;
                    // ANGER | SHOVE
                    case "K":
                        //Debug.Log("K Reached");
                        platform.GetComponent<DynamicPlatformController>().QueueMovement(gameObject.transform.position, AOERadius, angerEffectDuration, "Shove");
                        break;
                    // SAD | SLOW
                    case "L":
                        //Debug.Log("L Reached");
                        platform.GetComponent<DynamicPlatformController>().SetSlowMultiplier(slowMultiplier, sadEffectDuration);
                        break;
                    default:
                        Debug.LogError("No match for keyPressed");
                        break;
                }
            }
        }
    }

    private IEnumerator EffectUsageCooldown()
    {
        yield return new WaitForSeconds(effectCooldown + effectDuration);
        inputIsCool = true;
    }

    // A temporary Debug Draw method to show us where the area of effect is until we have a proper VFX, only visible in Scene view.
    private void DebugDrawAOE(string keyPressed)
    {
        float duration = 10f;
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