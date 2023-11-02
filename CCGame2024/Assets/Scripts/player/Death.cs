using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Animator textAnim;
    [SerializeField] Animator buttonAnim;
    [SerializeField] Animator button2Anim;
    [SerializeField] GameObject buttonRespawn;
    [SerializeField] GameObject buttonMainMenu;

    bool isHit = false;

    // Start is called before the first frame update
    void Start()
    {
        buttonMainMenu.SetActive(false);
        buttonRespawn.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Die()
    {
        //red fade
        anim.SetTrigger("fadeRedIn");
        yield return new WaitForSeconds(1);
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        //text fade
        textAnim.SetTrigger("FadeDeathTextIn");
        yield return new WaitForSeconds(1);
    
        //button fade
        buttonMainMenu.SetActive(true);
        buttonRespawn.SetActive(true);
        buttonAnim.SetTrigger("FadeButtonIn");
        button2Anim.SetTrigger("FadeButtonIn");
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        if(hit.collider.gameObject.CompareTag("Death") && !isHit) {
            isHit = true;
            StartCoroutine(Die());
        } else if(!hit.collider.gameObject.CompareTag("Death") && isHit) {
            isHit = false;
        }
    }
}
