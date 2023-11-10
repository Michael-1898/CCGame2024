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

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
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
