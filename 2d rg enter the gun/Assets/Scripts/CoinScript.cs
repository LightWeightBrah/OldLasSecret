using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public float yOffset;
    public float fallSpeed;
    public float moveToPlayerSpeed;

    Vector2 enemyPosition;

    bool hasFell;

    bool goToPlayer;

    GameObject player;

    public float pickUpDistance;

    public int howManyCoinsToGet;

    bool hasPickedCoin;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        enemyPosition = transform.position;
        transform.position = new Vector3(enemyPosition.x, enemyPosition.y + yOffset, transform.position.z);
    }

    void Update()
    {
        if(hasFell == false)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyPosition, fallSpeed * Time.deltaTime);
            if((Vector2)transform.position == enemyPosition)
            {
                hasFell = true;

            }
        }
        else if(hasFell == true && Vector2.Distance(transform.position, player.transform.position) < pickUpDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveToPlayerSpeed * Time.deltaTime);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Destroy(gameObject);

            if(hasPickedCoin == false)
            {
                hasPickedCoin = true;

                switch (howManyCoinsToGet)
                {
                    case 1:
                        SAYLevelManager.instance.GetCoins(1);
                        break;

                    case 2:
                        SAYLevelManager.instance.GetCoins(2);
                        break;

                    case 3:
                        SAYLevelManager.instance.GetCoins(3);
                        break;
                }
            }
            
        }

    }

}
