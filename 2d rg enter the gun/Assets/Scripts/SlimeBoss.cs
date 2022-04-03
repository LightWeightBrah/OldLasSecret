using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBoss : MonoBehaviour
{

    public enum Phase1 { Intro, Idle, Shooting, Jump, Landing };
    public enum Phase2 { Intro, Idle, Shooting, Jump, Landing };

    [Header("Phase1")]

    public GameObject projectile;

    public float speed;
    public float triggerDistance;
    public float introTime;
    public int howManyTimesToJump;

    public float jumpTime;
    public float landingTime;
    public float shootPhase1Time;
    public float phase1Idle;

    [Header("Phase1 Shooting")]

    public float breaksBetweenShoots;
    public Transform[] landingShootFirePoints;
    public Transform[] shootingFirePoints;

    public float shootingPhaseEndBreak;

    public float shootPhaseBegginBreak;



    //private
    float activeSpeed;
    Animator animator;
    string currentAnimationState;
    public Phase1 currentState = Phase1.Intro;
    public Phase2 phase2CurrentState = Phase2.Intro;
    GameObject player;

    public float counter;
    int currentJumps;
    float shootCounter;
    float shootingPhaseBreakBetweenPhaseCounter;

    float shootPhaseBegginBreakCounter;

    bool dontShoot;





    [Header("Phase2")]
    public float Phase2speed;
    public float Phase2introTime;
    public int Phase2howManyTimesToJump;
    public float Phase2jumpTime;
    public float Phase2landingTime;

    public float Phase2Idle;

    [Header("Phase2 Shooting")]

    public Transform[] Phase2landingShootFirePoints;
    public Transform[] Phase2shootingFirePoints;

    public float Phase2shootPhaseBegginBreak;

    public float Phase2breaksBetweenShoots;
    public float Phase2shootPhase1Time;

    public float Phase2shootingPhaseEndBreak;

    bool hasCounterBeenSet;

    public Transform allFirePoints;

    public float speedToRotateAroundSpiral;


    public Material matWhite;
    Material matDefault;
    SpriteRenderer sr;

    bool canShakeCamera = true;

    void Start()
    {

        sr = GetComponentInChildren<SpriteRenderer>();
        matDefault = sr.material;
        activeSpeed = speed;

        //end break
        shootingPhaseBreakBetweenPhaseCounter = shootPhase1Time - shootingPhaseEndBreak;
        //beggin break
        shootPhaseBegginBreakCounter = shootPhaseBegginBreak;


        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        counter = introTime;
    }

    void Update()
    {
        if (BossClass.instance.Health > 600)
        {


            switch (currentState)
            {
                case Phase1.Intro:
                    if (BossClass.instance.playIntro)
                    {
                        ChangeAnimationState("SlimeBoss_Intro");
                        if (counter > 0)
                        {
                            counter -= Time.deltaTime;
                        }
                        else
                        {
                            currentState = Phase1.Idle;
                            counter = phase1Idle;
                        }
                    }
                    break;

                case Phase1.Idle:
                    ChangeAnimationState("SlimeBoss_Idle");

                    if (counter > 0)
                    {
                        counter -= Time.deltaTime;
                    }
                    else
                    {
                        counter = jumpTime;
                        currentState = Phase1.Jump;
                    }
                    break;

                case Phase1.Shooting:

                    ChangeAnimationState("SlimeBoss_Idle");

                    if (shootPhaseBegginBreakCounter > 0)
                    {
                        shootPhaseBegginBreakCounter -= Time.deltaTime;
                    }
                    else
                    {
                        if (counter > 0)
                        {
                            counter -= Time.deltaTime;

                            shootingPhaseBreakBetweenPhaseCounter -= Time.deltaTime;

                            //beggin break; 1 
                            if (shootPhaseBegginBreakCounter < 0)
                            {
                                if (shootingPhaseBreakBetweenPhaseCounter > 0)
                                {
                                    if (shootCounter > 0)
                                    {
                                        shootCounter -= Time.deltaTime;
                                    }
                                    else
                                    {
                                        foreach (Transform t in shootingFirePoints)
                                        {
                                            Instantiate(projectile, t.position, t.rotation);
                                        }
                                        shootCounter = breaksBetweenShoots;
                                    }
                                }

                            }
                            else
                            {
                                shootPhaseBegginBreakCounter -= Time.deltaTime;
                            }

                        }
                        else
                        {
                            shootingPhaseBreakBetweenPhaseCounter = shootPhase1Time - shootingPhaseEndBreak;
                            //beggin break
                            shootPhaseBegginBreakCounter = shootPhaseBegginBreak;

                            counter = jumpTime;
                            currentState = Phase1.Jump;
                        }
                    }



                    break;

                case Phase1.Jump:
                    ChangeAnimationState("SlimeBoss_Jump");

                    if (counter > 0)
                    {
                        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, activeSpeed * Time.deltaTime);
                        counter -= Time.deltaTime;
                    }
                    else
                    {
                        currentState = Phase1.Landing;
                        counter = landingTime;
                    }


                    break;

                case Phase1.Landing:

                    if (canShakeCamera)
                    {
                        CameraShaker.ShakeStrong(landingTime, .15f);
                        canShakeCamera = false;
                    }

                    if (dontShoot == false)
                    {
                        foreach (Transform t in landingShootFirePoints)
                        {
                            Instantiate(projectile, t.position, t.rotation);
                        }

                        dontShoot = true;
                    }

                    ChangeAnimationState("SlimeBoss_Landing");




                    if (counter > 0)
                    {
                        counter -= Time.deltaTime;
                    }
                    else
                    {
                        dontShoot = false;


                        canShakeCamera = true;

                        if (currentJumps == howManyTimesToJump)
                        {
                            currentJumps = 0;
                            counter = shootPhase1Time;
                            currentState = Phase1.Shooting;
                        }
                        else
                        {
                            currentState = Phase1.Jump;
                            counter = jumpTime;
                            currentJumps++;

                        }
                    }

                    break;
            }
        }
        else
        {
            if (hasCounterBeenSet == false)
            {
                counter = Phase2introTime;
                hasCounterBeenSet = true;
            }

            switch (phase2CurrentState)
            {
                case Phase2.Intro:
                    ChangeAnimationState("SlimeBoss_Stage 2 Intro");

                    if (canShakeCamera)
                    {
                        CameraShaker.ShakeStrong(Phase2introTime, .35f);
                        canShakeCamera = false;
                    }

                    if (counter > 0)
                    {
                        counter -= Time.deltaTime;
                    }
                    else
                    {
                        canShakeCamera = true;

                        activeSpeed = Phase2speed;

                        counter = Phase2Idle;
                        shootCounter = Phase2breaksBetweenShoots;

                        shootingPhaseBreakBetweenPhaseCounter = Phase2shootPhase1Time - Phase2shootingPhaseEndBreak;

                        shootPhaseBegginBreakCounter = Phase2shootPhaseBegginBreak;





                        phase2CurrentState = Phase2.Idle;
                    }

                    break;

                case Phase2.Idle:

                    ChangeAnimationState("SlimeBoss_Idle Stage 2");

                    if (counter > 0)
                    {
                        counter -= Time.deltaTime;
                    }
                    else
                    {
                        counter = Phase2jumpTime;
                        phase2CurrentState = Phase2.Jump;
                    }
                    break;

                case Phase2.Shooting:

                    ChangeAnimationState("SlimeBoss_Idle Stage 2");

                    //rotate 
                    allFirePoints.Rotate(Vector3.forward * speedToRotateAroundSpiral * Time.deltaTime);

                    if (shootPhaseBegginBreakCounter > 0)
                    {
                        shootPhaseBegginBreakCounter -= Time.deltaTime;
                    }
                    else
                    {
                        if (counter > 0)
                        {
                            counter -= Time.deltaTime;

                            shootingPhaseBreakBetweenPhaseCounter -= Time.deltaTime;

                            if (shootPhaseBegginBreakCounter < 0)
                            {
                                //shootingPhaseBreakBetweenPhaseCounter;

                                if (shootingPhaseBreakBetweenPhaseCounter > 0)
                                {
                                    if (shootCounter > 0)
                                    {
                                        shootCounter -= Time.deltaTime;
                                    }
                                    else
                                    {
                                        foreach (Transform t in Phase2shootingFirePoints)
                                        {
                                            Instantiate(projectile, t.position, t.rotation);
                                        }
                                        shootCounter = Phase2breaksBetweenShoots;
                                    }
                                }


                            }
                            else
                            {
                                shootingPhaseBreakBetweenPhaseCounter -= Time.deltaTime;
                            }

                        }
                        else
                        {
                            shootingPhaseBreakBetweenPhaseCounter = Phase2shootPhase1Time - Phase2shootingPhaseEndBreak;

                            shootPhaseBegginBreakCounter = Phase2shootPhaseBegginBreak;

                            counter = Phase2jumpTime;
                            allFirePoints.rotation = Quaternion.identity;
                            phase2CurrentState = Phase2.Jump;
                        }
                    }



                    break;

                case Phase2.Jump:
                    ChangeAnimationState("SlimeBoss_Stage 2 Jump");

                    if (counter > 0)
                    {
                        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, activeSpeed * Time.deltaTime);
                        counter -= Time.deltaTime;
                    }
                    else
                    {
                        phase2CurrentState = Phase2.Landing;
                        counter = Phase2landingTime;
                    }


                    break;

                case Phase2.Landing:

                    if (canShakeCamera)
                    {
                        CameraShaker.ShakeStrong(landingTime, .15f);
                        canShakeCamera = false;
                    }

                    if (dontShoot == false)
                    {
                        foreach (Transform t in Phase2landingShootFirePoints)
                        {
                            Instantiate(projectile, t.position, t.rotation);
                        }

                        dontShoot = true;
                    }

                    ChangeAnimationState("SlimeBoss_Landing Stage 2");




                    if (counter > 0)
                    {
                        counter -= Time.deltaTime;
                    }
                    else
                    {
                        dontShoot = false;

                        canShakeCamera = true;

                        if (currentJumps == Phase2howManyTimesToJump)
                        {
                            currentJumps = 0;
                            counter = Phase2shootPhase1Time;
                            phase2CurrentState = Phase2.Shooting;
                        }
                        else
                        {
                            phase2CurrentState = Phase2.Jump;
                            counter = Phase2jumpTime;
                            currentJumps++;

                        }
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
}