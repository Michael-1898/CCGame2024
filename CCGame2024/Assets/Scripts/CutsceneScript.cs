using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneScript : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] GameObject fade;
    [SerializeField] GameObject point1;
    [SerializeField] GameObject point2;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Cutscene());
    }

    IEnumerator Cutscene() {
        yield return new WaitForSeconds(1.75f);

        //blink
        fade.SetActive(true);
        anim.SetTrigger("fadeOut");
        yield return new WaitForSeconds(1);
        transform.position = point1.transform.position;
        anim.SetTrigger("fadeIn");
        yield return new WaitForSeconds(1.75f);

        //blink
        fade.SetActive(true);
        anim.SetTrigger("fadeOut");
        yield return new WaitForSeconds(1);
        transform.position = point2.transform.position;
        //rotate player correctly
        transform.Rotate(0, 185, 90);
        anim.SetTrigger("fadeIn");
        yield return new WaitForSeconds(1.75f);

        //fadeout and switch scene
        anim.SetTrigger("fadeOut");
        yield return new WaitForSeconds(1);
    }
}