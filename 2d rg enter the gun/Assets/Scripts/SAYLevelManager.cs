using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SAYLevelManager : MonoBehaviour
{
    public static SAYLevelManager instance;

    public float waitToLoad = 4f;

    public bool isPaused;

    public int currentCoins;

    public Transform startPoint;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SAYController.instance.cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        SAYController.instance.transform.position = startPoint.transform.position;
        SAYController.instance.canMove = true;

        currentCoins = CharacterTracker.instance.currentCoins;

        Time.timeScale = 1f;

        SAYUIController.instance.coinText.text = currentCoins.ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    public IEnumerator LevelEnd(string nextLevel)
    {
        SAYController.instance.canMove = false;
        SAYController.instance.StopMoving();

        SAYUIController.instance.StartFadeToBlack();

        yield return new WaitForSeconds(waitToLoad);

        CharacterTracker.instance.currentCoins = currentCoins;
        CharacterTracker.instance.currentHealth = SAYPlayerHealthController.instance.currentHealth;
        CharacterTracker.instance.maxHealth = SAYPlayerHealthController.instance.maxHealth;

        SceneManager.LoadScene(nextLevel);
    }

    public void PauseUnpause()
    {
        if (!isPaused)
        {
            SAYUIController.instance.pauseMenu.SetActive(true);

            isPaused = true;

            Time.timeScale = 0f;
        }
        else
        {
            SAYUIController.instance.pauseMenu.SetActive(false);

            isPaused = false;

            Time.timeScale = 1f;
        }
    }

    public void GetCoins(int amount)
    {
        currentCoins += amount;

        SAYUIController.instance.coinText.text = currentCoins.ToString();
    }

    public void SpendCoins(int amount)
    {
        currentCoins -= amount;

        if (currentCoins < 0)
        {
            currentCoins = 0;
        }

        SAYUIController.instance.coinText.text = currentCoins.ToString();
    }
}
