using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupClass : MonoBehaviour
{
    [SerializeField] private LayerMask PickupLayer;
    [SerializeField] private Camera PlayerCamera;
    [SerializeField] private float PickupRange;
    [SerializeField] private float ThrowingForce;
    [SerializeField] Transform Hand;


    private Rigidbody CurrentObjectRigidbody;
    public GameObject CurrentGameObject;
    private Collider CurrentObjectCollider;

    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Ray PickupRay = new Ray(PlayerCamera.transform.position, PlayerCamera.transform.forward);

        if(Input.GetKeyDown(KeyCode.E)) 
        {
            
            if (Physics.Raycast(PickupRay, out RaycastHit hitInfo, PickupRange, PickupLayer)) 
            {
                if (CurrentObjectRigidbody) 
                {

                    CurrentObjectRigidbody.isKinematic = false;
                    CurrentObjectCollider.enabled = true;

                    CurrentObjectRigidbody = hitInfo.rigidbody;
                    CurrentObjectCollider = hitInfo.collider;
                    CurrentGameObject = hitInfo.collider.gameObject;


                    CurrentObjectRigidbody.isKinematic = true;
                    CurrentObjectCollider.enabled = false;

                    
                }
                else 
                {
                    CurrentObjectRigidbody = hitInfo.rigidbody;
                    CurrentObjectCollider = hitInfo.collider;

                    CurrentObjectRigidbody.isKinematic = true;
                    CurrentObjectCollider.enabled = false;
                    CurrentGameObject = hitInfo.collider.gameObject;


                }
                return;
            }
            

            if (CurrentObjectRigidbody) 
            {
                CurrentObjectRigidbody.isKinematic = false;
                CurrentObjectCollider.enabled = true;

                CurrentObjectRigidbody = null;
                CurrentObjectCollider = null;

            }


        }

        if (Input.GetKeyDown(KeyCode.Q))
        {

            if (CurrentObjectRigidbody) 
            {
                CurrentObjectRigidbody.isKinematic = false;
                
                CurrentObjectCollider.enabled = true;

                float random = Random.Range(-5f,5f);
                CurrentObjectRigidbody.AddForce(PlayerCamera.transform.forward * ThrowingForce, ForceMode.Impulse);
                CurrentObjectRigidbody.AddTorque(new Vector3 (random, random, random));



                CurrentObjectRigidbody = null;
                CurrentObjectCollider = null;
            }
        }

        if(CurrentObjectRigidbody) 
        {
            CurrentObjectRigidbody.position = Hand.position;
            CurrentObjectRigidbody.rotation = Hand.rotation;
        }

    }

}
