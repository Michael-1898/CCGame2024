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

    //energy conversion
    float lastYPosition;
    float currentYPosition;
    float velocityScalar = 1;
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
    }

    // Update is called once per frame
    void Update()
    {
        ApplyGravity();

        currentYPosition = transform.position.y;

        // print(lastYPosition);
        // print(currentYPosition);

        //ground check switches value of isGrounded and canJump variable
        GroundCheck();

        if(rb.velocity.magnitude < 0.3f) {
            lastYPosition = transform.position.y;
            //print("new y pos");
        }

        if(isGrounded && rb.velocity.magnitude > 0.05f) {
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
        
        //clamp speed
        if(rb.velocity.magnitude > maxWalkSpeed && canJump) {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxWalkSpeed + deltaV);
            print("yerp");
        } else if(maxWalkSpeed + deltaV <= 4 && canJump) {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, 4);
            print("nerp");
        }
    }

    void FixedUpdate()
    {
        if(!isSliding) {
            //force based movement, use force to accelerate, then clamp speed
            rb.AddForce(movementVector, ForceMode.Acceleration);
            
            //if not pressing buttons, apply a friction force to slow player down faster
            if(Mathf.Abs(horizontalInput) < 0.5f && Mathf.Abs(verticalInput) < 0.5f && rb.velocity.magnitude > 1 && canJump) {
                rb.velocity = rb.velocity * 0.2f;
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
        //deltaY /= 4;

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
        transform.GetChild(0).gameObject.GetComponent<CapsuleCollider>().height = 1f;
        transform.GetChild(0).gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0, -0.5f, 0);
        playerCam.transform.position = new Vector3(playerCam.transform.position.x, playerCam.transform.position.y-0.75f, playerCam.transform.position.z);

        slideDirection = transform.forward;
        //print(slideDirection);
    }

    void ExitSlide()
    {
        isSliding = false;
        transform.GetChild(0).gameObject.GetComponent<CapsuleCollider>().height = 2f;
        transform.GetChild(0).gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0, 0, 0);
        playerCam.transform.position = new Vector3(playerCam.transform.position.x, playerCam.transform.position.y + 0.75f, playerCam.transform.position.z);
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
        rb.AddForce(-slideDirection /* * localSlideFriction*/, ForceMode.Acceleration);
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
