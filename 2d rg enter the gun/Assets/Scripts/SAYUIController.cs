using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SAYUIController : MonoBehaviour
{
    public static SAYUIController instance;

    public Slider healthSlider;

    public Text healthText, coinText;

    public GameObject deathScreen;

    public Image fadeScreen;
    public float fadeSpeed;
    bool fadeToBlack, fadeOutBlack;

    public string newGameScene, mainMenuScene;

    public GameObject pauseMenu, mapDisplay, bigMapText;

    public Image bossHealthBar;
    public Image bossBarAll;
    public Text bossName;

    bool isMinimapEnabled;

    public bool isInTown;

    public bool canEnableMinimap = true;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        fadeOutBlack = true;
        fadeToBlack = false;

        if(isInTown == true)
        {
            mapDisplay.SetActive(false);
        }

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

        //minimap
        /*if (isInTown == false)
        {
            if(canEnableMinimap == true)
            {
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    if (isMinimapEnabled == false)
                    {
                        isMinimapEnabled = true;
                        mapDisplay.SetActive(true);
                    }
                    else if (isMinimapEnabled == true)
                    {
                        isMinimapEnabled = false;
                        mapDisplay.SetActive(false);
                    }
                }
            }
            
        }*/

        
    }

    public void StartFadeToBlack()
    {
        fadeToBlack = true;
        fadeOutBlack = false;
    }

    public void NewGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(newGameScene);

        Destroy(SAYController.instance.gameObject);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(mainMenuScene);

        Destroy(SAYController.instance.gameObject);
    }

    public void Resume()
    {
        SAYLevelManager.instance.PauseUnpause();
    }
}
