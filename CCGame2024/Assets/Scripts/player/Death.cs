using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    [SerializeField] Animator anim;

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
        anim.SetTrigger("fadeRedIn");
        yield return new WaitForSeconds(1);
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        //SceneManager.LoadScene(sceneIndex);
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
