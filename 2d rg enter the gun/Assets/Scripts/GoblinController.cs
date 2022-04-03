using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinController : MonoBehaviour
{
    public Room theRoom;

    [Header("Variables")]
    public float speed = 5f;
    public float triggerDistance = 10f;
    public float TimetoTakeDamage = 0.1f;

    public float distanceToAttack = 4f;

    public float speedWhenHit = 2f;

    public float speedWhenDie = 3f;
    public float howLongAwayAfterDestroyed = 1f;
    public enum GoblinStates { Idle, Walk , TakeDamage, Die, PostMortem };

    GoblinStates currenState;

    public Transform swordTransform;

    public Transform playerTransform;

    public SpriteRenderer bodySR;

    public SpriteRenderer swordSR;

    float counter;

    public float TimeBetweenAttacks = 2f;

    Rigidbody2D rb;

    [Header("Combat")]
    public GameObject projectile;
    public Transform firePoint;

    public Material matWhite;
    Material matDefault;

    Animator animator;
    string currentAnimationState;

    float angle;
    float maxAngle = 20f;

    GeneralEnemy genEnemy;

    // Start is called before the first frame update
    void Start()
    {
        genEnemy = GetComponent<GeneralEnemy>();

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        matDefault = bodySR.material;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!theRoom.roomActive) return;

        Vector2 direction = playerTransform.position - transform.position;
        angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        firePoint.rotation = Quaternion.Euler(0, 0, angle);


        if (playerTransform.position.x > transform.position.x)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            gameObject.transform.rotation = Quaternion.Euler(0, -180, 0);

        }

        /*
        if (playerTransform.position.x > transform.position.x)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

            if (angle < maxAngle)
            {
                if(playerTransform.position.y > transform.position.y)
                {
                    swordTransform.rotation = Quaternion.Euler(0, 0, angle);
                }
                else
                {
                    swordTransform.rotation = Quaternion.Euler(0, 0, -angle);
                }
            }
        }
        else
        {
            gameObject.transform.rotation = Quaternion.Euler(0, -180, 0);

            if (angle < maxAngle)
            {
                if(playerTransform.position.y > transform.position.y)
                {
                    swordTransform.rotation = Quaternion.Euler(0, 0, angle);
                }
                else
                {
                    swordTransform.rotation = Quaternion.Euler(0,  0, -angle);
                }
            }

        }
        */

        switch(currenState)
        {
            case GoblinStates.Idle:

                ChangeAnimationState("Goblin_Idle");

                if (Vector2.Distance(transform.position, playerTransform.position) < triggerDistance)
                {
                    currenState = GoblinStates.Walk;

                }
                break;

            case GoblinStates.Walk:
                ChangeAnimationState("Goblin_Walk");

                transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);

                if (counter > 0)
                {
                    counter -= Time.deltaTime;
                }

                if (Vector2.Distance(transform.position, playerTransform.position) < distanceToAttack)
                {
                    if(counter <= 0)
                    {
                        Instantiate(projectile, firePoint.position, firePoint.rotation);
                        counter = TimeBetweenAttacks;
                    }
                }
                
              
                break;

         

            case GoblinStates.TakeDamage:
                ChangeAnimationState("Goblin_TakeDamage");

                if (counter > 0)
                {
                    counter -= Time.deltaTime;
                    Vector3 moveDirection = transform.position - playerTransform.position;
                    moveDirection.Normalize();
                    rb.velocity = moveDirection * speedWhenHit;
                }
                else
                {
                    rb.velocity = Vector2.zero;
                    counter = TimeBetweenAttacks;
                    currenState = GoblinStates.Walk;
                }

                break;

            case GoblinStates.Die:


                Invoke("ResetMaterial", 0.1f); //jak dlugo ma byc bialy

                ChangeAnimationState("Goblin_TakeDamage");

                Vector3 movingWay = transform.position - playerTransform.position;
                movingWay.Normalize();
                rb.velocity = movingWay * speedWhenDie;
                Invoke("KillSelf", howLongAwayAfterDestroyed);

                CameraShaker.ShakeStrong(.20f, .25f);

                currenState = GoblinStates.PostMortem;

                break;

            case GoblinStates.PostMortem:

                break;


        }
    }

    void ChangeAnimationState(string newState)
    {
        if (currentAnimationState == newState) return;

        animator.Play(newState);

        currentAnimationState = newState;
    }

    void KillSelf()
    {
        Destroy(gameObject);

        gameObject.GetComponent<GeneralEnemy>().DropCoin();
    }

    void ResetMaterial()
    {
        bodySR.material = matDefault;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerBullet")
        {
            Destroy(collision.gameObject);
            
            bodySR.material = matWhite;
            if(genEnemy.health <= 0)
            {
                currenState = GoblinStates.Die;
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
            else
            {
                counter = TimetoTakeDamage;
                currenState = GoblinStates.TakeDamage;
                Invoke("ResetMaterial", 0.1f);
            }
        }
    }
}
