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
        transform.Rotate(0f, PlayerPrefs.GetFloat("yPlayerAngle"), 0f, Space.Self);
        Camera.transform.Rotate(PlayerPrefs.GetFloat("xCameraAngle"), 0f, 0f, Space.Self);
    }
}
