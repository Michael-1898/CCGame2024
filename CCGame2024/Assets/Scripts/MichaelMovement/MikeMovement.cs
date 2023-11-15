using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    float velocityScalar;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        lastYPosition = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        currentYPosition = transform.position.y;
        currentVelocity = rb.velocity;

        EnergyConservation();
        //calculate the change in y pos
        //calculate the change in potential energy
        //change in potential energy = change in kinetic energy
        //calculate change in velocity

        //get movement input
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        currentVelocity = (transform.forward * verticalInput) + (transform.right * horizontalInput);
        currentVelocity = currentVelocity * moveSpeed;

        //ground check switches value of isGrounded and canJump variable
        GroundCheck();

        //get jump input
        if(Input.GetKeyDown("space") && isGrounded && canJump) {
            Jump();
        }

        
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector3(currentVelocity.x, rb.velocity.y, currentVelocity.z);
    }

    void LateUpdate()
    {
        lastYPosition = currentYPosition;
    }

    void EnergyConservation()
    {
        float deltaY = currentYPosition - lastYPosition;
        // if(Mathf.Approximately(deltaY, 0)) {
        //     deltaY = 0;
        // }
        float deltaU = rb.mass * 9.8f * deltaY;
        
        //k = (1/2)m(v^2)
        //v = sqrt(2k/m)
        float deltaV = Mathf.Sqrt((2 * Mathf.Abs(deltaU)) / rb.mass);
        if(deltaU > 0) {
            deltaV *= -1;
        }
        
        //oldV * vScalar = newV
        //vScalar = newV/oldV
        velocityScalar = (rb.velocity.magnitude + deltaV) / rb.velocity.magnitude;
    }

    void Jump()
    {
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
        }
    }

    IEnumerator CoyoteJump()
    {
        yield return new WaitForSeconds(coyoteJumpTime);
        canJump = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
