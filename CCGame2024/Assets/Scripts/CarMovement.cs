using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarMovement : MonoBehaviour
{
    [SerializeField] Light headRight;
    [SerializeField] Light headLeft;
    [SerializeField] int carSpeed;
    [SerializeField] Animator carMove;
    AnimatorStateInfo animStateInfo;
    GameObject dodgeText;
    GameObject player;
    Animator playerAnim;
    public float Ntime;

    bool carGo;
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
        carGo = false;
        StartCoroutine(carStart());
    }

    // Update is called once per frame
    void Update()
    {
        //print(carGo);
        if(carGo)
        {
            print(carMove.GetCurrentAnimatorStateInfo(0).normalizedTime);
            headRight.enabled = true;
            headLeft.enabled = true;
            carMove.SetTrigger("carDrive");
            //animStateInfo = carMove.GetCurrentAnimatorStateInfo(0);
            //Ntime = animStateInfo.normalizedTime;
            if(carMove.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.22f)
            {
                print("slow down");
                Time.timeScale = 0.1f;
                dodgeText.GetComponent<Text>().enabled = true;
                if(Input.GetKeyDown("q"))
                {
                    Time.timeScale = 1.0f;
                    dodgeText.GetComponent<Text>().enabled = false;
                    playerDodge();
                }
            }
        }

    }

    void playerDodge()
    {
        playerAnim.SetTrigger("playerRoll");
    }

    IEnumerator carStart()
    {
        yield return new WaitForSeconds(1);
        carGo = true;
    }

    
}
