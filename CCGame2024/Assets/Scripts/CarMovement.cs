using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] Light headRight;
    [SerializeField] Light headLeft;
    [SerializeField] int carSpeed;
    [SerializeField] Animator carMove;
    AnimatorStateInfo animStateInfo;
    GameObject player;
    Animator playerAnim;
    public float Ntime;

    bool carGo;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerAnim = player.GetComponent<Animator>();
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
            headRight.enabled = true;
            headLeft.enabled = true;
            carMove.SetTrigger("carDrive");
            animStateInfo = carMove.GetCurrentAnimatorStateInfo (0);
            Ntime = animStateInfo.normalizedTime;
            if(Ntime > 1.0f)
            {
                playerDodge();
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
