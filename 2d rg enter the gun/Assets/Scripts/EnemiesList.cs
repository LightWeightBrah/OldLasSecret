using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesList : MonoBehaviour
{
    public static EnemiesList instance;

    public GameObject[] allEnemies;

    public int level;

    public GameObject indicator;

    private void Awake()
    {
        instance = this;
    }

}
