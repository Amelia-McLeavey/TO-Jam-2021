using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("Momentum and Facing")]
    // store the velocity of the player controller 
    public Vector2 velocity = new Vector2(0, 0);
    public Rigidbody2D playerBody;
    // The rate of acceleration that will be applied on any given frame
    float aDeltaV;
    float dDeltaV;
    float xLastInputFacingDirection;
    public enum FacingDirection { Left, Right }

    [Header("Jump Values")]
    // apex jump hieght of the minimum jump
    public float m_jumpApexHeight = 2.25f;
    // stores the previous jump apex hieght value to track changes to m_jumpApexHeight
    float pjahValue;
    // apex jump time of the minimum jump
    public float m_jumpApexTime = 0.3f;
    // stores the previous jump apext time value to track changes to m_jumpApexTime
    float pjatValue;
    public float m_jumpBufferTime = 0.1f;
    public float m_terminalVelocity = 22.0f;
    // variables for controlling the jump buffer
    public bool jumpBuffered;
    public float bufferWindow;
    public float currrentJumpBufferTime;
    // stores the hieght ratio between maximum and minimum jumps to 2, meaning a maximum jump is twice the hieght of a minimum jump
    public float minMaxJumpRatio = 2.25f;
    // Calculated using the minMaxJumpRatio, max Additional jump height limits the maximum amount of additional height the player can gain from a jump
    float maxAdditionalJumpHeight;
    public float currentAdditionalJumpHeight;
    // tracks jump reset frames for making perfectly replicatable max additional jump heights
    public bool jumpReset = false;
    float initialJumpVelocity;

    [Header("Acceleraton Variables")]
    public float m_accelerationTimeFromRest = 0.36f;
    public float m_decelerationTimeToRest = 0.16f;
    public float m_maxHorizontalSpeed = 6.0f;
    // controls the ratio of horizontal motion allowed at maximum while falling
    public float maxFallHorizontalVelocity = 0.75f;

    [Header("States")]
    // Store player input and jump input
    public bool spaceReleased = false;
    bool spacePressed = false;
    bool spaceCurrentlyPressed = false;
    Vector2 input = new Vector2();

    // state variables
    public bool grounded = false;
    public bool jumping = false;
    public bool falling = false;
    public bool inputActive = true;

    [Header("Grounding")]
    // track the layer that platforms are located on
    public Vector2 platformLayer;
    // grounding and gravity related variables
    // gravity is determined by the apex jump time and the apex jump hieght
    float gravity;
    // tracks whether or not gravity has been applied this frame
    bool gravityApplied = false;
    // maximum distance from the ground that we classify as grounded
    public float collisionThreshold = 0.005f;
    // time before falling after walking off ledge where player can still jump
    float CoyoteTime = 0.1f;
    bool inCoyoteTime = false;
    public float m_coyoteTime = 0.1f;


    // overrides any acceleration times that are set to 0, this effectively creates a one frame delay 
    // preventing true 0 aceleration, as true 0 acceleration breaks my method of calculating aDeltaV and dDeltaV. 
    private float effectiveZero = 0.00001f;
    #endregion

    #region Update Methods

    // Add additional methods such as Start, Update, FixedUpdate, or whatever else you think is necessary, below.
    private void Start()
    {
        // Run Setup of the Controller

        // constrain our rigidbody so that we dont rotate during collision resolution
        // I cant change the prefab but I can hard code it ;o) 
        gameObject.GetComponent<Rigidbody2D>().freezeRotation = true;

        // store the layer of the ground
        platformLayer.x = LayerMask.GetMask("Platform");
        platformLayer.y = LayerMask.GetMask("SelectablePlatformIgnoring");

        // store the values for jumpApexHeight so we can recalculate when they change
        pjahValue = m_jumpApexHeight;
        pjatValue = m_jumpApexTime;

        // run all our initial calculations
        CalculateJumpData();
    }

    private void Update()
    {
        // update whether or not the player is grounded
        IsGrounded();

        // check that our jump data has not changed
        CheckJumpData();

        if(inputActive)
        {
            // store directional input for this frames calculations
            UpdateInput();
        } else
        {
            input.x = 0;
            input.y = 0;

            spacePressed = false;

            if (spaceCurrentlyPressed == true)
            {
                spaceReleased = true;
                spaceCurrentlyPressed = false;
            }
        }


        UpdateMovement();

        // update for moving platforms
        detectMovingPlatforms();
    }

    // applies gravity to the character for this frame
    private void UpdateGravity()
    {
        // if we are not in coyote time, we are not grounded then gravity to our Y velocity
        //if(!inCoyoteTime && !grounded && !IsGrounded() && !gravityApplied)
        if ((jumping || falling) && !gravityApplied)
        {
            // apply gravity to the y velocity of the character
            velocity.y = velocity.y - (gravity * Time.deltaTime);
            gravityApplied = true;
        }

        // apply terminal velocity
        velocity.y = Mathf.Clamp(velocity.y, -m_terminalVelocity, initialJumpVelocity);

    }

    // updates all movement inputs for the character controller
    public void UpdateMovement()
    {
        //updates whether or not we are grounded
        UpdateCoyoteTime();

        UpdateJumpBuffering();

        // !! requires input setup !!
        UpdateHorizontalVelocity();
        UpdateVerticalVelocity();

        // check if the player has collided with a platform from above
        detectBonk();

        //update the rigidbodies velocity
        playerBody.velocity = velocity;

    }

    // updates everything for the characters vertical velocity
    void UpdateVerticalVelocity()
    {
        gravityApplied = false;

        if (jumping)
        {
            // applies additional jump force if the player is holding jump input
            UpdateAdditionalJumpHeight();
        }

        // updates the players jump
        UpdateJump();

        // update falling
        UpdateFalling();

        // applies gravity to the player when appropriate
        UpdateGravity();
    }

    // updates the falling state of the player 
    void UpdateFalling()
    {
        falling = true;

        if (velocity.y < 0)
        {
            return;
        } else if (grounded || jumping)
        {
            falling = false;
        }
    }

    // updates the jump state for the player controller
    void UpdateJump()
    {
        // if the player is not jumping, they are grounded, and inputed that they want to jump, jump
        if ((spacePressed || jumpBuffered) && !jumping && grounded)
        {
            // set jumping to true reset our players coyote time and set the player to no longer be grounded
            jumping = true;
            grounded = false;
            ResetJumpBuffer();
            ResetCoyoteTime();

            //appply initial jump velocity
            velocity.y = initialJumpVelocity;
        }

        // reset jump velocity if this is a reset frame
        if (jumpReset)
        {
            //appply initial jump velocity
            velocity.y = initialJumpVelocity;
            jumpReset = false;
        }
    }

    // applies additional jump height to the player if they are holding space
    void UpdateAdditionalJumpHeight()
    {
        // stores the difference between the addtional jump height at beginning and end
        float actualDeltaY;
        float targetDeltaY = maxAdditionalJumpHeight - currentAdditionalJumpHeight;

        // if the player is holding jump continue resetting jump velocity
        // until the player is hitting the jump ratio height
        if (jumping && spaceCurrentlyPressed && !spaceReleased)
        {
            // if we have not added more additional jump height then the maximum alloted add more
            if (currentAdditionalJumpHeight < maxAdditionalJumpHeight)
            {
                velocity.y = initialJumpVelocity;

                // update gravity again as it was just overwritten
                UpdateGravity();

                // store the amount of gained height from this additional jump height
                currentAdditionalJumpHeight = currentAdditionalJumpHeight + (velocity.y * Time.deltaTime);

                actualDeltaY = maxAdditionalJumpHeight - currentAdditionalJumpHeight;

                TrimAdditionalJumpHeight(actualDeltaY, targetDeltaY);
            }
        } else
        {
            // ensure we are not currently jumping before resetting additional jump height
            // prevents flying
            if (!jumping)
            {
                currentAdditionalJumpHeight = 0;
            }
            return;
        }
    }

    // updates the buffering system for jumping including buffering a jump
    void UpdateJumpBuffering()
    {
        // if we have a jump buffered increment our buffer timer 
        if (jumpBuffered)
        {
            currrentJumpBufferTime = currrentJumpBufferTime + Time.deltaTime;
        }

        // if space is pressed set ump buffered to true
        // this will not cause jump a to buffer an aditional jump as jump buffers are reset on jumping
        if (spacePressed)
        {
            jumpBuffered = true;
            currrentJumpBufferTime = 0;
        }

        // reset buffering if we dont jump before the buffer window closes
        if (currrentJumpBufferTime > bufferWindow)
        {
            ResetJumpBuffer();
        }
    }

    // updates whether or not the player is grounded based off of coyoteTime
    public void UpdateCoyoteTime()
    {
        // if we are not jumping, not falling, and we are not grounded we are in CoyoteTime
        if (!jumping && !falling && !IsGrounded() && grounded)
        {
            inCoyoteTime = true;

            // if coyotetime has maxed out, start falling and set grounded to false
            if (CoyoteTime > m_coyoteTime)
            {
                grounded = false;
                falling = true;
                ResetCoyoteTime();
            }
        }
        else
        {
            inCoyoteTime = false;
        }

        if (inCoyoteTime == true)
        {
            CoyoteTime = CoyoteTime + Time.deltaTime;
        }
    }

    // calculates the momentum of the ship for a given frame
    void UpdateHorizontalVelocity()
    {
        float projectedXVelocity = velocity.x;

        // predict this frames acceleration changes
        CalculateDeltaV();

        // if we have no input do not accelerate, decelerate
        if (input.x != 0)
        {
            projectedXVelocity = projectedXVelocity + (aDeltaV * input.x);
        }
        else if (Mathf.Abs(projectedXVelocity) > 0)
        {
            projectedXVelocity = projectedXVelocity - Decelerate(projectedXVelocity);
        }

        // clamp speed for x axis while falling to max horizontal velocity only if we have no already 
        // exceeded it, otherwise clamp it to the last known x velocity of the player
        if (falling)
        {
            // if we are traveleing slower then or equal to the max fall horizontal velocity apply then apply projected velocity and clamp to the maximum horizontal velocity
            // otherwise only apply projectedXVelocity if we are decelerating
            if (Mathf.Abs(velocity.x) <= (m_maxHorizontalSpeed * maxFallHorizontalVelocity))
            {
                velocity.x = projectedXVelocity;
                velocity.x = Mathf.Clamp(velocity.x, -m_maxHorizontalSpeed * maxFallHorizontalVelocity, m_maxHorizontalSpeed * maxFallHorizontalVelocity);
            }
            else
            {
                velocity.x = projectedXVelocity;
                velocity.x = Mathf.Clamp(velocity.x, -m_maxHorizontalSpeed, m_maxHorizontalSpeed);
            }

        } else
        {
            velocity.x = projectedXVelocity;
            velocity.x = Mathf.Clamp(velocity.x, -m_maxHorizontalSpeed, m_maxHorizontalSpeed);
        }
    }

    // updates all the player inputs for the character controller
    void UpdateInput() {

        // update inputs
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // figure out whether or not space was pressed
        spacePressed = Input.GetKeyDown("space");

        if (spaceCurrentlyPressed != Input.GetKey("space"))
        {
            spaceCurrentlyPressed = Input.GetKey("space");
            if (spaceCurrentlyPressed == false)
            {
                spaceReleased = true;
            }
        }

        if (input.x != 0)
        {
            xLastInputFacingDirection = input.x;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            GetComponent<BalloonAOE>().PopBalloon("J");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            GetComponent<BalloonAOE>().PopBalloon("K");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            GetComponent<BalloonAOE>().PopBalloon("L");
        }
    }

    #endregion

    #region Calculation Methods
    // calculates the deltaV of the player for this frame
    public void CalculateDeltaV()
    {
        if (m_accelerationTimeFromRest < effectiveZero)
        {
            m_accelerationTimeFromRest = effectiveZero;
        }

        if (m_decelerationTimeToRest < effectiveZero)
        {
            m_decelerationTimeToRest = effectiveZero;
        }

        aDeltaV = (m_maxHorizontalSpeed / m_accelerationTimeFromRest) * Time.deltaTime;
        dDeltaV = (m_maxHorizontalSpeed / m_decelerationTimeToRest) * Time.deltaTime;
    }



    // calculates the proper deceleration for the passed veclocity
    float Decelerate(float velocity)
    {
        float decelValue = CalcDecel(velocity);

        // ensure if the decel value is greater then the current velocity we decel straight to 0
        if (Mathf.Abs(velocity) < Mathf.Abs(decelValue))
        {
            return velocity;
        }
        else
        {
            return decelValue;
        }
    }

    // returns the decel for the passed momentum
    float CalcDecel(float velocity)
    {
        return dDeltaV * (velocity / Mathf.Abs(velocity));
    }

    // reset coyotetimer and sets our state to be out of coyotetime
    public void ResetCoyoteTime()
    {
        inCoyoteTime = false;
        CoyoteTime = 0;
    }

    void ResetJumpBuffer()
    {
        jumpBuffered = false;
        currrentJumpBufferTime = 0; 
    }

    // recalculates the values for jumping
    void CalculateJumpValues()
    {
        // calculate the initial jump velocity
        // first calculate the average velocity which is equal to the hieght of the jump divided by the time
        initialJumpVelocity = m_jumpApexHeight / m_jumpApexTime;

        // calculate the initial velocity which is double the average; average is the maximum velocity + the minimum divided by 2
        // minimum velocity in this case is 0 as jump velocity at the apex is 0, meaning that double the average velocity is the initial
        initialJumpVelocity = initialJumpVelocity * 2;

        // calculate maximum additional jump height
        maxAdditionalJumpHeight = (m_jumpApexHeight * minMaxJumpRatio) - m_jumpApexHeight;
    }

    // calculates the value for gravity
    void CalculateGravity()
    {
        // acceleration of gravity is equal to the distance it took the jump to decelerate divided by the time of the jump squared
        gravity = (2 * m_jumpApexHeight) / (Mathf.Pow(m_jumpApexTime, 2));
    }


    void CheckJumpData()
    {
        bool change = false;

        // check that jump apex height has not changed
        if (pjahValue != m_jumpApexHeight)
        {
            change = true;
            pjahValue = m_jumpApexHeight;
        }

        // check that jump apex time has not changed
        if (pjatValue != m_jumpApexTime)
        {
            change = true;
            pjatValue = m_jumpApexTime;
        }

        // if we have changed a value recalculate our jump and gravity values
        if (change)
        {
            CalculateJumpData();
        }
    }

    // used to forcibly calculate jump data
    void CalculateJumpData()
    {
        CalculateJumpValues();
        CalculateGravity();
    }

    #endregion

    #region Functions
    // trims the current velocity to constrain it to the maximum alloted additional jump height
    void TrimAdditionalJumpHeight(float actualDeltaY, float targetDeltaY)
    {
        // trim velocity to ensure we dont exceed maximum additional jump height
        if (currentAdditionalJumpHeight > maxAdditionalJumpHeight)
        {
            // store the ratio of the target additional jump height to the ratio
            // apply that ratio to the velocty to ensure that we do not exceed our maximum jump height
            float ratio = targetDeltaY / actualDeltaY;
            velocity.y = velocity.y * ratio;
            jumpReset = true;
        } else
        {
            jumpReset = true;
        }
    }

    // if the absolute value of the x velocty is greater then 0 return true else return false
    public bool IsWalking()
    {
        if (Mathf.Abs(velocity.x) > 0)
        {
            return true;
        }

        return false;
    }

    // checks if the player is grounded
    public bool IsGrounded()
    {
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, maximumGroundingDistance, platformLayer);
        RaycastHit2D hit1 = Physics2D.BoxCast(transform.position, new Vector2(GetComponent<CapsuleCollider2D>().size.x - 0.18f, collisionThreshold * 2), 0, Vector2.down, 0, (int)platformLayer.x);
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, maximumGroundingDistance, platformLayer);
        RaycastHit2D hit2 = Physics2D.BoxCast(transform.position, new Vector2(GetComponent<CapsuleCollider2D>().size.x - 0.18f, collisionThreshold * 2), 0, Vector2.down, 0, (int)platformLayer.y);


        if (hit1.collider != null || hit2.collider != null)
        {
            falling = false;
            grounded = true;
            currentAdditionalJumpHeight = 0;
            spaceReleased = false;
            if(velocity.y < 0)
            {
                velocity.y = 0;
                jumping = false;

                GetComponentInChildren<PlayerAudio>().PlayCharacterLand();
            }

            ResetCoyoteTime();
            return true;
        }
        else
        {
            return false;
        }
    }
    
    // detects if the player has bonked their head on an object above them
    public void detectBonk()
    {
        float hitboxHieght = transform.GetComponent<CapsuleCollider2D>().size.y;

        // send out a ray detecting platforms over the players head
        RaycastHit2D hit1 = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y + 0.01f + collisionThreshold, 0), Vector2.up, hitboxHieght + (collisionThreshold), (int)platformLayer.x);
        // send out a ray detecting platforms over the players head
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y + 0.01f + collisionThreshold, 0), Vector2.up, hitboxHieght + (collisionThreshold), (int)platformLayer.y);

        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + hitboxHieght + collisionThreshold, transform.position.z) , Vector2.up, Color.white);

        // if the player is in contact with an overhead platform kill their upwards momentum
        if((hit1.collider != null || hit2.collider != null) && velocity.y > 0)
        {
            velocity.y = 0;
            // do not allow them to boost jump
            spaceReleased = true;
        }
    }

    // tracks whether or not the player is on a moving platform
    // also compensates for them being on one or not
    public void detectMovingPlatforms()
    {
        // cast a ray downwards and detect if there is a platform
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, collisionThreshold * 2, LayerMask.GetMask("Platform"));

        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, maximumGroundingDistance);
        //RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(GetComponent<CapsuleCollider2D>().size.x - 0.3f, collisionThreshold * 2), 0, Vector2.down, collisionThreshold * 2, (int)platformLayer.y);

        if (hit.collider != null && hit.collider.tag == "Moving")
        {
            MovingPlatform script = hit.collider.GetComponent<MovingPlatform>();    

            if(IsGrounded())
            {
                // if the player is current standing on the platform then move them including y
                transform.position = transform.position + new Vector3(script._delta.x, script._delta.y, 0);
            } else
            {
                // otherwise do not move the player on the y axis only the x
                transform.position = transform.position + new Vector3(script._delta.x, 0, 0);
            }
        }
    }


    // returns the direction the player is facing
    public FacingDirection GetFacingDirection()
    {
        if (xLastInputFacingDirection > 0)
        {
            return FacingDirection.Right;
        }
        return FacingDirection.Left;
    }

    #endregion
}
