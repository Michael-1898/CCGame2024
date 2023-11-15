using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarMovement : MonoBehaviour
{
    [SerializeField] float slowTime;
    [SerializeField] Light headRight;
    [SerializeField] Light headLeft;
    [SerializeField] Light moonLight;
    [SerializeField] int carSpeed;
    [SerializeField] Animator carMove;
    AnimatorStateInfo animStateInfo;
    GameObject dodgeText;
    GameObject player;
    Animator playerAnim;
    public float Ntime;

    bool carGo;
    bool carStart;
    bool isSlowed;
    // Start is called before the first frame update
    void Start()
    {

        Time.timeScale = 1.0f;
        dodgeText = GameObject.Find("Dodge Text");
        player = GameObject.Find("Player");
        playerAnim = player.GetComponent<Animator>();
        dodgeText.GetComponent<Text>().enabled = false;
        headRight.enabled = false;
        headLeft.enabled = false;
        moonLight.enabled = false;
        carGo = false;
        carStart = false;
        isSlowed = false;
        StartCoroutine(carDrive());
    }

    // Update is called once per frame
    void Update()
    {
        //print(carGo);
        if(carGo)
        {
            //print(carMove.GetCurrentAnimatorStateInfo(0).normalizedTime);
            headRight.enabled = true;
            headLeft.enabled = true;
            carMove.SetTrigger("carDrive");
            if(carStart)
            {
                StartCoroutine(carSlow());
                carStart = false;
            }

            if(Input.GetKeyDown("q") && isSlowed)
            {
                Time.timeScale = 1.0f;
                dodgeText.GetComponent<Text>().enabled = false;
                playerDodge();
                moonLight.enabled = true;
                isSlowed = false;
            }
        
        }

    }

    void playerDodge()
    {
        playerAnim.SetTrigger("playerRoll");
    }

    IEnumerator carDrive()
    {
        yield return new WaitForSeconds(1);
        carGo = true;
        carStart = true;
    }

    IEnumerator carSlow()
    {
        yield return new WaitForSeconds(slowTime);
        print("slow down");
        Time.timeScale = 0.01f;
        dodgeText.GetComponent<Text>().enabled = true;
        isSlowed = true;
    }

    
}
