using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] Light headRight;
    [SerializeField] Light headLeft;
    [SerializeField] int carSpeed;
    bool carGo;
    // Start is called before the first frame update
    void Start()
    {
        headRight.enabled = false;
        headLeft.enabled = false;
        carGo = false;
        StartCoroutine(carStart());
    }

    // Update is called once per frame
    void Update()
    {
        print(carGo);
        if(carGo)
        {
            headRight.enabled = true;
            headLeft.enabled = true;
            transform.Translate(Vector3.right * Time.deltaTime * carSpeed);
        }
    }

    IEnumerator carStart()
    {
        yield return new WaitForSeconds(1);
        carGo = true;
    }

    
}
