using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] float maxIntensity = 1;
    [Range(0,1)] public float intensityPercent = 0;
    
    void Update()
    {
        Vector2 ranPos = Random.insideUnitCircle * maxIntensity * intensityPercent;
        Camera.main.transform.localPosition = new Vector3(ranPos.x, ranPos.y, 0);
    }
}
