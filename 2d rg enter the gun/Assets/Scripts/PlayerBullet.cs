using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 7.5f;
    public Rigidbody2D theRB;

    //public GameObject impactEffect;

    public float damageToGive;

    [Header("DestroyAfterSeconds")]
    public bool shouldDestroyAfterTime;
    public float timeToBeDestroyed;
    float counter;

    public bool shouldntDestroyOnCollision;
    

    void Start()
    {
        counter = timeToBeDestroyed;
        
    }

    void Update()
    {
        theRB.velocity = transform.right * speed;

        if(shouldDestroyAfterTime)
        {
            if(counter > 0)
            {
                counter -= Time.deltaTime;
            }
            else if(counter <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Instantiate(impactEffect, transform.position, transform.rotation);
        if(shouldntDestroyOnCollision == false)
        {
            Destroy(gameObject);
        }

        //AudioManager.instance.PlaySFX(4);

        if (other.tag == "Enemy")
        {
            if (other.GetComponent<GeneralEnemy>() != null)
            {
                other.GetComponent<GeneralEnemy>().TakeDamage(damageToGive);
            }

            if (shouldDestroyAfterTime)
            {
                //CinemachineShake.instance.ShakeCamera(ShakeLevel.One, 3f);
            }

        }

        if(other.tag == "Boss")
        {
            BossClass.instance.TakeDamage(damageToGive);

            //Instantiate(BossController.instance.hitEffect, transform.position, transform.rotation);
        }
    }

}
