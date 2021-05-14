using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderFollow : MonoBehaviour
{
    public GameObject attachedRigidbody;

    // Update is called once per frame
    void FixedUpdate()
    {
        // ensure that this collider is identical to the attached collider
        transform.position = attachedRigidbody.transform.position;
        transform.rotation = attachedRigidbody.transform.rotation;
        transform.localScale = attachedRigidbody.transform.localScale;
    }
}
