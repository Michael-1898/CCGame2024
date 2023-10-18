using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    [SerializeField] int transitionToIndex;
    [SerializeField] Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut()
    {
        anim.SetTrigger("fadeOut");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(transitionToIndex);
    }
}
