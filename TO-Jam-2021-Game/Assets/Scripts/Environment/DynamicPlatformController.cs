using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicPlatformController : MonoBehaviour
{
    
    [Header("Control Variables")]
    public string _type;
    // controls whether or not this is a vator floating platform
    public bool _binaryVatorPlatform = false;
    // toggles whether or not this platform is affected by gravity
    public bool _affectedByGravity = true;
    public bool _affectedBySlow = false;
    public bool _horizontalVator = false;
    public bool _automaticVator = false;

    [Header("States")]

    // the translation state of the platform/block
    public bool _translatingX = false;
    public bool _translatingY = false; 
    public bool _slowed = false;
    public bool _floating = false;
    public bool _gravityLocked = false;
    public bool _atRest = false;

    //its falling if everything is false but affected by grav true
    [Header("Timers")]
    public float _slowTimer;
    public float _maxTimeSlowed;
    public float _floatTimer;
    public float _maxFloatTime;

    [Header("References")]
    public Rigidbody2D _rigidbody;

    [Header("Movement Variables")]
    public Vector3 _delta = new Vector3(0,0,0);
    float pfDeltaY;
    public float _translateSpeed = 1.0f;
    public float _slowMultiplier = 1.0f;
    // position the platform is translating towards
    public Vector3 _target = new Vector2();

    // tracks the initial rigidbody gravity scale
    float _gravityBaseScaleValue;    

    [Header("Vator References")]
    // stored positions the platform can move to 
    public GameObject _transformTarget1;
    public GameObject _transformTarget2;
    PlatformAudio m_audio;


    #region Initialization

    // initilialize the dynamic platform
    private void Start()
    {
        // get this dynamic platforms rigidbody
        _rigidbody = GetComponent<Rigidbody2D>();

        // store this dyamic platforms base gravity scale value incase it is needed later
        _gravityBaseScaleValue = _rigidbody.gravityScale;

        // ensure that binary vator platforms have the correct settings
        if (_binaryVatorPlatform)
        {
            // move this vator to its starting position
            transform.position = _transformTarget1.transform.position;

            // ensure that it has no gravity scale and a velocity of 0
            _rigidbody.gravityScale = 0;
            _rigidbody.velocity = new Vector3(0, 0, 0);

            if(!_horizontalVator)
            {
                _gravityLocked = true;
            }

            // if this is an automatic vator set it to begin moving immediately
            if (_automaticVator)
            {
                DistSwapTransformTarget();

                if (_horizontalVator)
                {
                    _translatingX = true;
                }
                else
                {
                    _translatingY = true;
                }
            }
        }

        if(!_affectedByGravity)
        {
            _atRest = true;
        }

        m_audio = GetComponent<PlatformAudio>();
    }

    #endregion

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateTimers();

        UpdateMotion();

     

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
        if(_floating && !_translatingY)
        {
            _floatTimer = _floatTimer + Time.deltaTime;

            if (_floatTimer >= _maxFloatTime)
            {
                _floating = false;
                _gravityLocked = false;
            }
        }
    }

    // update gravity
    void UpdateGravity()
    {
        // update this objects gravity scalar to be equal to its base multiplier times the slow multiplier
        _rigidbody.gravityScale = (_gravityBaseScaleValue * _slowMultiplier);
    }

    // updates the motion of the dynamic platform
    void UpdateMotion()
    {
        // reset delta
        _delta = new Vector3(0,0,0);

        _delta.y = _translatingY ? _target.y - gameObject.transform.position.y : 0;
        _delta.x = _translatingX ? _target.x - gameObject.transform.position.x : 0;

        float distToTarget = _delta.magnitude;

        _delta.Normalize();

        // calculate the delta for this frame accounting for the slowmultiplier
        _delta = _delta * _translateSpeed * Time.deltaTime * _slowMultiplier;

        // check if gravity is being applied, if so, retain the current y velocity assuming its gravity from the previous frame
        if (!_floating && _affectedByGravity && (!_translatingY && !_horizontalVator) && !_gravityLocked && !_atRest)
        {
            if(GravitationalOvershoot() && _binaryVatorPlatform)
            {
                _rigidbody.gravityScale = 0;
                _rigidbody.velocity = new Vector3();
                _gravityLocked = true;
                transform.position = _transformTarget1.transform.position;
            } else
            {
                UpdateGravity();
            }
        } else
        {
            _rigidbody.gravityScale = 0;
            _rigidbody.velocity = new Vector3(0, 0, 0);
        }

        // check if there is x overshoot, if so set the x value equal to the target
        if((Mathf.Abs(_delta.x) >= Mathf.Abs(_target.x - gameObject.transform.position.x)) && _translatingX)
        {
            gameObject.transform.position = gameObject.transform.position  + new Vector3(_delta.x, 0, 0);

            _delta.x = 0;

            if (!_automaticVator)
            {
                _translatingX = false;
                _target.x = gameObject.transform.position.x;
            } else
            {
                DistSwapTransformTarget();
            }
        }

        // check if there is y overshoot, set the y value equal to the target
        if ((Mathf.Abs(_delta.y) >= Mathf.Abs(_target.y - gameObject.transform.position.y)) && _translatingY)
        {
            gameObject.transform.position = gameObject.transform.position + new Vector3(0, _delta.y, 0);

            _delta.y = 0;

            if (!_automaticVator)
            {
                _translatingY = false;

                _target.y = gameObject.transform.position.y;

                _floating = true;

                _gravityLocked = true;
            } else
            {
                DistSwapTransformTarget();
            }
        }

        if(_floating || _gravityLocked)
        {
            _rigidbody.velocity = new Vector3(); 
        }

        transform.position = transform.position + _delta;
    }

    #endregion

    #region Checks
    // checks for a gravitation position overshoot on vertical vators
    public bool GravitationalOvershoot()
    {
        // if the velocity of the block will cuase it to accelerate past the transform target origin return true
        if(_rigidbody.velocity.magnitude > (transform.position - _transformTarget1.transform.position).magnitude)
        {
            return true;
        }

        return false;
    }


    #endregion

    #region Platform Movement

    // queues a float for this platform
    // <type> can be loaded with either "Shove" or "Float"
    public void QueueMovement(Vector2 playerPosition, float emotionRadius, float duration, string type)
    {
        if((_type == type || _type == "Universal") && !_automaticVator)
        {
            if (_binaryVatorPlatform)
            {
                QueueVatorPlatformMovement(duration);
            }
            else
            {
                QueuePlatformMovement(playerPosition, emotionRadius, duration, type);
            }

            m_audio.PlayRock(1,duration);
        }
    }

   

    // swaps the elevator float target and sets the platform to be floating
    public void QueueVatorPlatformMovement(float duration)
    {
        //FMOD either translate XoY means moving
        if(_horizontalVator)
        {
            _translatingX = true;
        } else
        {
            _translatingY = true;
        }

        DistSwapTransformTarget(duration);
    }

    // transforms a platform based off of the type of passed transform
    public void QueuePlatformMovement(Vector2 playerPosition, float emotionRadius, float duration, string type)
    {
        // reset the translation timers 
        _floatTimer = 0.0f;
        _maxFloatTime = duration;

        Vector2 tempPosition = _target;

        switch(type)
        {
            case "Shove":
                int relativeDirection = (gameObject.transform.position.x - playerPosition.x) > 0 ? 1 : -1;
                tempPosition.x = playerPosition.x + ((emotionRadius + (gameObject.GetComponent<BoxCollider2D>().size.x / 2)) * relativeDirection);
                _target.x = tempPosition.x;
                _translatingX = true;
                break;
            case "Float":
                tempPosition.y = playerPosition.y + emotionRadius - (gameObject.GetComponent<BoxCollider2D>().size.y / 2);
                _target.y = tempPosition.y;
                _translatingY = true;
                break;
        }
    }

    // sets the slow multiplier for this object
    public void SetSlowMultiplier(float slowMultiplier, float duration)
    {
        if(_affectedBySlow)
        {
            // reset the slow timer 
            _slowMultiplier = slowMultiplier;
            _slowTimer = 0.0f;
            _maxTimeSlowed = duration;
        }
    }

    // swaps the transform target based off of distance
    public void DistSwapTransformTarget()
    {
        float tempDistToTT1 = Vector3.Distance(transform.position, _transformTarget1.transform.position);
        float tempDistToTT2 = Vector3.Distance(transform.position, _transformTarget2.transform.position);

        if (tempDistToTT1 > tempDistToTT2)
        {
            _target = _transformTarget1.transform.position;
        }
        else
        {
            _target = _transformTarget2.transform.position;
        }
    }

    // swaps the transform target based off of distance
    public void DistSwapTransformTarget(float duration)
    {
        float tempDistToTT1 = Vector3.Distance(transform.position, _transformTarget1.transform.position);
        float tempDistToTT2 = Vector3.Distance(transform.position, _transformTarget2.transform.position);

        if (tempDistToTT1 > tempDistToTT2)
        {
            _target = _transformTarget1.transform.position;
        }
        else
        {
            _target = _transformTarget2.transform.position;

            if (!_horizontalVator)
            {
                _floatTimer = 0.0f;
                _maxFloatTime = duration;
            }
        }
    }
    #endregion

    // ensure that when this collides with another object it stops its translation immediately
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("called");

        if (collision.gameObject.layer == gameObject.layer && collision.gameObject.tag != "Player")
        {
            // undo the last step of movement
            transform.position = transform.position - _delta;

            Debug.Log("translation canceled");
            _translatingX = false;
            _target.x = transform.position.x;
            _translatingY = false;
            _target.y = transform.position.y;
            m_audio.PlayRock(0, 3);
        }
    }
}
