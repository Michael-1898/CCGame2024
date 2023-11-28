using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newPickup : MonoBehaviour
{
    [SerializeField] private LayerMask pickupMask;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform pickupTarget;

    [SerializeField] private float pickupRange;
    private Rigidbody currentObject;
    private Collider currentCol;

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {

            if(currentObject)
            {
                currentObject.useGravity = true;
                Physics.IgnoreCollision(GetComponent<Collider>(), currentCol, false);
                currentCol = null;
                currentObject = null;
                return;
            }

            Ray CameraRay = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if(Physics.Raycast(CameraRay, out RaycastHit HitInfo, pickupRange, pickupMask))
            {
                currentObject = HitInfo.rigidbody;
                currentObject.useGravity = false;
                Physics.IgnoreCollision(GetComponent<Collider>(), HitInfo.collider);
                currentCol = HitInfo.collider;
            }
        }
    }

    void FixedUpdate()
    {
        if (currentObject)
        {
            Vector3 directionToPoint = pickupTarget.position - currentObject.position;
            float distanceToPoint = directionToPoint.magnitude;

            currentObject.velocity = directionToPoint * 12f * distanceToPoint;

        }
    }
}
