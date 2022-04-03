using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookySpawner : MonoBehaviour
{
    public int health;

    public float timeToResp;

    float counter;

    public GameObject objectToResp;

    Material matDefault;
    public Material matWhite;

    SpriteRenderer sr;

    SpookySpawner boss;

    // Start is called before the first frame update
    void Start()
    {
        SpookyWoodBoss.instance.spawnersList.Add(gameObject);
        sr = GetComponent<SpriteRenderer>();
        matDefault = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (SpookyWoodBoss.instance.isDead)
        {
            Destroy(gameObject);
        }

        if (counter > 0)
        {
            counter -= Time.deltaTime;
        }
        else
        {
            if (SpookyWoodBoss.instance.maxEnemyNumber >= SpookyWoodBoss.instance.enemyCounter)
            {
                Instantiate(objectToResp, transform.position, Quaternion.identity);
                SpookyWoodBoss.instance.enemyCounter++;
            }

            counter = timeToResp;
        }
    }

    void ResetMaterial()
    {
        sr.material = matDefault;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerBullet")
        {
            Destroy(collision.gameObject);
            health--;
            sr.material = matWhite;
            if (health <= 0)
            {
                SpookyWoodBoss.instance.spawnersList.Remove(gameObject);
                Destroy(gameObject);
            }
            else
            {
                Invoke("ResetMaterial", 0.1f); //jak dlugo ma byc bialy
            }
        }
    }

}
