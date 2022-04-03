using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralEnemy : MonoBehaviour
{
    public float health;

    public GameObject[] coins;

    public void DropCoin()
    {
        int random = Random.Range(0, 101);
        if (random <= 60 && random > 30)
        {
            Instantiate(coins[0], transform.position, Quaternion.identity);
        }
        else if (random <= 30 && random > 10)
        {
            Instantiate(coins[1], transform.position, Quaternion.identity);
        }
        else if (random <= 10)
        {
            Instantiate(coins[2], transform.position, Quaternion.identity);
        }
    }


    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
    }

}
