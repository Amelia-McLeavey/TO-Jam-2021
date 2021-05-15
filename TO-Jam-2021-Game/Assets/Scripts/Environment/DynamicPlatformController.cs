using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicPlatformController : MonoBehaviour
{

    // controls whether or not this is a vator floating platform
    public bool _binaryVatorPlatform = false;

    [Header("States")]
    public bool _translating = false;
    public bool _holdingPosition = false;
    public bool _slowed = false;
    public bool _atTargetPosition = false;

    [Header("Timers")]
    public float _slowTimer;
    public float _maxTimeSlowed;
    public float _translationTimer;
    public float _maxTimeTranslated;

    [Header("References")]
    public Rigidbody2D _rigidbody;

    [Header("Universal Behavior Variables")]
    public Vector3 _startingPosition;
    public float _tranlateSpeed = 1.0f;
    // position the platform is translating towards
    public Vector3 _target = new Vector2();
    public float _slowMultiplier = 1.0f;
    // toggles whether or not this platform is affected by gravity
    public bool _affectedByGravity = true;

    [Header("Vator Variables")]
    public string _vatorType;
    public bool _isHorizontal = false;
    // stored positions the platform can move to 
    public GameObject _floatTarget1;
    public GameObject _floatTarget2;

    // scales the action speed of everything done by this transformer
    float _gravityBaseScaleValue;

    #region Initialization

    private void Start()
    {
        _gravityBaseScaleValue = _rigidbody.gravityScale;
        _startingPosition = transform.position;
        // ensure that starting position z is ignored
        _startingPosition.z = 0; 
    }

    #endregion

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateTimers();

        // update gravity only if we are not moving this frame
        if(!_translating && _affectedByGravity)
        {
            UpdateGravity();
        }

        // moves the platform upwards
        if(_translating)
        {
            movePlatform();
        }
    }

    #region Updates

    // update timers
    void UpdateTimers()
    {
        _slowTimer = _slowTimer + Time.deltaTime;

        // resets the slowmultipier once the timer has exceeded its limit
        if(_slowTimer > _maxTimeSlowed)
        {
            _slowMultiplier = 1.0f; 
        }

        // translation timer only applies if this is a resetting platform
        if(!_binaryVatorPlatform && _atTargetPosition)
        {
            _translationTimer = _translationTimer + Time.deltaTime;

            if (_translationTimer > _maxTimeTranslated)
            {
                _translating = true;
                _target = _startingPosition;
            }
        }

    }

    // update gravity
    void UpdateGravity()
    {
        // convert the bool to an int
        int abgInt = _affectedByGravity ? 1 : 0;

        // update this objects gravity scalar to be equal to its base multiplier times the slow multiplier
        _rigidbody.gravityScale = (_gravityBaseScaleValue * _slowMultiplier) * abgInt;
    }

    #endregion

    #region Moving Platform

    // moves the platform ensuring that we do not overshoot our target
    public void movePlatform()
    {
        // set the temp delta to the difference between the target position and the player position
        Vector3 delta = (_target - transform.position);
        // ensure that we are ignoring the z axis
        delta.z = 0; 
        float distToTarget = delta.magnitude;        
        delta.Normalize();

        // calculate the delta for this frame accounting for the slowmultiplier
        delta = delta * _tranlateSpeed * Time.deltaTime * _slowMultiplier; 

        // apply the delta only if it is less then the distance between the current position and the target position
        // otherwise set the position of this object to be equal to the target position
        if(distToTarget > delta.magnitude)
        {
            transform.position = transform.position + delta;
        } else
        {
            transform.position = _target;
            _translating = false;

            // only store that we are at the target position if this target position is not the start position
            if(_target != _startingPosition)
            {
                _atTargetPosition = true;
            }
        }
    }

    #endregion

    #region Platform Movement

    // queues a float for this platform
    public void QueueMovement(Vector2 playerPosition, float emotionRadius, float duration, string type)
    {
        if(_vatorType == type)
        {
            if (_binaryVatorPlatform)
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
        _translating = true;
    }

    // floats the platform
    public void QueuePlatformMovement(Vector2 playerPosition, float emotionRadius, float duration)
    {
        // reset the translation timers 
        _translationTimer = 0.0f;
        _maxTimeTranslated = duration;


    }

    // sets the slow multiplier for this object
    public void SetSlowMultiplier(float slowMultiplier, float duration)
    {
        // reset the slow timer 
        _slowMultiplier = slowMultiplier;
        _slowTimer = 0.0f;
        _maxTimeSlowed = duration;
    }

    #endregion

    // ensure that when this collides with another object it stops its translation immediately
    private void OnCollisionEnter(Collision collision)
    {
        
        /// !!! NEEDS TO HAVE DETECTION FOR COLLISIONS TO STOP TRANSLATION ON COLLISION !!!

    }
}
