using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MikeMovement : MonoBehaviour
{
    //base movement
    private float verticalInput;
    private float horizontalInput;
    [SerializeField] float moveSpeed;
    [SerializeField] Rigidbody2D rb;
    private Vector3 currentVelocity;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        verticalInput = Input.GetAxis("vertical");
        horizontalInput = Input.GetAxis("horizontal");

        currentVelocity = (transform.forward * verticalInput) + (transform.right * horizontalInput);
    }

    void FixedUpdate()
    {
        rb.velocity = currentVelocity;
    }
}
