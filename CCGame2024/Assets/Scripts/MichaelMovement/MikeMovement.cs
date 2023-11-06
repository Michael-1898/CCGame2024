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

        //ground check switches value of isGrounded variable
        GroundCheck();

        //get jump input
        if(Input.GetKeyDown("space")) {
            Jump();
        }
    }

    void FixedUpdate()
    {
        currentVelocity *= moveSpeed;
        currentVelocity *= Time.deltaTime;
        currentVelocity.y = rb.velocity.y;
        rb.velocity = currentVelocity;
    }

    void Jump()
    {
        print("jump");
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void GroundCheck()
    {
        
    }
}
