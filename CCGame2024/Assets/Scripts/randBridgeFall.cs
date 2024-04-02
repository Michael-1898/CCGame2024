using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randBridgeFall : MonoBehaviour
{
   public GameObject[] buildingsChunks;

    private float time;
    private int i;
    private int e;
    [SerializeField] private int R;
    [SerializeField] private int G;
    [SerializeField] private int B;

    float initialY;

    // Start is called before the first frame update
    void Start()
    {

        buildingsChunks = GameObject.FindGameObjectsWithTag("BridgePieces");
        initialY = buildingsChunks[0].transform.position.y;

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
            yield return new WaitForSeconds(1f);
            for (int c = 0; c < 5; c++)
            {
                int d = (int) fallOrder[e];
                MeshRenderer[] mR = buildingsChunks[d].GetComponentsInChildren<MeshRenderer>();
                for (int f = 0; f < 4; f++)
                {
                    mR[f].material.color = new Color(R, G, B);
                }

                ParticleSystem[] ps = buildingsChunks[d].GetComponentsInChildren<ParticleSystem>();
                if(ps.Length != 0) {
                    var main = ps[0].main;
                    main.startColor = new Color(R, G, B);
                    var emission = ps[0].emission;
                    emission.rate = 200;
                }

                e++;
            }
            yield return new WaitForSeconds(2f);
            for (int b = 0; b < 5; b++)
            {
                int a = (int) fallOrder[i];
                buildingsChunks[a].GetComponent<Rigidbody>().isKinematic = false;
                i++;
            }

            //reset chunks/falling
            if(i == buildingsChunks.Length) {
                yield return new WaitForSeconds(1f);
                for(int v = 0; v < buildingsChunks.Length; v++) {
                    ParticleSystem[] ps = buildingsChunks[v].GetComponentsInChildren<ParticleSystem>();
                    if(ps.Length != 0) {
                        var main = ps[0].main;
                        main.startColor = new Color(255, 255, 255);
                        var emission = ps[0].emission;
                        emission.rate = 50;
                    }
                    MeshRenderer[] mR = buildingsChunks[v].GetComponentsInChildren<MeshRenderer>();
                    for (int f = 0; f < 4; f++)
                    {
                        mR[f].material.color = new Color(0, 0, 0);
                    }
                }
                yield return new WaitForSeconds(4.5f);
                for(int v = 0; v < buildingsChunks.Length; v++) {
                    buildingsChunks[v].GetComponent<Rigidbody>().isKinematic = true;
                    buildingsChunks[v].transform.position = new Vector3(buildingsChunks[v].transform.position.x, initialY, buildingsChunks[v].transform.position.z);
                }
                yield return new WaitForSeconds(1.5f);
                i = 0;
                e = 0;
            }
        }

    }
}
