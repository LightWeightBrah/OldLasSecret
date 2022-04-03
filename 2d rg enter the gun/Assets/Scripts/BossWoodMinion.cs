using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWoodMinion : MonoBehaviour
{
    [Header("Variables")]
    public int health;
    public float speed;

    [Header("Shooting")]
    public float startTimeBtwnShots;
    float timeBtwShots;
    public float shootDistance; // min distance to shoot
    public GameObject projectile;
    public int howManyBulletsToSpawn; // Set it to 6
    public float TimeBetweenBullets; // set it to 0.1f
    public float TimeToReload; // set it to 2f
    Transform player;

    [Header("Taking Damage")]
    public Material matWhite;
    Material matDefault;
    SpriteRenderer bodySR;
    public GameObject expolsionRef;

    public float speedWhenHit;

    float counter;
    public float TimeToGetHit;


    int numBulletsShoot = 0;
    float lastBullet = 0;

    Animator animator;
    string currentAnimationState;

    Rigidbody2D rb;

    enum TreeEnemyStates {Walk, Hit, Die, PostMortem };
    TreeEnemyStates currentState;

    public float howLongAwayAfterDestroyed;

    public float speedWhenDie;

    bool isAlive = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timeBtwShots = startTimeBtwnShots;

        bodySR = GetComponentInChildren<SpriteRenderer>();
        //matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material; //castuje na material bo deafultowo jest obiekt
        matDefault = bodySR.material;

    }

    void Update()
    {
        if (SpookyWoodBoss.instance.isDead)
        {
            Instantiate(expolsionRef, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }


        switch (currentState)
        {
            case TreeEnemyStates.Walk:
                ChangeAnimationState("WoodBossMinion_Walk");

                float distance = Vector2.Distance(transform.position, player.position);

                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

                // If we are close enough we will shoot
                if (distance < shootDistance)
                {
                    if (lastBullet > 0)
                    {
                        lastBullet -= Time.deltaTime;
                    }
                    else
                    {
                        numBulletsShoot++;
                        if (numBulletsShoot < howManyBulletsToSpawn)
                        { // Max "howManyBulletsToSpawn" bullets before reloading
                            lastBullet = TimeBetweenBullets;
                            Instantiate(projectile, transform.position, Quaternion.identity); // The bullet should move in some way, there is no code here to move the bullet
                        }
                        else
                        { // Reloading
                            lastBullet = TimeToReload; // 2 seconds reload
                            numBulletsShoot = 0;
                        }
                    }

                }
                break;

            case TreeEnemyStates.Hit:
                ChangeAnimationState("WoodBossMinion_Hit");
                if (counter > 0)
                {
                    counter -= Time.deltaTime;
                    Vector3 moveDirection = transform.position - player.transform.position;
                    moveDirection.Normalize();
                    rb.velocity = moveDirection * speedWhenHit;
                }
                else
                {
                    rb.velocity = Vector2.zero;
                    counter = TimeToGetHit;
                    bodySR.flipX = false;
                    currentState = TreeEnemyStates.Walk;
                }
                break;

            case TreeEnemyStates.Die:

                //CameraShaker.Shake(0.10f, 0.15f);

                Invoke("ResetMaterial", 0.1f); //jak dlugo ma byc bialy

                ChangeAnimationState("WoodBossMinion_Hit");

                Vector3 movingWay = transform.position - player.transform.position;
                movingWay.Normalize();
                rb.velocity = movingWay * speedWhenDie;
                Invoke("KillSelf", howLongAwayAfterDestroyed);


                CameraShaker.ShakeStrong(.20f, .25f);

                currentState = TreeEnemyStates.PostMortem;

                break;

            case TreeEnemyStates.PostMortem:

                break;

        }
        // Calculate the distance


    }
    void ChangeAnimationState(string newState)
    {
        if (currentAnimationState == newState) return;

        animator.Play(newState);

        currentAnimationState = newState;
    }

    void KillSelf()
    {
        Instantiate(expolsionRef, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    void ResetMaterial()
    {
        bodySR.material = matDefault;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerBullet")
        {
            Destroy(collision.gameObject);
            health--;
            bodySR.material = matWhite;
            if (player.transform.position.x > transform.position.x)
            {
                bodySR.flipX = false;
            }
            else
            {
                bodySR.flipX = true;
            }
            if (health <= 0)
            {
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                currentState = TreeEnemyStates.Die;
                if(isAlive == true)
                {
                    SpookyWoodBoss.instance.enemyCounter--;
                    isAlive = false;
                }
            }
            else
            {
                counter = TimeToGetHit;
                currentState = TreeEnemyStates.Hit;
                Invoke("ResetMaterial", 0.1f); //jak dlugo ma byc bialy
            }
        }

    }
}
