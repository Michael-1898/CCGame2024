using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BellSwitch : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject gasStation;
    [SerializeField] GameObject glow;
    
    [SerializeField] Animator fadeAnim;

    bool clicked = false;

    void Start()
    {
        glow.SetActive(false);
    }

    void OnMouseOver()
    {
        glow.SetActive(true);
    }

    void OnMouseDown()
    {
        if(!clicked) {
            clicked = true;
            StartCoroutine(GasStationSwitch());
        }
    }

    void OnMouseExit()
    {
        glow.SetActive(false);
    }

    IEnumerator GasStationSwitch()
    {
        //light fickering
        fadeAnim.SetTrigger("lightsOut");
        yield return new WaitForSeconds(0.5f);
        fadeAnim.SetTrigger("lightsOn");
        yield return new WaitForSeconds(1f);
        fadeAnim.SetTrigger("lightsOut");
        yield return new WaitForSeconds(0.2f);
        fadeAnim.SetTrigger("lightsOn");
        yield return new WaitForSeconds(0.2f);
        fadeAnim.SetTrigger("lightsOut");
        yield return new WaitForSeconds(0.25f);
        fadeAnim.SetTrigger("lightsOn");
        yield return new WaitForSeconds(0.2f);
        fadeAnim.SetTrigger("lightsOut");
        yield return new WaitForSeconds(0.07f);

        SaveLocation();

        SceneManager.LoadScene("DreamCity");
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