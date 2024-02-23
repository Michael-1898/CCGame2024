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
        PlayerPrefs.SetFloat("xDiff", gasStation.transform.position.x - player.transform.position.x);
        PlayerPrefs.SetFloat("yDiff", gasStation.transform.position.y - player.transform.position.y);
        PlayerPrefs.SetFloat("zDiff", gasStation.transform.position.z - player.transform.position.z);
    }
}   

