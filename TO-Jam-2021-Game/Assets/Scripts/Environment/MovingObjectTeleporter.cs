using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// teleports the object to a teleport location
public class MovingObjectTeleporter : MonoBehaviour
{
    [Header("References")]
    // store the two portals
    public GameObject TeleportLocation;

    // determines whether or not this teleporter will cut the momentum of objects it teleports
    public bool maintainMomentum;

    // check that the collision is a dynamic platform
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // store the rigidbody on the collision as we do not want to teleport static objects with moving properties
        Rigidbody2D body = collision.gameObject.GetComponent<Rigidbody2D>();

        // only teleport moving objects
        if (collision.tag == ("Moving") && body != null)
        {
            // kill momentum if maintain is false
            if(!maintainMomentum)
            {
                body.velocity = new Vector3(0, 0, 0);
            }

            // teleport the object
            collision.gameObject.transform.position = TeleportLocation.transform.position;
        }
    }
}
