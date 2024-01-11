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

    IEnumerator fall()
    {
        var fallOrder = new ArrayList();

        for (int b = 0; b < buildingsChunks.Length; b++)
        {
            while (true)
            {
                int max = buildingsChunks.Length;
                int rInt = (int) Random.Range(0, max);

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
            yield return new WaitForSeconds(2);
            int a = (int) fallOrder[i];
            buildingsChunks[a].GetComponent<Rigidbody>().isKinematic = false;
            i++;
        }

    }
}
