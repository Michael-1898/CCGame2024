using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bridgeFall : MonoBehaviour
{
    public GameObject[] buildingsChunks;

    private float time;
    private int i;
    // Start is called before the first frame update
    void Start()
    {
        buildingsChunks = GameObject.FindGameObjectsWithTag("BridgePieces");

        StartCoroutine(fall());
        

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator fall()
    {
        i = 0;
        while (i < buildingsChunks.Length)
        {
            yield return new WaitForSeconds(4);
            buildingsChunks[i].GetComponent<Rigidbody>().isKinematic = false;
            i++;
        }

    }
}
