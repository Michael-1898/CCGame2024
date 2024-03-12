using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    [SerializeField] float deltaV;

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Player")) {
            //col.gameObject.GetComponent<MikeMovement>().SendMessage("SpeedBoost", deltaV);
            //SpeedBoost(deltaV);
            print(col.gameObject);
        }
    }
}
