using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploadRight : MonoBehaviour
{
    public float speed;

    public float lifeTime;

    public Transform[] firePoints;

    float counter;

    public GameObject bulletGoesRight;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        counter = lifeTime;
    }

    // Update is called once per frame
    void Update()
    {


        transform.position += transform.right * speed * Time.deltaTime;

        if(counter > 0)
        {
            counter -= Time.deltaTime;
        }
        else
        {
            Expload();
        }
    }

    private void Expload()
    {
        foreach (Transform t in firePoints)
        {
            Instantiate(bulletGoesRight, t.position, t.rotation);
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Expload();
    }

}

