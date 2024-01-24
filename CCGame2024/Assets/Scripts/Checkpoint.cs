using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    Vector3 position;
    bool dieOnFall;

    // Start is called before the first frame update
    void Start()
    {
        dieOnFall = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision col)
    {
        
        if (col.gameObject.tag == "Respawn" && dieOnFall == true)
        {
            transform.position = position;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Checkpoint")
        {
            position = col.transform.position;
            dieOnFall = true;
        }

        if (col.gameObject.tag == "CheckpointReset")
        {
            dieOnFall = false;
        }
    }
}
