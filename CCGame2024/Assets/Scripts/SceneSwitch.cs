using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    [SerializeField] int transitionToIndex;
    [SerializeField] Animator anim;
    [SerializeField] GameObject fade;

    // Start is called before the first frame update
    void Start()
    {
        fade.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void switchScene(int transitionIndex)
    {
        StartCoroutine(FadeOut());
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
        fade.SetActive(true);
        anim.SetTrigger("fadeOut");
        //buttonAnim.SetTrigger("buttonFadeIn");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(transitionToIndex);
    }
}
