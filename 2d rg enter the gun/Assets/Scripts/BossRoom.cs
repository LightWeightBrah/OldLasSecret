using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    public static BossRoom instance;

    public GameObject doors;

    public GameObject nextLevel;

    public GameObject boss;

    public bool level2;

    private void Awake()
    {
        instance = this;
    }

    public void BossDead()
    {
        doors.SetActive(false);
        nextLevel.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && level2)
        {
            boss.GetComponentInChildren<Collider2D>().enabled = true;
            doors.SetActive(true);
        }

        else if (other.tag == "Player")
        {
            boss.GetComponent<Collider2D>().enabled = true;
            doors.SetActive(true);
        }
    }
}
