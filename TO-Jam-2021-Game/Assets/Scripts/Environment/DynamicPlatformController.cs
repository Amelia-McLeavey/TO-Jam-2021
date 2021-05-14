using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicPlatformController : MonoBehaviour
{
    [Header("States")]
    public bool _floating;

    [Header("References")]
    public Rigidbody2D _rigidbody;

    [Header("Move Variables")]
    public float _floatSpeed = 1.0f;
    public Vector2 _floatTarget = new Vector2();
    // position that the platform is currently moving towards
    public Vector2 _target;
    public float _slowMultiplier = 1.0f;
    // toggles whether or not this platform is affected by gravity
    public bool _affectedByGravity = true;

    [Header("Vator Variables")]
    public string _vatorType;
    // controls whether or not this is a vator floating platform
    public bool _vatorFloatingPlatform = false;
    public bool _isHorizontal = false;
    // stored positions the platform can move to 
    public GameObject _floatTarget1;
    public GameObject _floatTarget2;

    // scales the action speed of everything done by this transformer
    float _gravityBaseScaleValue;



    // Update is called once per frame
    void FixedUpdate()
    {
        // moves the platform upwards
        if(_floating)
        {
            movePlatform();
        }
    }

    #region Moving Platform
    
    public void movePlatform()
    {

    }

    #endregion

    #region Platform Movement

    // queues a float for this platform
    public void QueueMovement(Vector2 playerPosition, float emotionRadius, float duration, string type)
    {
        if(_vatorType == type)
        {
            if (_vatorFloatingPlatform)
            {
                QueueVatorPlatformMovement();
            }
            else
            {
                QueuePlatformMovement(playerPosition, emotionRadius, duration);
            }
        }
    }

    // swaps the elevator float target and sets the platform to be floating
    public void QueueVatorPlatformMovement()
    {
        _floating = true;

    }

    // floats the platform
    public void QueuePlatformMovement(Vector2 playerPosition, float emotionRadius, float duration)
    {

    }

    // sets the slow multiplier for this object
    public void SetSlowMultiplier()
    {

    }

    #endregion
}
