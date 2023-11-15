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
        
        print("collision");
        if (col.gameObject.tag == "Respawn" && dieOnFall == true)
        {
            print("collision2");
            transform.position = position;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        print("trigger");
        if (col.gameObject.tag == "Checkpoint")
        {
            print("trigger2");
            position = col.transform.position;
            dieOnFall = true;
        }
    }
}
