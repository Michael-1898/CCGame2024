using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeCheckpoint : MonoBehaviour
{

    public static Vector3 checkpointPos = new Vector3((float)2.5, (float)4.38, (float)11.2);
    public static Vector3 checkpointRot;

    private void OnTriggerEnter(Collider other)
    {
        checkpointPos = other.transform.position;
        checkpointRot = other.transform.eulerAngles;
    }
    // Start is called before the first frame update
    void Start()
    {
        //checkpointPos = new Vector3((float)2.5,(float)4.38,(float)11.2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
