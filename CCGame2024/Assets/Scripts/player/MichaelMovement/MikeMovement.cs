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
-player gets friction applied when they are touching ground
-velocity/momentum is preserved when entering slide, p=mv
-player presses slide key, start sliding, cotinue sliding until they either slow down enough, jump, or press the slide key again

Gravity Notes:
-increase gravity so that the player stays grounded, but set the gravity back to normal when they jump or fall
-no gravity scale with rigidbody, so need to do manual gravity
*/

public class MikeMovement : MonoBehaviour
{
    //base movement
    float verticalInput;
    float horizontalInput;
    [SerializeField] float moveForce;
    [SerializeField] Rigidbody rb;
    Vector3 movementVector;
    [SerializeField] float maxWalkSpeed;
    [SerializeField] float minWalkSpeed;
    [SerializeField] float maxSlopeAngle;
    RaycastHit slopeHit;

    //jump
    [SerializeField] float jumpForce;
    bool isGrounded;
    Collider[] groundColliders;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;
    //coyote jump
    bool canJump;
    [SerializeField] float coyoteJumpTime;

    //slide
    bool isSliding = false;
    [SerializeField] float slideFriction;
    [SerializeField] Camera playerCam;
    Vector3 slideDirection;
    float slideDuration;
    [SerializeField] float maxSlideSpeed;
    Transform playerModel;

    //energy conversion
    float lastYPosition;
    float currentYPosition;
    float deltaV;

    //gravity
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
        print(OnSlope());

        if(!OnSlope()) {
            ApplyGravity();
        }

        currentYPosition = transform.position.y;

        // print(lastYPosition);
        // print(currentYPosition);

        //ground check switches value of isGrounded and canJump variable
        GroundCheck();

        if(rb.velocity.magnitude < 0.3f) {
            lastYPosition = transform.position.y;
            //print("new y pos");
        }

        ApplyDrag();

        if(isGrounded && rb.velocity.magnitude > 0.5f) {
            EnergyConservation();
        }
        //calculate the change in y pos
        //calculate the change in potential energy
        //change in potential energy = change in kinetic energy
        //calculate change in velocity

        //get movement input
        if(!isSliding) {
            verticalInput = Input.GetAxis("Vertical");
            horizontalInput = Input.GetAxis("Horizontal");
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

        //print(maxWalkSpeed + deltaV);
        //print(rb.velocity.magnitude);
        
        ClampSpeed();
    }

    void FixedUpdate()
    {
        if(!isSliding && !OnSlope()) { //default movement
            rb.AddForce(movementVector, ForceMode.Force);
        } else if(!isSliding && OnSlope()) { //slope movement
            rb.AddForce(GetSlopeMoveVector(), ForceMode.Force);
            
            if(rb.velocity.y > 0) { //downward force while on slope to keep player on it
                rb.AddForce(-slopeHit.normal * 80f, ForceMode.Force);
            }
        } else if (isSliding) { //if sliding
            SlideMovement();
            if(rb.velocity.magnitude > maxSlideSpeed) {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSlideSpeed + deltaV);
            }
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

    void ClampSpeed()
    {
        //limiting speed on slope
        if(OnSlope()) {
            if(rb.velocity.magnitude > maxWalkSpeed + deltaV) {
                if(maxWalkSpeed + deltaV < minWalkSpeed) {
                    rb.velocity = rb.velocity.normalized * minWalkSpeed;
                } else {
                    rb.velocity = rb.velocity.normalized * (maxWalkSpeed + deltaV);
                }
            }
        } else {
            Vector3 currentVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            //if current speed is too high
            if(currentVelocity.magnitude > maxWalkSpeed + deltaV) {
                //reduce to max speed and apply
                Vector3 clampedVelocity;
                if(maxWalkSpeed + deltaV < minWalkSpeed) {
                    clampedVelocity = currentVelocity.normalized * minWalkSpeed;    
                } else {
                    clampedVelocity = currentVelocity.normalized * (maxWalkSpeed + deltaV);
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
        // transform.GetChild(0).gameObject.GetComponent<CapsuleCollider>().height = 1f;
        // transform.GetChild(0).gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0, -0.5f, 0);
        playerModel.localScale = new Vector3(playerModel.localScale.x, 0.5f, playerModel.localScale.z); //shrink player
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse); //push player down cause now they're floating a bit since they shrunk from top and bottom
        //playerCam.transform.position = new Vector3(playerCam.transform.position.x, playerCam.transform.position.y-0.75f, playerCam.transform.position.z);

        slideDirection = transform.forward;
        //print(slideDirection);
    }

    void ExitSlide()
    {
        isSliding = false;
        // transform.GetChild(0).gameObject.GetComponent<CapsuleCollider>().height = 2f;
        // transform.GetChild(0).gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0, 0, 0);
        playerModel.localScale = new Vector3(playerModel.localScale.x, 1f, playerModel.localScale.z);
        //playerCam.transform.position = new Vector3(playerCam.transform.position.x, playerCam.transform.position.y + 0.75f, playerCam.transform.position.z);
    }

    void SlideMovement()
    {
        float localSlideFriction = slideFriction;

        //slide duration
        // slideDuration += Time.deltaTime;
        // if(slideDuration > 0.75f) {
        //     localSlideFriction *= (10+slideDuration);
        // }

        //apply friction
        rb.AddForce(-slideDirection * slideFriction, ForceMode.Acceleration);
    }

    void GroundCheck()
    {
        groundColliders = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, groundLayer);
        if(groundColliders.Length == 0 && isGrounded) {
            //in air
            isGrounded = false;
            gravityScalar = airGravityScale;
            StartCoroutine(CoyoteJump());
        } else if(groundColliders.Length != 0 && (!isGrounded || !canJump)) {
            //on ground
            isGrounded = true;
            canJump = true;
            gravityScalar = groundGravityScale;
            if(rb.velocity.magnitude < 0.3f) {
                lastYPosition = transform.position.y;
                //print("new y pos");
            }
        }
    }

    bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, 1.3f)) {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return (angle < maxSlopeAngle && angle != 0);
        }
        return false;
    }

    Vector3 GetSlopeMoveVector()
    {
        return (Vector3.ProjectOnPlane(movementVector, slopeHit.normal).normalized * moveForce);
    }

    IEnumerator CoyoteJump()
    {
        yield return new WaitForSeconds(coyoteJumpTime);
        canJump = false;
    }

    void ApplyDrag()
    {
        if(isGrounded && Mathf.Abs(horizontalInput) < 0.5f && Mathf.Abs(verticalInput) < 0.5f && rb.velocity.magnitude > 1) {
            rb.velocity = new Vector3(rb.velocity.x * 0.2f, rb.velocity.y, rb.velocity.z * 0.2f);
        }
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
