using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour //doesn't overlap on enemy collision cause we used "collisions layers"
{
    public float speed;
    Vector3 direction;

    public GameObject player;

    [Header("DestroyAfterSeconds")]
    public bool shouldDestroyAfterTime;
    public float timeToBeDestroyed;
    float counter;

    private void Awake()
    {
        counter = timeToBeDestroyed;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        if(player != null)
        {
            direction = player.transform.position - transform.position;
            direction.Normalize();
        }
        
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        if (shouldDestroyAfterTime)
        {
            if (counter > 0)
            {
                counter -= Time.deltaTime;
            }
            else if (counter <= 0)
            {
                Destroy(gameObject);
            }
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
    }

}
