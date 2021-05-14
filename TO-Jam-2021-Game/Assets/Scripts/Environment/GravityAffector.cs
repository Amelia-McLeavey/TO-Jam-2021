using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAffector : MonoBehaviour
{
    public float _slowMultiplier = 1.0f;

    // toggles whether or not this platform is affected by gravity
    public bool _affectedByGravity;

    public Rigidbody2D _rigidBody;
    // scales the action speed of everything done by this transformer
    public float _gravityBaseScaleValue = 1.0f;

    private void FixedUpdate()
    {
        _rigidBody.velocity = new Vector2();
    }

    private void Update()
    {
        // convert the bool to an int
        int abgInt = _affectedByGravity ? 1 : 0;

        // update this objects gravity scalar to be equal to its base multiplier times the slow multiplier
         _rigidBody.gravityScale = (_gravityBaseScaleValue * _slowMultiplier) * abgInt;
    }

    // updates the value of the slow multiplier
    public void UpdateSlowMulti(float multiplier)
    {
        _slowMultiplier = multiplier;
    }
}
