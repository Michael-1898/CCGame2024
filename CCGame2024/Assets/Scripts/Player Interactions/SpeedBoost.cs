using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    [SerializeField] float deltaV;
    MikeMovement moveScript;

    void Start()
    {
        moveScript = GameObject.FindGameObjectWithTag("Player").GetComponent<MikeMovement>();
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Player")) {
            moveScript.SpeedBoost(deltaV);
            //print(col.gameObject);
        }
    }
}
