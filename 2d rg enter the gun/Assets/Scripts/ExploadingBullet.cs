using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploadingBullet : MonoBehaviour
{
    public float speed;

    public float lifeTime;

    public Transform[] firePoints;

    float counter;

    public GameObject bulletGoesRight;

    Vector2 direction;

    GameObject player;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // Start is called before the first frame update
    void Start()
    {
        


        if(player != null)
        {
            direction = player.transform.position;

            counter = lifeTime;
        }
        
    }

    // Update is called once per frame
    void Update()
    {


        transform.position = Vector2.MoveTowards(transform.position, direction, speed * Time.deltaTime);

        if((Vector2)transform.position == direction)
        {
            foreach (Transform t in firePoints)
            {
                Instantiate(bulletGoesRight, t.position, t.rotation);
            }
            Destroy(gameObject);
        }

        if(counter > 0)
        {
            counter -= Time.deltaTime;
        }
        else
        {
            foreach(Transform t in firePoints)
            {
                Instantiate(bulletGoesRight, t.position, t.rotation);
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (SAYController.instance.isInvincible == false)
            {
                CameraShaker.ShakeStrong(.20f, .35f);
                SAYPlayerHealthController.instance.DamagePlayer();
                Destroy(gameObject);
            }
        }
        if (other.tag != "Player")
        {
            Destroy(gameObject);
        }

        foreach (Transform t in firePoints)
        {
            Instantiate(bulletGoesRight, t.position, t.rotation);
        }
    }

}
