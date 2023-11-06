using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MikeMovement : MonoBehaviour
{
    //base movement
    private float verticalInput;
    private float horizontalInput;
    [SerializeField] float moveSpeed;
    [SerializeField] Rigidbody rb;
    private Vector3 currentVelocity;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        currentVelocity = (transform.forward * verticalInput) + (transform.right * horizontalInput);
    }

    void FixedUpdate()
    {
        currentVelocity *= moveSpeed;
        currentVelocity *= Time.deltaTime;
        currentVelocity.y = rb.velocity.y;
        rb.velocity = currentVelocity;
    }
}
