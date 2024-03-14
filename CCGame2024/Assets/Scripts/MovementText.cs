using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementText : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody playerRB;
    bool triggered = false;

    // Update is called once per frame
    void Update()
    {
        if(playerRB.velocity.magnitude > 1 && !triggered) {
            anim.SetTrigger("fadeOut");
            triggered = true;
        }
    }
}
