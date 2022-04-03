using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject Credits;
    public Image fadeScreen;
    public float fadeSpeed;
    bool fadeToBlack, fadeOutBlack;

    public float waitToLoad;

    int counter;

    public GameObject[] cutscenes;

    bool areCutScenesEnabled;

    private void Start()
    {
        foreach (GameObject g in cutscenes)
        {
            g.SetActive(false);
        }

        Credits.SetActive(false);
        fadeOutBlack = true;
        fadeToBlack = false;
    }

    void Update()
    {
        if (fadeOutBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 0f)
            {
                fadeOutBlack = false;
            }
        }

        if (fadeToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 1f)
            {
                fadeToBlack = false;
            }
        }

        if(areCutScenesEnabled && Input.anyKeyDown)
        {
            StartCoroutine(ShowNextCutscene());
        }
    }

    public IEnumerator ShowNextCutscene()
    {
        StartFadeToBlack();

        yield return new WaitForSeconds(waitToLoad);

        StartFadeOutBlack();

        if (counter + 1 == cutscenes.Length)
        {
            SceneManager.LoadScene("Town");
        }
        else
        {
            cutscenes[counter].gameObject.SetActive(false);
            counter++;
            cutscenes[counter].gameObject.SetActive(true);
        }
    }

    public void StartFadeToBlack()
    {
        fadeToBlack = true;
        fadeOutBlack = false;
    }

    public void StartFadeOutBlack()
    {
        fadeToBlack = false;
        fadeOutBlack = true;
    }

    public void PlayGame()
    {
        StartCoroutine(StartGame());
    }

    public void GoBackFromCredits()
    {
        StartCoroutine(UnloadCredits());
    }

    public void GoToCredits()
    {
        StartCoroutine(LoadCredits());
    }

    IEnumerator StartGame()
    {
        StartFadeToBlack();

        yield return new WaitForSeconds(waitToLoad);

        StartFadeOutBlack();

        cutscenes[0].gameObject.SetActive(true);

        areCutScenesEnabled = true;
    }

    IEnumerator LoadCredits()
    {
        StartFadeToBlack();

        yield return new WaitForSeconds(waitToLoad);

        Credits.SetActive(true);

        StartFadeOutBlack();

    }

    IEnumerator UnloadCredits()
    {
        StartFadeToBlack();

        yield return new WaitForSeconds(waitToLoad);

        Credits.SetActive(false);

        StartFadeOutBlack();
    }


}
