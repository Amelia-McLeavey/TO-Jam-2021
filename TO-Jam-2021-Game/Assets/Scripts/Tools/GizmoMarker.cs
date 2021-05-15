using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoMarker : MonoBehaviour
{
    public Color gizmoColor;
    public Vector3 gizmoSize = new Vector3(1, 1, 1);
    public bool scalingGizmo = false;

    // duration the gizmo flashes for
    public float currentFlashTime = 0.0f;
    public float flashDuration = 1.0f;
    public bool isFlashing = false;

    private void OnDrawGizmos()
    {
        if (scalingGizmo)
        {
            scaleGizmo();
        }

        // check flash duration
        if (currentFlashTime > flashDuration)
        {
            isFlashing = false;
            currentFlashTime = 0.0f;
        }

        // change color depending on whether or not we are flashing
        if (isFlashing)
        {
            currentFlashTime = currentFlashTime + Time.deltaTime;
            Gizmos.color = Color.green;
        } else
        {
            Gizmos.color = gizmoColor;
        }

        Gizmos.DrawCube(transform.position, gizmoSize);


    }

    // auto scales the gizmo to the object size
    public void scaleGizmo()
    {
        gizmoSize = transform.localScale;
    }

    // sets this gizmo to flash for a set duration
    public void flash()
    {
        isFlashing = true;
    }
}
