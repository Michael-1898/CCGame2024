﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openDoor : MonoBehaviour
{

    [SerializeField] Animator anim;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Key")
        {
            anim.SetTrigger("openDoor");
        }

        if (col.gameObject.tag == "KeyKeep")
        {
            anim.SetTrigger("openDoor");
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "KeyKeep")
        {
            anim.SetTrigger("closeDoor");
        }
    }

}
