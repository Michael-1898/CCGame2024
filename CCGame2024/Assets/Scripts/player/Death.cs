using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    [SerializeField] Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*IEnumerator fadeRed()
    {
        anim.SetTrigger("fadeOut");
    }*/

    void PlayerDeath(int sceneIndex)
    {
        //SceneManager.LoadScene(sceneIndex);
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.name == "Cube")
        {
            PlayerDeath(2);
        }
    }
}
