using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Redo Notes:
-refactor movement to be force based
-used add force with force mode acceleration for movement and then clamp the movespeed
-for energy conservation don't scale the velocity directly, just increase the clamp by the calculated change in velocity
-this way slide can just apply friction through a force as well

Slide Notes:
-locks movement to direction the player was moving when they entered the slide
-player can still look around, but moves in same direction
-player can accelerate to even higher speeds than when running
-player gets slide force applied determined by deltaY (since player naturally comes to a stop anyways when no forces are applied)
-playyer has initial slide speed, but after half a second or something they start slowing down
-velocity/momentum is preserved when entering slide, p=mv
-player presses slide key, start sliding, cotinue sliding until they either slow down enough, jump, or press the slide key again

Wallrunning Notes:
-check if there is a wall in range and that the player is off the ground
-find forward vector of wall to direct player's momentum/velocity along that vector
-once player has started wallrunning, set y-velocity to zero, set lateral velocity to correct/prior momentum/velocity
-then add force like normal movement, clamping it
-have player slow down over time while wallrunning, like how slide slows down
-create wall jump

Gravity Notes:
-increase gravity so that the player stays grounded, but set the gravity back to normal when they jump or fall
-no gravity scale with rigidbody, so need to do manual gravity

Speed Clamp Notes:
-potential way to cap speed without directly setting velocity (only using forces):
-rigidbody.AddForce(targetSpeed - rigidbody.velocity, ForceMode.Velocity)
-by subtracting target speed by the current velocity, this does the same thing as changing the velocity directly without the flaws of doing so
*/

public class MikeMovement : MonoBehaviour
{
    [Header("Base Movement")]
        float verticalInput;
        float horizontalInput;
        [SerializeField] float moveForce;
        [SerializeField] Rigidbody rb;
        Vector3 movementVector;
        [SerializeField] float maxWalkSpeed;
        [SerializeField] float minWalkSpeed;
        [SerializeField] float maxSlopeAngle;
        RaycastHit slopeHit;
        [SerializeField] float groundDrag;
        [SerializeField] Collider playerCollider;

    [Header("Jump")]
        [SerializeField] float jumpForce;
        bool isGrounded;
        Collider[] groundColliders;
        [SerializeField] Transform groundCheck;
        [SerializeField] float groundCheckRadius;
        [SerializeField] LayerMask groundLayer;
        //coyote jump
        bool canJump;
        [SerializeField] float coyoteJumpTime;

    [Header("Slide")]
        bool isSliding = false;
        [SerializeField] float slideForce;
        [SerializeField] Camera playerCam;
        Vector3 slideDirection;
        [SerializeField] float maxSlideSpeed;
        Transform playerModel;
        float slideDeltaY;
        float slideInitialY;
        float lastSlideSpeed;
        [SerializeField] float slideFriction;

    [Header("Wallrunning")]
        [SerializeField] float wallRunFriction;
        //speed at which wallrun friction will begin to increase
        [SerializeField] float wallFrictionSpeed;
        [SerializeField] float wallRunGravity;
        [SerializeField] float wallRunForce;
        [SerializeField] float minJumpHeight;
        [SerializeField] LayerMask wallLayer;
        [SerializeField] float wallCheckDistance;
        RaycastHit leftWallHit;
        RaycastHit rightWallHit;
        bool wallLeft;
        bool wallRight;
        bool isWallRunning;
        bool canWallRun;
        [SerializeField] float wallrunCooldown;
        float lastWallRunSpeed;

    [Header("Energy Conservation")]
        float lastYPosition;
        float currentYPosition;
        float deltaV;

    [Header("Gravity")]
        [SerializeField] float groundGravityScale;
        [SerializeField] float airGravityScale;
        float gravityScalar;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        gravityScalar = airGravityScale;
        playerModel = transform.GetChild(0).gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //print(OnSlope());

        currentYPosition = transform.position.y;

        // print(lastYPosition);
        // print(currentYPosition);

        //ground check switches value of isGrounded and canJump variable
        GroundCheck();

        WallCheck();

        if(rb.velocity.magnitude < 0.3f) {
            lastYPosition = transform.position.y;
            //print("new y pos");
        }

