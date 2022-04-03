using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    public string levelToLoad;

    bool canExit;

    public GameObject someText;

    public bool isInTown;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && canExit)
        {
            StartCoroutine(SAYLevelManager.instance.LevelEnd(levelToLoad));
            canExit = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && isInTown)
        {
            StartCoroutine(GoFromTown(levelToLoad));


            
        }

        if (other.tag == "Player" && isInTown == false)
        {
            canExit = true;
            someText.SetActive(true);
        }
    }

    public IEnumerator GoFromTown(string nextLevel)
    {
        SAYUIController.instance.StartFadeToBlack();

        yield return new WaitForSeconds(1f);

        CharacterTracker.instance.currentCoins = SAYLevelManager.instance.currentCoins;
        CharacterTracker.instance.currentHealth = SAYPlayerHealthController.instance.currentHealth;
        CharacterTracker.instance.maxHealth = SAYPlayerHealthController.instance.maxHealth;

        Destroy(SAYController.instance.gameObject);
        SceneManager.LoadScene("Level 1");

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && isInTown == false)
        {
            canExit = false;
            someText.SetActive(false);
        }
    }
}
