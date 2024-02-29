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

    // Update is called once per frame
    void Update()
    {
        //initial blink/fade in
        // StartCoroutine(Wait(1f));
        // transform.position = point1.transform.position;
        // StartCoroutine(Blink());
        // transform.position = point2.transform.position;
        // StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        fade.SetActive(true);
        anim.SetTrigger("fadeOut");
        yield return new WaitForSeconds(1);
        anim.SetTrigger("fadeIn");
    }

    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
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
    }
}