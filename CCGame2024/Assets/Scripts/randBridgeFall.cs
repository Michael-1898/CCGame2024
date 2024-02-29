using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randBridgeFall : MonoBehaviour
{
   public GameObject[] buildingsChunks;

    private float time;
    private int i;
    private int e;
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
        e = 0;
        while (i < buildingsChunks.Length)
        {
            yield return new WaitForSeconds(1);
            for (int c = 0; c < 5; c++)
            {
                int d = (int) fallOrder[e];
                MeshRenderer[] mR = buildingsChunks[d].GetComponentsInChildren<MeshRenderer>();
                for (int f = 0; f < 4; f++)
                {
                    mR[f].material.color = new Color(255, 0, 0);
                }
                e++;
            }
            yield return new WaitForSeconds(1);
            for (int b = 0; b < 5; b++)
            {
                int a = (int) fallOrder[i];
                buildingsChunks[a].GetComponent<Rigidbody>().isKinematic = false;
                i++;
            }
        }

    }
}
