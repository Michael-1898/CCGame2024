using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyTransfer : MonoBehaviour
{
    float lastYPosition;
    float currentYPosition;
    [SerializeField] Rigidbody rb;
    float velocityScalar;
    Vector3 currentVelocity;

    void Awake()
    {
        lastYPosition = transform.position.y;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentYPosition = transform.position.y;
        currentVelocity = rb.velocity;

        EnergyConservation();
        print(velocityScalar);
        //calculate the change in y pos
        //calculate the change in potential energy
        //change in potential energy = change in kinetic energy
        //calculate change in velocity
    }

    void FixedUpdate()
    {
        rb.velocity = currentVelocity * velocityScalar;
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
}
