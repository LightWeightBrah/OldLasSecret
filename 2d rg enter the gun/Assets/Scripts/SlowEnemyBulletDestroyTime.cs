using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEnemyBulletDestroyTime : MonoBehaviour
{
    public float speed;

    public float timeToBeDestroyed;
    float counter;

    // Start is called before the first frame update
    void Start()
    {
        counter = timeToBeDestroyed;
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
    }
}
