using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BellSwitch : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject gasStation;
    

    void OnMouseDown()
    {
        SaveLocation();

        SceneManager.LoadScene("DreamCity");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    void SaveLocation()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        
        PlayerPrefs.SetFloat("xDiff", gasStation.transform.position.x - player.transform.position.x);
        PlayerPrefs.SetFloat("yDiff", gasStation.transform.position.y - player.transform.position.y);
        PlayerPrefs.SetFloat("zDiff", gasStation.transform.position.z - player.transform.position.z);
       
        PlayerPrefs.SetFloat("xLook", player.GetComponent<MikeLook>().GetXRotation());
        PlayerPrefs.SetFloat("yLook", player.GetComponent<MikeLook>().GetYRotation());
    }
}