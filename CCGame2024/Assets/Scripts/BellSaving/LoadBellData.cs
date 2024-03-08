using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadBellData : MonoBehaviour
{
    [SerializeField] GameObject gasStation;
    [SerializeField] GameObject Camera;


    // Start is called before the first frame update
    void Start()
    {
        LoadLocation();
    }

    void LoadLocation()
    {
        transform.position = new Vector3(gasStation.transform.position.x - PlayerPrefs.GetFloat("xDiff"), gasStation.transform.position.y - PlayerPrefs.GetFloat("yDiff"), gasStation.transform.position.z - PlayerPrefs.GetFloat("zDiff"));
        transform.RotateAround(gasStation.transform.position, Vector3.up, 90);

        GetComponent<MikeLook>().SetXRotation(PlayerPrefs.GetFloat("xLook"));
        GetComponent<MikeLook>().SetYRotation(PlayerPrefs.GetFloat("yLook") + 90);
        //print("Pl: " + PlayerPrefs.GetFloat("yLook"));
        //print("C1: " + PlayerPrefs.GetFloat("xLook"));
    }
}
