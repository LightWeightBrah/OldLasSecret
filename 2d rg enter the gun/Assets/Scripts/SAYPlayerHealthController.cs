using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAYPlayerHealthController : MonoBehaviour
{
    public static SAYPlayerHealthController instance;

    public int currentHealth;
    public int maxHealth;

    public float damageInvincLength = 1f;
    float invincCount;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        maxHealth = CharacterTracker.instance.maxHealth;
        currentHealth = CharacterTracker.instance.currentHealth;

        //currentHealth = maxHealth;

        SAYUIController.instance.healthSlider.maxValue = maxHealth;
        SAYUIController.instance.healthSlider.value = currentHealth;
        SAYUIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }

    void Update()
    {
        if (invincCount > 0)
        {
            invincCount -= Time.deltaTime;

            if (invincCount <= 0) //isn't invincible
            {
                SAYController.instance.isInvincible = false;

                /*SAYController.instance.bodySR.color = new Color(PlayerController.instance.bodySR.color.r, SAYController.instance.bodySR.color.g, PlayerController.instance.bodySR.color.b, 1f);*/

                SAYController.instance.bodySr.color = SAYController.instance.defaultColor;
            }
        }
    }

    public void DamagePlayer()
    {
        if (invincCount <= 0) //isn't invincible
        {

            SAYController.instance.isInvincible = true;
            currentHealth--;

            invincCount = damageInvincLength;

            SAYController.instance.bodySr.color = SAYController.instance.redColor;

            /*SAYController.instance.bodySR.color = new Color(SAYController.instance.bodySR.color.r, SAYController.instance.bodySR.color.g, SAYController.instance.bodySR.color.b, 0.5f);*/

            if (currentHealth <= 0)
            {
                SAYController.instance.gameObject.SetActive(false);
                SAYUIController.instance.deathScreen.SetActive(true);
            }

            SAYUIController.instance.healthSlider.value = currentHealth;
            SAYUIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
        }

    }

    public void MakeInvincible(float length)
    {
        invincCount = length;
        SAYController.instance.bodySr.material = SAYController.instance.matRed;
  /*      SAYController.instance.bodySR.color = new Color(PlayerController.instance.bodySR.color.r, PlayerController.instance.bodySR.color.g, PlayerController.instance.bodySR.color.b, 0.5f);*/
    }

    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        SAYUIController.instance.healthSlider.value = currentHealth;
        SAYUIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth += amount;

        SAYUIController.instance.healthSlider.maxValue = maxHealth;
        SAYUIController.instance.healthSlider.value = currentHealth;
        SAYUIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }
}
