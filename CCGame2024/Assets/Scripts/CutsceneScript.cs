using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneScript : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] GameObject fade;
    [SerializeField] GameObject point1;
    [SerializeField] GameObject point2;
    [SerializeField] int sceneIndex;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Cutscene());
    }

    IEnumerator Cutscene() {
        yield return new WaitForSeconds(2f);

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
        transform.Rotate(-16.45f, -170.3f, 88);
        anim.SetTrigger("fadeIn");
        yield return new WaitForSeconds(2f);

        //fadeout and switch scene
        anim.SetTrigger("fadeOut");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneIndex);
    }
}