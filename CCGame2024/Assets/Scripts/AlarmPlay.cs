using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmPlay : MonoBehaviour
{

    [SerializeField] private AudioSource audio;

    private void OnTriggerEnter (Collider col)
    {
        if (col.CompareTag("Player"))
        {
            audio.Play();
        }
    }
}
