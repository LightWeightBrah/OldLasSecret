using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandMageBoss : MonoBehaviour
{
    enum Phase1 { Intro, Idle, GatlingGun, Dash };
    enum Phase2 { Intro, Idle, NightStart, NightAttack, GoVisible, Dash };
    Phase1 currentState = Phase1.Intro;
    Phase2 phase2CurrentState = Phase2.Intro;
    [Header("Variables")]
    public float normalSpeed;
    public float speedWhenGuntlingGun;
    public float dashSpeed;
    public float phase2DashSpeed;
    public float phase2GoVisible;

    float activeSpeed;

    [Header("Times")]
    public float introTime;
    public float gatlingGunTime;
    public float idleTime;

    public float phase2introTime;
    public float phase2IdleTime;
    public float phase2NightStartTime;
    public float phase2NightAttackTime;
    public float phase2DashTime;

    [Header("GutlingGun")]
    public GameObject gatlingGunBullet;
    public Transform[] gatlingGunFirePoints;
    public float gatlingGunShootingBreaks;
    [Header("Idle")]
    public Transform[] allFirePoints;
    public float waitTimeBeforeShooting;
    float waitTimeCounter;
    bool hasShootCircle;
    [Header("Dash")]
    public GameObject bulletRight;
    public float dashTime;
    public Transform[] dashMovePoints;
    public float dashBreaks;
    float dashBreaksCounter;
    bool hasPickedPoint;
    int randomMovePoint;
    bool dashCircle;

    [Header("Phase2")]
    public float bookVisibleSpeed;
    bool shouldFade = true;
    public SpriteRenderer book;
    public SpriteRenderer bodySR;

    public GameObject bossBulletLeft;

    public GameObject nightPhaseFirePointLeft;
    public GameObject nightPhaseFirePointRight;

    public float speedToMoveFirePoint;

    bool hasCounterBeenSet;

    float phase2NightShootCounter;
    public float nightShootAttackBreaks;

    [Header("Phase2 Dash")]
    public float phase2DashBreaks;


    float shootCounter;
    float counter;

    GameObject player;

    public GameObject bigBall;


    Material matDefault;

    public BoxCollider2D collider;

    void Start()
    {
        matDefault = bodySR.material;

        dashBreaksCounter = dashBreaks;
        activeSpeed = normalSpeed;
        waitTimeCounter = waitTimeBeforeShooting;
        player = GameObject.FindGameObjectWithTag("Player");
        counter = introTime;
    }

    void Update()
    {
        if (BossClass.instance.Health > 1000)
        {
            switch (currentState)
            {
                case Phase1.Intro:
                    if (BossClass.instance.playIntro)
                    {
                        if (counter > 0)
                        {
                            counter -= Time.deltaTime;
                        }
                        else
                        {
                            currentState = Phase1.GatlingGun;
                            counter = gatlingGunTime;
                        }
                    }

                    break;

                case Phase1.GatlingGun:
                    if (counter > 0)
                    {
                        counter -= Time.deltaTime;

                        activeSpeed = speedWhenGuntlingGun;

                        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, activeSpeed * Time.deltaTime);

                        if (shootCounter > 0)
                        {
                            shootCounter -= Time.deltaTime;
                        }
                        else
                        {
                            foreach (Transform t in gatlingGunFirePoints)
                            {
                                Instantiate(gatlingGunBullet, t.position, t.rotation);
                            }
                            shootCounter = gatlingGunShootingBreaks;
                        }
                    }
                    else
                    {
                        currentState = Phase1.Idle;
                        counter = idleTime;
                    }
                    break;
                case Phase1.Idle:
                    if (counter > 0)
                    {
                        counter -= Time.deltaTime;

                        waitTimeCounter -= Time.deltaTime;

                        if (waitTimeCounter < 0)
                        {
                            if (hasShootCircle == false)
                            {
                                foreach (Transform t in allFirePoints)
                                {
                                    Instantiate(gatlingGunBullet, t.position, t.rotation);
                                }
                                hasShootCircle = true;
                            }
                        }
                    }
                    else
                    {
                        waitTimeCounter = waitTimeBeforeShooting;
                        hasShootCircle = false;
                        currentState = Phase1.Dash;
                        counter = dashTime;
                    }
                    break;
                case Phase1.Dash:

                    if (counter > 0)
                    {
                        counter -= Time.deltaTime;

                        if (dashBreaksCounter > 0)
                        {
                            if (dashCircle == false)
                            {
                                foreach (Transform t in allFirePoints)
                                {
                                    Instantiate(bulletRight, t.position, t.rotation);
                                }
                                dashCircle = true;
                            }

                            dashBreaksCounter -= Time.deltaTime;
                        }
                        else
                        {
                            activeSpeed = dashSpeed;

                            if (hasPickedPoint == false)
                            {
                                randomMovePoint = Random.Range(0, dashMovePoints.Length);
                                hasPickedPoint = true;
                            }


                            transform.position = Vector2.MoveTowards(transform.position, dashMovePoints[randomMovePoint].position, activeSpeed * Time.deltaTime);
                            if (Vector2.Distance(transform.position, dashMovePoints[randomMovePoint].position) < 0.1f)
                            {
                                Debug.Log("okay");
                                dashBreaksCounter = dashBreaks;
                                hasPickedPoint = false;
                                dashCircle = false;
                            }
                        }
                    }
                    else
                    {
                        currentState = Phase1.GatlingGun;
                        counter = gatlingGunTime;
                    }

                    break;
            }

        }
        else
        {
            if (hasCounterBeenSet == false)
            {
                counter = phase2introTime;
                hasCounterBeenSet = true;
            }

            switch (phase2CurrentState)
            {
                case Phase2.Intro:
                    if (counter > 0)
                    {
                        counter -= Time.deltaTime;
                        if (shouldFade)
                        {
                            book.color = new Color(book.color.r, book.color.g, book.color.b, Mathf.MoveTowards(book.color.a, 1f, bookVisibleSpeed * Time.deltaTime));
                            if (book.color.a == 1f)
                            {
                                shouldFade = false;
                            }
                        }
                    }
                    else
                    {
                        hasPickedPoint = false;
                        dashCircle = false;
                        dashBreaksCounter = phase2DashBreaks;
                        phase2NightShootCounter = nightShootAttackBreaks;
                        counter = phase2NightStartTime;
                        phase2CurrentState = Phase2.NightStart;
                        shouldFade = true;
                    }
                    break;

                case Phase2.Idle:

                    break;
                case Phase2.NightStart:

                    if (counter > 0)
                    {
                        counter -= Time.deltaTime;

                        if (shouldFade)
                        {
                            book.color = new Color(book.color.r, book.color.g, book.color.b, Mathf.MoveTowards(book.color.a, 0f, bookVisibleSpeed * Time.deltaTime));
                            bodySR.color = new Color(bodySR.color.r, bodySR.color.g, bodySR.color.b, Mathf.MoveTowards(bodySR.color.a, 0f, bookVisibleSpeed * Time.deltaTime));

                            if (bodySR.color.a == 0f)
                            {
                                shouldFade = false;
                            }
                        }
                    }
                    else
                    {
                        collider.enabled = false;
                        shouldFade = true;
                        counter = phase2NightAttackTime;
                        phase2CurrentState = Phase2.NightAttack;
                    }
                    break;


                case Phase2.NightAttack:

                    if (counter > 0)
                    {
                        counter -= Time.deltaTime;

                        Vector2 tempPos = nightPhaseFirePointLeft.transform.position;

                        tempPos.y = Mathf.MoveTowards(nightPhaseFirePointLeft.transform.position.y, player.transform.position.y, speedToMoveFirePoint * Time.deltaTime);

                        Vector2 tempPosRight = nightPhaseFirePointRight.transform.position;

                        tempPosRight.y = Mathf.MoveTowards(nightPhaseFirePointRight.transform.position.y, player.transform.position.y, speedToMoveFirePoint * Time.deltaTime);

                        nightPhaseFirePointLeft.transform.position = tempPos;
                        nightPhaseFirePointRight.transform.position = tempPosRight;

                        if (phase2NightShootCounter > 0)
                        {
                            phase2NightShootCounter -= Time.deltaTime;
                        }
                        else
                        {
                            Instantiate(bulletRight, nightPhaseFirePointLeft.transform.position, nightPhaseFirePointLeft.transform.rotation);
                            Instantiate(bossBulletLeft, nightPhaseFirePointRight.transform.position, nightPhaseFirePointLeft.transform.rotation);

                            phase2NightShootCounter = nightShootAttackBreaks;
                        }
                    }
                    else
                    {
                        counter = phase2GoVisible;
                        phase2CurrentState = Phase2.GoVisible;
                    }

                    break;

                case Phase2.GoVisible:

                    if (counter > 0)
                    {
                        collider.enabled = true;

                        counter -= Time.deltaTime;

                        if (shouldFade)
                        {
                            book.color = new Color(book.color.r, book.color.g, book.color.b, Mathf.MoveTowards(book.color.a, 1f, bookVisibleSpeed * Time.deltaTime));
                            bodySR.color = new Color(bodySR.color.r, bodySR.color.g, bodySR.color.b, Mathf.MoveTowards(bodySR.color.a, 1f, bookVisibleSpeed * Time.deltaTime));

                            if (bodySR.color.a == 1f)
                            {
                                shouldFade = false;
                            }
                        }
                    }
                    else
                    {
                        counter = phase2DashTime;
                        phase2CurrentState = Phase2.Dash;
                        shouldFade = true;
                    }


                    break;

                case Phase2.Dash:

                    if (counter > 0)
                    {
                        counter -= Time.deltaTime;

                        if (dashBreaksCounter > 0)
                        {
                            if (dashCircle == false)
                            {
                                foreach (Transform t in allFirePoints)
                                {
                                    Instantiate(bulletRight, t.position, t.rotation);
                                }

                                Instantiate(bigBall, transform.position, transform.rotation);

                                dashCircle = true;
                            }

                            dashBreaksCounter -= Time.deltaTime;
                        }
                        else
                        {
                            activeSpeed = phase2DashSpeed;

                            if (hasPickedPoint == false)
                            {
                                randomMovePoint = Random.Range(0, dashMovePoints.Length);
                                hasPickedPoint = true;
                            }


                            transform.position = Vector2.MoveTowards(transform.position, dashMovePoints[randomMovePoint].position, activeSpeed * Time.deltaTime);
                            if (Vector2.Distance(transform.position, dashMovePoints[randomMovePoint].position) < 0.1f)
                            {
                                Debug.Log("okay");
                                dashBreaksCounter = dashBreaks;
                                hasPickedPoint = false;
                                dashCircle = false;
                            }
                        }
                    }
                    else
                    {
                        phase2CurrentState = Phase2.NightStart;
                        counter = phase2NightStartTime;
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
            bodySR.material = BossClass.instance.matWhite;
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
        bodySR.material = matDefault;
    }
}

