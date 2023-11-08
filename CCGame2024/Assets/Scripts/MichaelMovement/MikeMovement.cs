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
        currentVelocity.y = rb.velocity.y;

        //ground check switches value of isGrounded variable
        GroundCheck();

        //get jump input
        if(Input.GetKeyDown("space") && isGrounded) {
            Jump();
        }

        
    }

    void FixedUpdate()
    {
        rb.velocity = currentVelocity;
    }

    void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void GroundCheck()
    {
        groundColliders = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, groundLayer);
        if(groundColliders.Length == 0 && isGrounded) {
            isGrounded = false;
        } else if(groundColliders.Length != 0 && !isGrounded) {
            isGrounded = true;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