        //drag
        if(!isSliding && isGrounded) {
            rb.drag = groundDrag;
        } else {
            rb.drag = 0;
        }

        if(isGrounded && rb.velocity.magnitude > 0.5f) {
            EnergyConservation();
        }
        //calculate the change in y pos
        //calculate the change in potential energy
        //change in potential energy = change in kinetic energy
        //calculate change in velocity

        //get movement input
        if(!isSliding) {
            verticalInput = Input.GetAxisRaw("Vertical");
            horizontalInput = Input.GetAxisRaw("Horizontal");
            //print("V: " + verticalInput);
            //print("H: " + horizontalInput);
            movementVector = (transform.forward * verticalInput) + (transform.right * horizontalInput).normalized;
            movementVector = movementVector * moveForce;
        }
        //print(currentVelocity);

        //get jump input
        if(Input.GetKeyDown("space") && isGrounded && canJump) {
            Jump();
        }

        //sliding input
        if(Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && !isSliding && rb.velocity.magnitude > 1) {
            StartSlide();
        } else if(Input.GetKeyDown(KeyCode.LeftShift) && isSliding && rb.velocity.magnitude > 1) {
            ExitSlide();
        } else if(isSliding && rb.velocity.magnitude < 1) {
            ExitSlide();
        }

        //print(canWallRun);

        //wallrunning input
        if((wallLeft || wallRight) && verticalInput > 0.1f && AboveGround() && !isWallRunning && canWallRun) {
            //start wallrun
            StartWallRun();
            //print("wallrun started");
        } else if((verticalInput < 0.1f || isGrounded || !(wallLeft || wallRight)) && isWallRunning) {
            StopWallRun();
            //print("wallrun stopped");
        }
        
        //walljump input
        if(isWallRunning && Input.GetKeyDown("space")) {
            WallJump();
        }

        //print(maxWalkSpeed + deltaV);
        //print(rb.velocity.magnitude);
        //print(deltaV);

