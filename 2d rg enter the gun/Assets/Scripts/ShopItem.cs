using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public string upgradeName;
    public int itemCost;

    public GameObject buyMessage;

    public bool isHealthRestore, isHealthUpgrade, isWeapon;

    public int healthUpgradeAmount;

    public int swordID;
    public Sprite swordSprite;
    public Text infoText;

    public GameObject weaponToDisable;

    bool inBuyZone;

    bool isBought;

    void Start()
    {


        infoText.text = upgradeName + "\n - " + itemCost + " Gold - ";

    }

    void Update()
    {
        if (inBuyZone)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (SAYLevelManager.instance.currentCoins >= itemCost)
                {
                    SAYLevelManager.instance.SpendCoins(itemCost);

                    if (isHealthRestore)
                    {
                        SAYPlayerHealthController.instance.HealPlayer(healthUpgradeAmount);
                    }

                    if (isHealthUpgrade)
                    {
                        SAYPlayerHealthController.instance.IncreaseMaxHealth(healthUpgradeAmount);
                    }

                    if (isWeapon && isBought == false)
                    {
                        SAYController.instance.BuyWeaponUpgrade(swordID);

                        weaponToDisable.gameObject.SetActive(false);
                        isBought = true;
                        buyMessage.SetActive(false);

                        inBuyZone = false;
                    }



                    //AudioManager.instance.PlaySFX(18);
                }
                else
                {
                    //AudioManager.instance.PlaySFX(19);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (isWeapon && isBought == false)
            {
                buyMessage.SetActive(true);
            }

            if (isWeapon == false)
            {
                buyMessage.SetActive(true);
            }

            inBuyZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (isWeapon && isBought == false)
            {
                buyMessage.SetActive(false);
            }

            if (isWeapon == false)
            {
                buyMessage.SetActive(false);
            }

            inBuyZone = false;
        }
    }
}
