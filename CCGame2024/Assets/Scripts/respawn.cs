using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class respawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = BridgeCheckpoint.checkpointPos;
        transform.eulerAngles = BridgeCheckpoint.checkpointRot;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {

        if (col.tag == "Respawn")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
    }
}
