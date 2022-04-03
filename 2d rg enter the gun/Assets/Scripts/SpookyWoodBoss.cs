using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookyWoodBoss : MonoBehaviour
{
    public static SpookyWoodBoss instance;

    [Header("Variables")]
    public int maxEnemyNumber;
    public float speed;
    float activeSpeed;
    public float triggerDistance;

    public float goingToSpawnPointsSpeed;

    //[HideInInspector]
    public int enemyCounter = 0;

    [Header("States times Phase1")]
    public float introTime;
    public float stayTime;
    public float chaseWithBothHandsShootingTime;
    public float stayAndShootBombsTime;
    public float breakTime;
    public float chaseTIme;

    enum Phase1 { Intro, Stay, ChaseWithBothHandsShooting, StayAndShootBombs , SpawnEnemies , Break  , Chase};

    enum Phase2 { Intro, Stay, ChaseWithBigBall, StayAndShootBombs, BowAttack, SpawnEnemies, Break, Chase };

    Phase1 currentState = Phase1.Intro;
    Phase2 currentPhase2State = Phase2.Intro;

    [Header("Shooting")]
    public float rotateSpeedBothHandsPhase1;

    public Transform leftHand;
    public Transform rightHand;

    public GameObject bulletGoesRightPrefab;
    public GameObject bombBullet;

    float shootCounter;
    public float breakBetweenShotsBothHandsPhase1;
    public float breaksBetweenBombsPhase1;

    GameObject player;

    float counter;

    Animator animator;
    string currentAnimationState;

    [Header("Spawning")]

    public Transform[] spawnPoints;

    int currentSpawnPoint;

    public GameObject spawner;

    public List<GameObject> spawnersList = new List<GameObject>();

    bool canSpawnSpawners = true;

    [Header("Phase2 Times")]
    public float phase2Speed;
    public float phase2GoingToSpawnPointsSpeed;


    public float phase2IntroTime;
    public float phase2StayTime;
    public float phase2ChaseBigBallTime;
    public float phase2StayAndShootBombsTime;
    public float phase2BowAttackTime;
    public float phase2BreakTime;
    public float phase2ChaseTIme;

    [Header("Phase2 Shooting")]
    
    public float breakBetweenBigBallsPhase2;
    public float breaksBetweenBombsPhase2;

    public float breaksBetweenBowAttacksPhase2;

    bool hasCounterBeenSet;

    public GameObject phase2Bomb;

    public bool isDead;

    public GameObject bigBallPrefab;

    bool hasShootBigBall;

    public Transform BowAttackPoint;

    public Transform[] AllBowAttacksFirePoints;

    public GameObject bowAttackBullet;

    SpriteRenderer sr;

    Material matDefault;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        matDefault = sr.material;
        counter = introTime;
        activeSpeed = speed;
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(BossClass.instance.Health > 1250)
        {
            switch (currentState)
            {
                case Phase1.Intro:

                    if(BossClass.instance.playIntro)
                    {
                        ChangeAnimationState("SpookyWood_Intro");

                        if (counter > 0)
                        {
                            counter -= Time.deltaTime;
                        }
                        else
                        {
                            currentState = Phase1.Stay;
                            counter = stayTime;
                        }

                    }

                    break;



                case Phase1.Stay:

                    ChangeAnimationState("SpookyWood_Idle");

                    if (counter > 0)
                    {
                        counter -= Time.deltaTime;
                    }
                    else
                    {
                        currentState = Phase1.ChaseWithBothHandsShooting;
                        counter = chaseWithBothHandsShootingTime;
                    }

                    break;

                case Phase1.ChaseWithBothHandsShooting:

                    ChangeAnimationState("SpookyWood_Walk");

                    if (counter > 0)
                    {
                        counter -= Time.deltaTime;

                        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, activeSpeed * Time.deltaTime);

                        leftHand.Rotate(Vector3.forward * rotateSpeedBothHandsPhase1 * Time.deltaTime);
                        rightHand.Rotate(Vector3.back * rotateSpeedBothHandsPhase1 * Time.deltaTime);

                        if (shootCounter > 0)
                        {
                            shootCounter -= Time.deltaTime;
                        }
                        else
                        {
                            Instantiate(bulletGoesRightPrefab, leftHand.position, leftHand.rotation);
                            Instantiate(bulletGoesRightPrefab, rightHand.position, rightHand.rotation);

                            shootCounter = breakBetweenShotsBothHandsPhase1;
                        }

                    }
                    else
                    {
                        leftHand.rotation = Quaternion.identity;
                        rightHand.rotation = Quaternion.identity;
                        currentState = Phase1.StayAndShootBombs;
                        counter = stayAndShootBombsTime;
                        shootCounter = breaksBetweenBombsPhase1;
                    }

                    break;

                case Phase1.StayAndShootBombs:

                    ChangeAnimationState("SpookyWood_Idle");

                    if (counter > 0)
                    {
                        counter -= Time.deltaTime;

                        if (shootCounter > 0)
                        {
                            shootCounter -= Time.deltaTime;
                        }
                        else
                        {
                            Instantiate(bombBullet, transform.position, Quaternion.identity);
                            shootCounter = breaksBetweenBombsPhase1;
                        }

                    }
                    else
                    {
                        shootCounter = breakBetweenShotsBothHandsPhase1;

                        counter = breakTime;
                        currentState = Phase1.Break;
                    }

                    break;

                case Phase1.SpawnEnemies:

                    ChangeAnimationState("SpookyWood_Walk");

                    if (spawnersList.Count <= 0 || canSpawnSpawners)
                    {
                        activeSpeed = goingToSpawnPointsSpeed;

                        switch (currentSpawnPoint)
                        {
                            case 0:
                                canSpawnSpawners = true;
                                transform.position = Vector2.MoveTowards(transform.position, spawnPoints[0].position, activeSpeed * Time.deltaTime);

                                if (transform.position == spawnPoints[0].position)
                                {
                                    Instantiate(spawner, transform.position, Quaternion.identity);
                                    currentSpawnPoint = 1;
                                }

                                break;

                            case 1:
                                canSpawnSpawners = true;
                                transform.position = Vector2.MoveTowards(transform.position, spawnPoints[1].position, activeSpeed * Time.deltaTime);

                                if (transform.position == spawnPoints[1].position)
                                {
                                    Instantiate(spawner, transform.position, Quaternion.identity);
                                    currentSpawnPoint = 2;
                                }

                                break;
                            case 2:
                                canSpawnSpawners = true;
                                transform.position = Vector2.MoveTowards(transform.position, spawnPoints[2].position, activeSpeed * Time.deltaTime);

                                if (transform.position == spawnPoints[2].position)
                                {
                                    Instantiate(spawner, transform.position, Quaternion.identity);
                                    currentSpawnPoint = 3;
                                }

                                break;
                            case 3:
                                transform.position = Vector2.MoveTowards(transform.position, spawnPoints[3].position, activeSpeed * Time.deltaTime);

                                if (transform.position == spawnPoints[3].position)
                                {
                                    Instantiate(spawner, transform.position, Quaternion.identity);
                                    currentSpawnPoint = 0;
                                    activeSpeed = speed;
                                    currentState = Phase1.Chase;
                                    counter = chaseTIme;
                                    canSpawnSpawners = false;
                                }

                                break;

                        }
                    }
                    else
                    {
                        currentState = Phase1.Chase;
                        counter = chaseTIme;
                    }

                    break;

                case Phase1.Break:

                    ChangeAnimationState("SpookyWood_Idle");

                    if (counter > 0)
                    {
                        counter -= Time.deltaTime;
                    }
                    else
                    {
                        currentState = Phase1.SpawnEnemies;
                    }

                    break;

                case Phase1.Chase:

                    ChangeAnimationState("SpookyWood_Walk");

                    if (counter > 0)
                    {
                        counter -= Time.deltaTime;
                        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, activeSpeed * Time.deltaTime);
                    }
                    else
                    {
                        counter = chaseWithBothHandsShootingTime;
                        currentState = Phase1.ChaseWithBothHandsShooting;
                    }

                    break;
            }
        }

        else
        {
            if(hasCounterBeenSet == false)
            {
                counter = phase2IntroTime;
                shootCounter = breakBetweenBigBallsPhase2;
                activeSpeed = phase2Speed;
                hasCounterBeenSet = true;
            }

            switch (currentPhase2State)
            {
                case Phase2.Intro:

                    ChangeAnimationState("SpookyWood_Phase2Intro");

                    if (counter > 0)
                    {
                        counter -= Time.deltaTime;
                    }
                    else
                    {
                        currentPhase2State = Phase2.Stay;
                        counter = phase2StayTime;
                    }

                    break;

                case Phase2.Stay:

                    ChangeAnimationState("SpookyWood_Phase2Idle");

                    if (counter > 0)
                    {
                        counter -= Time.deltaTime;
                    }
                    else
                    {
                        currentPhase2State = Phase2.ChaseWithBigBall;
                        counter = phase2ChaseBigBallTime;
                    }

                    break;

                case Phase2.ChaseWithBigBall:

                    ChangeAnimationState("SpookyWood_Phase2Walk");

                    transform.position = Vector2.MoveTowards(transform.position, player.transform.position, activeSpeed * Time.deltaTime);

                    if (counter > 0)
                    {
                        counter -= Time.deltaTime;

                        if(hasShootBigBall == false)
                        {
                            Instantiate(bigBallPrefab, transform.position, Quaternion.identity);
                            hasShootBigBall = true;
                        }

                        if (shootCounter > 0)
                        {
                            shootCounter -= Time.deltaTime;
                        }
                        else
                        {
                            hasShootBigBall = false;
                            shootCounter = breakBetweenBigBallsPhase2;
                        }

                    }
                    else
                    {
                        hasShootBigBall = false;
                        leftHand.rotation = Quaternion.identity;
                        rightHand.rotation = Quaternion.identity;
                        currentPhase2State = Phase2.StayAndShootBombs;
                        counter = phase2StayAndShootBombsTime;
                        shootCounter = breaksBetweenBombsPhase2;
                    }

                    break;

                case Phase2.StayAndShootBombs:

                    ChangeAnimationState("SpookyWood_Phase2Idle");

                    if (counter > 0)
                    {
                        counter -= Time.deltaTime;

                        if (shootCounter > 0)
                        {
                            shootCounter -= Time.deltaTime;
                        }
                        else
                        {
                            Instantiate(phase2Bomb, transform.position, Quaternion.identity);
                            shootCounter = breaksBetweenBombsPhase2;
                        }

                    }
                    else
                    {
                        shootCounter = breakBetweenBigBallsPhase2;

                        counter = phase2BreakTime;
                        currentPhase2State = Phase2.Break;
                    }

                    break;

                case Phase2.SpawnEnemies:

                    ChangeAnimationState("SpookyWood_Phase2Walk");

                    if (spawnersList.Count <= 0 || canSpawnSpawners)
                    {
                        activeSpeed = phase2GoingToSpawnPointsSpeed;

                        switch (currentSpawnPoint)
                        {
                            case 0:
                                canSpawnSpawners = true;
                                transform.position = Vector2.MoveTowards(transform.position, spawnPoints[0].position, activeSpeed * Time.deltaTime);

                                if (transform.position == spawnPoints[0].position)
                                {
                                    Instantiate(spawner, transform.position, Quaternion.identity);
                                    currentSpawnPoint = 1;
                                }

                                break;

                            case 1:
                                canSpawnSpawners = true;
                                transform.position = Vector2.MoveTowards(transform.position, spawnPoints[1].position, activeSpeed * Time.deltaTime);

                                if (transform.position == spawnPoints[1].position)
                                {
                                    Instantiate(spawner, transform.position, Quaternion.identity);
                                    currentSpawnPoint = 2;
                                }

                                break;
                            case 2:
                                canSpawnSpawners = true;
                                transform.position = Vector2.MoveTowards(transform.position, spawnPoints[2].position, activeSpeed * Time.deltaTime);

                                if (transform.position == spawnPoints[2].position)
                                {
                                    Instantiate(spawner, transform.position, Quaternion.identity);
                                    currentSpawnPoint = 3;
                                }

                                break;
                            case 3:
                                transform.position = Vector2.MoveTowards(transform.position, spawnPoints[3].position, activeSpeed * Time.deltaTime);

                                if (transform.position == spawnPoints[3].position)
                                {
                                    Instantiate(spawner, transform.position, Quaternion.identity);
                                    currentSpawnPoint = 0;
                                    activeSpeed = phase2Speed;
                                    currentPhase2State = Phase2.BowAttack;
                                    counter = phase2BowAttackTime;
                                    canSpawnSpawners = false;
                                    shootCounter = breaksBetweenBowAttacksPhase2;
                                }

                                break;

                        }
                    }
                    else
                    {
                        currentPhase2State = Phase2.BowAttack;
                        counter = phase2BowAttackTime;
                        shootCounter = breaksBetweenBowAttacksPhase2;
                    }

                    break;

                case Phase2.Break:

                    ChangeAnimationState("SpookyWood_Phase2Idle");

                    if (counter > 0)
                    {
                        counter -= Time.deltaTime;
                    }
                    else
                    {
                        currentPhase2State = Phase2.SpawnEnemies;
                    }

                    break;

                case Phase2.Chase:

                    ChangeAnimationState("SpookyWood_Phase2Walk");

                    if (counter > 0)
                    {
                        counter -= Time.deltaTime;
                        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, activeSpeed * Time.deltaTime);
                    }
                    else
                    {
                        counter = phase2ChaseBigBallTime;
                        currentPhase2State = Phase2.ChaseWithBigBall;
                    }

                    break;

                case Phase2.BowAttack:

                    Vector2 direction = player.transform.position - transform.position;
                    direction.Normalize();

                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    BowAttackPoint.rotation = Quaternion.Euler(0, 0 ,angle);

                    transform.position = Vector2.MoveTowards(transform.position, player.transform.position, activeSpeed * Time.deltaTime);

                    if(counter > 0)
                    {
                        counter -= Time.deltaTime;
                        if (shootCounter > 0)
                        {
                            shootCounter -= Time.deltaTime;
                        }
                        else
                        {
                            foreach(Transform t in AllBowAttacksFirePoints)
                            {
                                Instantiate(bowAttackBullet, t.position, t.rotation);
                            }

                            shootCounter = breaksBetweenBowAttacksPhase2;
                        }
                    }
                    else
                    {
                        shootCounter = breakBetweenBigBallsPhase2;
                        counter = phase2ChaseBigBallTime;
                        currentPhase2State = Phase2.ChaseWithBigBall;

                    }



                    break;
            }
        }
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerBullet")
        {
            Destroy(collision.gameObject);
            sr.material = BossClass.instance.matWhite;
            if (BossClass.instance.Health <= 0)
            {
                BossRoom.instance.BossDead();
                Instantiate(BossClass.instance.explosionWhenDie, transform.position, transform.rotation);
                Destroy(gameObject);

            }
            else
            {
                Invoke("ResetMaterial", 0.1f); //jak dlugo ma byc bialy
            }
        }

        if (collision.tag == "Player")
        {
            if (SAYController.instance.isInvincible == false)
            {
                CameraShaker.ShakeStrong(.20f, .35f);
                SAYPlayerHealthController.instance.DamagePlayer();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {

            if (SAYController.instance.isInvincible == false)
            {
                CameraShaker.ShakeStrong(.20f, .35f);
                SAYPlayerHealthController.instance.DamagePlayer();
            }

        }
    }

    void ResetMaterial()
    {
        sr.material = matDefault;
    }

    void ChangeAnimationState(string newState)
    {
        if (currentAnimationState == newState) return;

        animator.Play(newState);

        currentAnimationState = newState;
    }
}
