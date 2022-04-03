using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallGreenSlime : MonoBehaviour
{
    public Room theRoom;

    public bool canDealDamageIfCollides;
    public float TimeToStayStill = 2;
    public float TImeToLand = 0.5f;

    public float TimeToMove = 3;

    public float TimeToGetHit = 0.1f;

    public float Speed = 5f;
    float activeSpeed;
    public float speedWhenHit;
    public float speedWhenDie;
    public float slowSpeed = 0.1f;

    public float howLongAwayAfterDestroyed;


    public float MaxDistance = 50; // Move only if the player is close than 50 Units

    public Transform Player;

    public float dist;

    float counter;

    [Header("Bullets")]

    public GameObject projectile;

    public bool canShoot;

    public enum SlimeStates { Idle, Jump, Land, Stay, Hit , Death , PostMortem };

    SlimeStates currentState;

    Animator animator;
    string currentAnimationState;

    [Header("Taking damage")]
    public Material matWhite;
    Material matDefault;
    SpriteRenderer sr;

    Rigidbody2D rb;

    SAYController playerController;

    float counterToStart = 0.5f;

    GeneralEnemy genEnemy;

    private void Start()
    {
        genEnemy = GetComponent<GeneralEnemy>();
        playerController = GameObject.FindObjectOfType<SAYController>();

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();

        activeSpeed = Speed;

        //currentState = SlimeStates.Jump;
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        counter = TimeToMove;
        matDefault = sr.material;
    }
    void Update()
    {
        if (Player.transform.position.x > transform.position.x)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;

        }

        if (!theRoom.roomActive) return;

        if(counterToStart > 0)
        {
            counterToStart -= Time.deltaTime;
            return;
        }
        else
        {
            switch (currentState)
            {
                case SlimeStates.Idle:
                    ChangeAnimationState("SmallGreenSlime_idle");

                    if (Vector2.Distance(transform.position, Player.position) < MaxDistance && genEnemy.health != 0)
                    {
                        currentState = SlimeStates.Jump;
                    }
                    break;

                case SlimeStates.Jump:
                    if (genEnemy.health != 0)
                    {
                        if (counter > 0)
                        {
                            Jump();
                            counter -= Time.deltaTime;
                        }
                        else
                        {
                            counter = TImeToLand;
                            currentState = SlimeStates.Land;
                        }
                    }

                    break;
                case SlimeStates.Land:
                    {
                        if (genEnemy.health != 0)
                        {
                            ChangeAnimationState("SmallGreenSlime_Landing");

                            if (counter > 0)
                            {
                                counter -= Time.deltaTime;

                                if (canShoot && genEnemy.health != 0 && Vector2.Distance(transform.position, Player.position) > 0.1f)//&& Vector2.Distance(transform.position , Player.position) > 1)
                                {
                                    Instantiate(projectile, transform.position, Quaternion.identity); // The bullet should move in some way, there is no code here to move the bullet
                                    canShoot = false;
                                }

                            }
                            else
                            {
                                canShoot = true;
                                counter = TimeToStayStill;
                                currentState = SlimeStates.Stay;
                            }
                        }


                    }
                    break;
                case SlimeStates.Stay:
                    if (genEnemy.health != 0)
                    {
                        if (counter > 0)
                        {
                            ChangeAnimationState("SmallGreenSlime_preperingToJump");
                            counter -= Time.deltaTime;
                        }
                        else
                        {
                            counter = TimeToMove;
                            currentState = SlimeStates.Jump;
                        }
                    }

                    break;
                case SlimeStates.Hit:
                    ChangeAnimationState("SmallGreenSlime_TakingDamage");
                    if (counter > 0)
                    {
                        counter -= Time.deltaTime;
                        Vector3 moveDirection = transform.position - Player.transform.position;
                        moveDirection.Normalize();
                        rb.velocity = moveDirection * speedWhenHit;
                    }
                    else
                    {
                        rb.velocity = Vector2.zero;
                        counter = TimeToStayStill;
                        currentState = SlimeStates.Stay;
                    }
                    break;

                case SlimeStates.Death:
                    {
                        Invoke("ResetMaterial", 0.1f); //jak dlugo ma byc bialy
                        ChangeAnimationState("SmallGreenSlime_TakingDamage");
                        Vector3 movingWay = transform.position - Player.transform.position;
                        movingWay.Normalize();
                        rb.velocity = movingWay * speedWhenDie;
                        Invoke("KillSelf", howLongAwayAfterDestroyed);
                        //CinemachineShake.instance.ShakeCamera(ShakeLevel.Two, 1f);
                        currentState = SlimeStates.PostMortem;
                    }
                    break;

                case SlimeStates.PostMortem:
                    break;



            }
        }

        

    }


    float prevValue = -1;
    void Jump()
    {
        /*
        float percent = TimeToMove - counter;
        if (percent < 0) percent = 0;
        if (percent > .5f) percent = .5f;
        percent *= 2;
        activeSpeed = Mathf.Lerp(slowSpeed, Speed, percent);

        if (prevValue != activeSpeed)
        {
            Debug.Log("Value changed! " + activeSpeed);
            prevValue = activeSpeed;
        }*/

        ChangeAnimationState("SmallGreenSlime_Jump");
        transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, activeSpeed * Time.deltaTime);
    }


    void ChangeAnimationState(string newState)
    {
        if (currentAnimationState == newState) return;

        animator.Play(newState);

        currentAnimationState = newState;
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
            sr.material = matWhite;
            if (genEnemy.health <= 0)
            {
                gameObject.GetComponent<CircleCollider2D>().enabled = false;
                currentState = SlimeStates.Death;
            }
            else
            {
                counter = TimeToGetHit;
                currentState = SlimeStates.Hit;
                Invoke("ResetMaterial", 0.1f); //jak dlugo ma byc bialy
            }
        }

        if(collision.tag == "Player")
        {
            if(canDealDamageIfCollides)
            {
                if (SAYController.instance.isInvincible == false)
                {
                    CameraShaker.ShakeStrong(.20f, .35f);
                    SAYPlayerHealthController.instance.DamagePlayer();
                    Destroy(gameObject);
                }
            }
        }
    }

    void KillSelf()
    {
        Destroy(gameObject);

        gameObject.GetComponent<GeneralEnemy>().DropCoin();
    }









    /*
    IEnumerator Move()
    {
        float moveTime = TimeToMove;

        while (true)
        { // Forever
            yield return null; // Wait next frame

            // Check distance
            Vector2 playerPos = Player.position;
            Vector2 myPos = transform.position;
            Vector2 dirV = (playerPos - myPos);

            dist = dirV.magnitude;
            if (Vector2.Distance(myPos, playerPos) < MaxDistance)
            {

                moveTime -= Time.deltaTime;
                if (moveTime > 0)
                {   // Moving
                    //transform.position = dirV * Speed * Time.deltaTime; // This one is faster if the enemy is far away  (use a smaller Speed value)
                    // transform.position = dirV.normalized * Speed * Time.deltaTime; // This one is constant speed (use a bigger Speed value)
                    Vector3 dirV3 = dirV;
                    transform.position += dirV3 * Speed * Time.deltaTime;

                }
                else if (moveTime > -TimeToStayStill)
                {
                    // Just wait here doing nothing
                }
                else
                {
                    // Start again
                    moveTime = TimeToMove;
                }
            }
            else
            {
                yield return new WaitForSeconds(.1f);
            }
        }
    }
    */
}