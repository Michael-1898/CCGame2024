using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneScript : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] GameObject fade;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Blink()
    {
        fade.SetActive(true);
        anim.SetTrigger("fadeOut");
        //buttonAnim.SetTrigger("buttonFadeIn");
        yield return new WaitForSeconds(1);
        anim.SetTrigger("fadeIn");
    }
}