using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarMovement : MonoBehaviour
{

    [SerializeField] Light headRight;
    [SerializeField] Light headLeft;
    [SerializeField] Light moonLight;
    [SerializeField] float slowTime;
    [SerializeField] int carSpeed;
    [SerializeField] Animator carMove;

    Animator modelAnim;
    AnimatorStateInfo animStateInfo;
    GameObject dodgeText;
    GameObject player;
    GameObject model;

    bool carGo;
    bool carStart;
    bool isSlowed;
    bool resetPosition;

    // Start is called before the first frame update
    void Start()
    {

        Time.timeScale = 1.0f;
        dodgeText = GameObject.Find("Dodge Text");
        player = GameObject.Find("Player");
        model = GameObject.Find("Model");
        modelAnim = model.GetComponent<Animator>();

        dodgeText.GetComponent<Text>().enabled = false;
        player.GetComponent<MikeLook>().enabled = false;
        player.GetComponent<MikeMovement>().enabled = false;

        headRight.enabled = false;
        headLeft.enabled = false;
        moonLight.enabled = false;
        carGo = false;
        carStart = false;
        isSlowed = false;
        resetPosition = true;

        StartCoroutine(carDrive());
    }

    // Update is called once per frame
    void Update()
    {
        if(carGo)
        {
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
                //moonLight.enabled = true;
                isSlowed = false;
            }
        
        }

        if(!(modelAnim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 < 0.99f) && carGo)
        {
            if(resetPosition)
            {
                // player.transform.position = getChildWorldSpacePos(player, 0); <-- this is giving the correct position
                // model.transform.position = new Vector3(0, 0, 0); <-- This line of code isn't working, need to set model position to 0,0,0 relative to the player,s position

                // resetPosition = false;
                print(getChildWorldSpacePos(player, 0));
            }
            player.GetComponent<MikeLook>().enabled = true;
            player.GetComponent<MikeMovement>().enabled = true;
        }

    }

    void playerDodge()
    {
        modelAnim.SetTrigger("playerRoll");
    }

    IEnumerator carDrive()
    {
        yield return new WaitForSeconds(1);
        headLeft.enabled = true;
        headRight.enabled = true;

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

    Vector3 getChildWorldSpacePos(GameObject parent, int childNum)
    {
        return parent.transform.GetChild(childNum).position;
    }

    
}