        if(!isSliding && !isWallRunning) {
            ClampSpeed(minWalkSpeed, maxWalkSpeed);
        } else if(isSliding || isWallRunning) {
            ClampSpeed(0, maxSlideSpeed);
        }
    }

    void FixedUpdate()
    {
        //gravity
        if(!OnSlope() && !isWallRunning) {
            ApplyGravity();
        }

        //default movement
        if(!isSliding && !OnSlope() && !isWallRunning) {
            rb.AddForce(movementVector, ForceMode.Force);
            //print("default movement");

        //slope movement
        } else if(!isSliding && OnSlope() && !isWallRunning) {
            rb.AddForce(GetSlopeMoveVector(movementVector, moveForce), ForceMode.Force);
            
            if(rb.velocity.y > 0) {
                //downward force while on slope to keep player on it
                rb.AddForce(-slopeHit.normal * 80f, ForceMode.Force);
            }
            //print("slope movement");

        //slide movement
        } else if (isSliding && !isWallRunning) {
            SlideMovement();
            //print("sliding");

        //wallrun movement    
        } else if(isWallRunning) {
            WallRunMovement();
            //print("wallrunning");
        }
    }

    void EnergyConservation()
    {
        float deltaY = currentYPosition - lastYPosition;
        //deltaY is having too quick of an effect, so gonna divide by a number so it increases at a lower rate
        //different number to divide by depending on whether or not deltaY is negative or positive may be good idea
        deltaY /= 4;

        float deltaU = rb.mass * 9.8f * deltaY;
        //print(deltaU);
        
        //negative U = positive k
        //k = (1/2)m(v^2)
        //v = sqrt(2k/m)
        deltaV = Mathf.Sqrt((2 * Mathf.Abs(deltaU)) / rb.mass);
        if(deltaU > 0) {
            deltaV *= -1;
        }
        //print(deltaV);
        //print(rb.velocity.magnitude);
        
        //----------For Old Velocity Movement-----------
        //oldV * vScalar = newV
        //vScalar = newV/oldV
        //velocityScalar = (rb.velocity.magnitude + deltaV) / rb.velocity.magnitude;
        //clamp velocity scalar
        //velocityScalar = Mathf.Clamp(velocityScalar, 0.5f, 2f);
        //print("scalar" + velocityScalar);
        //----------------------------------------------
    }

    void ClampSpeed(float minSpeed, float maxSpeed)
    {
        //limiting speed on slope
        if(OnSlope()) {
            if(rb.velocity.magnitude > maxSpeed + deltaV) {
                if(maxSpeed + deltaV < minSpeed) {
                    rb.velocity = rb.velocity.normalized * minSpeed;
                } else {
                    rb.velocity = rb.velocity.normalized * (maxSpeed + deltaV);
                }
            }
        } else {
            Vector3 currentVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            //if current speed is too high
            if(currentVelocity.magnitude > maxSpeed + deltaV) {
                //reduce to max speed and apply
                Vector3 clampedVelocity;
                if(maxSpeed + deltaV < minSpeed) {
                    clampedVelocity = currentVelocity.normalized * minSpeed;
                } else {
                    clampedVelocity = currentVelocity.normalized * (maxSpeed + deltaV);
                }
                rb.velocity = new Vector3(clampedVelocity.x, rb.velocity.y, clampedVelocity.z);
            }
        }
    }

    void Jump()
    {
        gravityScalar = airGravityScale;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        canJump = false;

        if(isSliding) {
            ExitSlide();
        }
    }

    void StartSlide()
    {
        //go to crouch height, change collider center, startslide
        isSliding = true;
        slideInitialY = transform.position.y - 0.5f;

        playerModel.localScale = new Vector3(playerModel.localScale.x, 0.5f, playerModel.localScale.z); //shrink player
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse); //push player down cause now they're floating a bit since they shrunk from top and bottom

        slideDirection = transform.forward;
        //print(slideDirection);

        //variable keeps track of the speed of the sliding player the last time they were on a slope.
        //If they were never on a slope, then tracks speed when they started slide.
        lastSlideSpeed = rb.velocity.magnitude;
    }

    void ExitSlide()
    {
        isSliding = false;
        playerModel.localScale = new Vector3(playerModel.localScale.x, 1f, playerModel.localScale.z);
    }

    void SlideMovement()
    {
        //USE SLIDE FORCE TO DETERMINE HOW MUCH PLAYER SPEEDS UP WHILE GOING DOWN SLOPE
        //USE SLIDE FRICTION TO DETERMINE HOW MUCH PLAYER SLOWS DOWN WHEN ON FLAT GROUND

        //calculate deltaY based on y level from when slide was started
        slideDeltaY = transform.position.y - slideInitialY;

        //if going down, add slide force
        if(rb.velocity.y < -0.1f && !OnSlope()) {
            rb.AddForce(slideDirection * slideForce, ForceMode.Force);
            //print("adding default slide force");

        } else if(rb.velocity.y < -0.1f && OnSlope()) { //acount for slope
            rb.AddForce(GetSlopeMoveVector(slideDirection, slideForce), ForceMode.Force);
            lastSlideSpeed = rb.velocity.magnitude;
            //print("adding slope slide force");

        } else if(rb.velocity.y > 0.1f && OnSlope()) {//if going up
            //decrease speed by how steep slope is
            rb.AddForce(GetSlopeMoveVector(-slideDirection, slideForce * slideDeltaY * 0.25f), ForceMode.Force);
            if(rb.velocity.y < 0.5f) {
                rb.velocity = Vector3.zero;
            }
            lastSlideSpeed = rb.velocity.magnitude;
            //print("slide up hill");

        } else if(!OnSlope()) {
            //only subtract velocity like this if slide speed is really high
            if(rb.velocity.magnitude > maxWalkSpeed + 3) {
                //keeps velocity/momentum of slide (stops player from slowing down)
                rb.velocity = rb.velocity.normalized * lastSlideSpeed;

                //decrease velocity/momentum over time (friction)
                lastSlideSpeed -= (slideFriction * Time.deltaTime);
                //print("slide friction");
            }
        }
    }

    void StartWallRun()
    {
        isWallRunning = true;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        //conserve momentum/speed

        //if sliding stop slide
        if(isSliding) {
            ExitSlide();
        }

        lastWallRunSpeed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
    }

    void StopWallRun()
    {
        isWallRunning = false;
        canWallRun = false;
    }

    void WallRunMovement()
    {
        //if wallRight, use wallRightNormal, else use wallLeftNormal
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
    
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if((transform.forward - wallForward).magnitude > (transform.forward - -wallForward).magnitude) {
            wallForward = -wallForward;
        }

        //have player slow down over time, like friction for slide, while wallrunning

        //keeps speed constant
        if(lastWallRunSpeed > 0.1f) {
            //rb.velocity = wallForward.normalized * lastWallRunSpeed;
            rb.velocity = new Vector3(wallForward.normalized.x * lastWallRunSpeed, rb.velocity.y, wallForward.normalized.z * lastWallRunSpeed);
        } else if(lastWallRunSpeed < 0.1f) {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
        
        //wallrun gravity
        //gravity proportional to speed? (if high speed, lower gravity?)
        rb.AddForce(-transform.up * wallRunGravity, ForceMode.Acceleration);

        //decrease velocity/momentum over time (friction)
        if(lastWallRunSpeed < wallFrictionSpeed && lastWallRunSpeed > 0.1f) {
            lastWallRunSpeed -= (1.5f * (wallFrictionSpeed-lastWallRunSpeed) * wallRunFriction * Time.deltaTime);
        } else if(lastWallRunSpeed > 0.1f) {
            lastWallRunSpeed -= (wallRunFriction * Time.deltaTime);
        }
        //print(lastWallRunSpeed);
    }

    void WallJump()
    {
        gravityScalar = airGravityScale;
        isWallRunning = false;

        //stop player from immedietely wallrunning again after jumping
        canWallRun = false;
        StartCoroutine(WallrunCooldown());

        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        //determine jump direction
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 wallJumpDirection = wallNormal + transform.up;
        rb.AddForce(wallJumpDirection.normalized * jumpForce, ForceMode.Impulse);
    }

    IEnumerator WallrunCooldown()
    {
        yield return new WaitForSeconds(wallrunCooldown);
        canWallRun = true;
    }

    void GroundCheck()
    {
        groundColliders = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, groundLayer);
        if(groundColliders.Length == 0 && isGrounded) {
            //in air
            isGrounded = false;
            gravityScalar = airGravityScale;
            //apply frictionless physics material
            playerCollider.material.dynamicFriction = 0;
            playerCollider.material.staticFriction = 0;
            playerCollider.material.frictionCombine = PhysicMaterialCombine.Minimum;
            StartCoroutine(CoyoteJump());
        } else if(groundColliders.Length != 0 && (!isGrounded || !canJump)) {
            //on ground
            isGrounded = true;
            canJump = true;
            gravityScalar = groundGravityScale;
            //apply default physics material
            playerCollider.material.dynamicFriction = 0.6f;
            playerCollider.material.staticFriction = 0.6f;
            playerCollider.material.frictionCombine = PhysicMaterialCombine.Average;

            //for energy transfer
            if(rb.velocity.magnitude < 0.3f) {
                lastYPosition = transform.position.y;
                //print("new y pos");
            }

            if(!canWallRun) {
                canWallRun = true;
            }
            if(isWallRunning) {
                isWallRunning = false;
            }
        }
    }

    bool AboveGround()
    {
        return !Physics.Raycast(transform.position, -transform.up, minJumpHeight, groundLayer);
    }

    void WallCheck()
    {
        wallRight = Physics.Raycast(transform.position, transform.right, out rightWallHit, wallCheckDistance, wallLayer);
        wallLeft = Physics.Raycast(transform.position, -transform.right, out leftWallHit, wallCheckDistance, wallLayer);
    }

    bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, 1.3f)) {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return (angle < maxSlopeAngle && angle != 0);
        }
        return false;
    }

    Vector3 GetSlopeMoveVector(Vector3 vector, float force)
    {
        return (Vector3.ProjectOnPlane(vector, slopeHit.normal).normalized * force);

        //maybe should work with sliding?
        //Vector3.ProjectOnPlane(rb.velocity, slopHit.normal).normalized * rb.velocity * magnitude; ?
    }

    IEnumerator CoyoteJump()
    {
        yield return new WaitForSeconds(coyoteJumpTime);
        canJump = false;
    }

    void ApplyGravity()
    {
        rb.AddForce(Physics.gravity * gravityScalar, ForceMode.Acceleration);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
