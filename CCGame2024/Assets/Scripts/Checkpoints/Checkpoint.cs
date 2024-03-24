using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    Vector3 position;
    bool dieOnFall;
    [SerializeField] Animator fadeAnim;
    [SerializeField] MikeMovement moveScript;

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
            StartCoroutine(Respawn());
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

        if (col.gameObject.tag == "Respawn" && dieOnFall == true)
        {
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        fadeAnim.SetTrigger("fadeOut");
        yield return new WaitForSeconds(1);
        moveScript.enabled = false;
        transform.position = position;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        fadeAnim.SetTrigger("fadeIn");
        yield return new WaitForSeconds(0.6f);
        moveScript.enabled = true;
    }
}
