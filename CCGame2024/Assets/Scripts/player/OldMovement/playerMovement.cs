using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float moveSmoothTime;
    public float gravityStrength;
    public float jumpStrength;
    public float walkSpeed;
    public float runSpeed;

    private CharacterController Controller;
    private Vector3 currentMoveVelocity;
    private Vector3 moveDampVelocity;
        
    private Vector3 currentForceVelocity;

    // Start is called before the first frame update
    void Start()
    {
        Controller = GetComponent<CharacterController>();
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerInput = new Vector3 {

            x = Input.GetAxisRaw("Horizontal"),
            y = 0f,
            z = Input.GetAxisRaw("Vertical") 
        };

        if (playerInput.magnitude > 1f) {
            playerInput.Normalize();
        }

        Vector3 moveVector = transform.TransformDirection(playerInput);
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        //float currentSpeed = walkSpeed;

        currentMoveVelocity = Vector3.SmoothDamp(currentMoveVelocity, moveVector * currentSpeed, ref moveDampVelocity, moveSmoothTime);

        Controller.Move(currentMoveVelocity * Time.deltaTime);

        //jumping
        Ray groundCheckRay = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(groundCheckRay, 1.25f)) {
            
            currentForceVelocity.y = -2f;

            if (Input.GetKey(KeyCode.Space)) {
                currentForceVelocity.y = jumpStrength;
            }
        }
        else
        {
            currentForceVelocity.y -= gravityStrength * Time.deltaTime;
        }

        Controller.Move(currentForceVelocity * Time.deltaTime);
    }
}
