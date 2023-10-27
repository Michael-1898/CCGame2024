using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Animator textAnim;
    [SerializeField] Animator buttonAnim;

    bool isHit = false;

    // Start is called before the first frame update
    void Start()
    {
        
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
        buttonAnim.SetTrigger("FadeButtonIn");
        yield return new WaitForSeconds(1);
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
