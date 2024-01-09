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
        var fallOrder = new ArrayList();

        for (int b = 0; b < buildingsChunks.Length; b++)
        {
            while (true)
            {
                Random r = new Random();
                int rInt = r.Next(0, buildingsChunks.Length);

                if (!fallOrder.Contains(rInt))
                {
                    fallOrder.Add(rInt);
                    break;
                }
            }
        }

        i = 0;
        while (i < buildingsChunks.Length)
        {
            yield return new WaitForSeconds(4);
            int a = (int) fallOrder[i];
            buildingsChunks[a].GetComponent<Rigidbody>().isKinematic = false;
            i++;
        }

    }
}
