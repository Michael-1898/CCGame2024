using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Slide Notes:
-locks movement to direction the player was moving when they entered the slide
-player can still look around, but moves in same direction
-player can accelerate to even higher speeds than when running
-player gets friction applied when they are touching ground
-velocity/momentum is preserved when entering slide, p=mv

General Notes:
-increase gravity so that the player stays grounded, but set the gravity back to normal when they jump or fall
-no gravity scale with rigidbody, so need to do manual gravity
*/

public class MikeMovement : MonoBehaviour
{
    //base movement
    float verticalInput;
    float horizontalInput;
    [SerializeField] float moveSpeed;
    [SerializeField] Rigidbody rb;
    Vector3 currentVelocity;

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

    //energy conversion
    float lastYPosition;
    float currentYPosition;
    float velocityScalar = 1;

    //gravity
    [SerializeField] float groundGravityScale;
    [SerializeField] float airGravityScale;
    float gravityScalar;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        currentYPosition = transform.position.y;
        currentVelocity = rb.velocity;

        // print(lastYPosition);
        // print(currentYPosition);

        //ground check switches value of isGrounded and canJump variable
        GroundCheck();

        if(rb.velocity.magnitude < 0.05) {
            lastYPosition = transform.position.y;
            //print("new y pos");
        }

        if(isGrounded && rb.velocity.magnitude > 0.05) {
            EnergyConservation();
        }
        //calculate the change in y pos
        //calculate the change in potential energy
        //change in potential energy = change in kinetic energy
        //calculate change in velocity

        //get movement input
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        currentVelocity = (transform.forward * verticalInput) + (transform.right * horizontalInput);
        currentVelocity = currentVelocity * moveSpeed;

        //get jump input
        if(Input.GetKeyDown("space") && isGrounded && canJump) {
            Jump();
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector3(currentVelocity.x * velocityScalar, rb.velocity.y, currentVelocity.z * velocityScalar);
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
        float deltaV = Mathf.Sqrt((2 * Mathf.Abs(deltaU)) / rb.mass);
        if(deltaU > 0) {
            deltaV *= -1;
        }
        //print(deltaV);
        //print(rb.velocity.magnitude);
        
        //oldV * vScalar = newV
        //vScalar = newV/oldV
        velocityScalar = (rb.velocity.magnitude + deltaV) / rb.velocity.magnitude;
        //clamp velocity scalar
        velocityScalar = Mathf.Clamp(velocityScalar, 0.5f, 2f);
        //print("scalar" + velocityScalar);
    }

    void Jump()
    {
        gravityScalar = airGravityScale;
        rb.velocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        canJump = false;
    }

    void GroundCheck()
    {
        groundColliders = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, groundLayer);
        if(groundColliders.Length == 0 && isGrounded) {
            //in air
            isGrounded = false;
            StartCoroutine(CoyoteJump());
        } else if(groundColliders.Length != 0 && (!isGrounded || !canJump)) {
            //on ground
            isGrounded = true;
            canJump = true;
            gravityScalar = groundGravityScale;
            lastYPosition = transform.position.y;
            //print("new y pos");
        }
    }

    IEnumerator CoyoteJump()
    {
        yield return new WaitForSeconds(coyoteJumpTime);
        canJump = false;
    }

    void ApplyGravity()
    {
        rb.AddForce(-transform.up * 9.81f * gravityScalar, ForceMode.Acceleration);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
