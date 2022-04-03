using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossClass : MonoBehaviour
{
    public static BossClass instance;

    public float MaxHealth;

    public float Health;

    public string bossName;

    public bool playIntro;

    public GameObject explosionWhenDie;

    public Material matWhite;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SAYUIController.instance.bossName.text = bossName;

        Health = MaxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        Health -= damageAmount;

        if (Health <= 0)
        {

            //Instantiate(deathEffect, transform.position, transform.rotation);

            /*if (Vector3.Distance(player.transform.position, levelExit.transform.position) < 2f)
            {
                levelExit.transform.position += new Vector3(4f, 0f, 0f);
            }

            levelExit.SetActive(true);*/

            SAYUIController.instance.bossBarAll.gameObject.SetActive(false);
        }

        SAYUIController.instance.bossHealthBar.fillAmount = Health / MaxHealth;
    }
}
