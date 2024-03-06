using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        var mouse = Mouse.current;
        transform.position = new Vector3(gasStation.transform.position.x - PlayerPrefs.GetFloat("xDiff"), gasStation.transform.position.y - PlayerPrefs.GetFloat("yDiff"), gasStation.transform.position.z - PlayerPrefs.GetFloat("zDiff"));
        transform.RotateAround(gasStation.transform.position, Vector3.up, 90);
        //transform.Rotate(0f, PlayerPrefs.GetFloat("yPlayerAngle"), 0f, Space.World);
        //Quaternion rotation = 
        //GetComponent<Rigidbody>().MoveRotation(rotation);
        //transform.rotation = Quaternion.Euler(0, PlayerPrefs.GetFloat("yPlayerAngle"), 0);
        //print("y rotation:" + transform.rotation.y);
        //transform.rotation = Quaternion.Euler(0, PlayerPrefs.GetFloat("yPlayerAngle"), 0);
        //Camera.transform.rotation = Quaternion.Euler(PlayerPrefs.GetFloat("xCameraAngle"), 0f, 0f);

        Camera.transform.localEulerAngles = new Vector3(PlayerPrefs.GetFloat("xLook"), 0f, 0f);
        transform.eulerAngles = new Vector3(0f, PlayerPrefs.GetFloat("yLook"), 0f);
        print("Pl: " + PlayerPrefs.GetFloat("yLook"));
        print("C1: " + PlayerPrefs.GetFloat("xLook"));
    }
}
