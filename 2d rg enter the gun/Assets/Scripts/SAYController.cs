using UnityEngine;
using UnityEngine.SceneManagement;

public class SAYController : MonoBehaviour
{
    public static SAYController instance;

    [Header("Combat")]
    public SpriteRenderer bodySr;
    public Camera cam;
    //public GameObject Dot;
    public Animator SwordAnim;
    public Transform Sword;
    public Transform firePoint;
    public GameObject[] swordEffect;
    public GameObject[] bullet;
    bool clockwise = true;
    public SpriteRenderer swordSr;

    public float timeBetweenAttacks;
    float attacksCounter;

    [Header("Move")]
    float activeMoveSpeed;
    public float moveSpeed;
    public Rigidbody2D theRB;
    Vector2 movement;
    public Animator PlayerAnimator;

    public float angle;

    public float dashSpeed = 8f, dashLength = 0.5f, dashCooldawn = 1f, dashInvincibility = 0.5f;
    [HideInInspector]
    public float dashCounter;
    float dashCoolCounter;

    public Material matWhite;
    public Material matRed;
    public Material matDefault;

    public bool canMove = true;

    public bool isInTown;

    public Color defaultColor;

    public Color redColor;

    public Sprite[] swordsUpgrades;

    int currentSword;

    [HideInInspector]
    public bool isInvincible;

    private void Awake()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        /*switch(currentSword)
        {
            case swordsToHold.Default:
                swordSr.sprite = swordsUpgrades[(int)swordsToHold.Default];
                break;

            case swordsToHold.Upgraded:
                swordSr.sprite = swordsUpgrades[(int)swordsToHold.Upgraded];
                break;
        }*/


        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Town"))
        {
            isInTown = true;
            swordSr.color = new Color(SAYController.instance.swordSr.color.r, SAYController.instance.swordSr.color.g, SAYController.instance.swordSr.color.b, 0f);
        }
        else
        {
            swordSr.color = new Color(SAYController.instance.swordSr.color.r, SAYController.instance.swordSr.color.g, SAYController.instance.swordSr.color.b, 1f);
            isInTown = false;
        }

        defaultColor = bodySr.color;
        matDefault = bodySr.material;
        activeMoveSpeed = moveSpeed;
        SwordAnim.Play("Sword anticlockwise");
    }
    private void Update()
    {
        if (canMove && !SAYLevelManager.instance.isPaused)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            bool stopping = (movement.x == 0 && movement.y == 0);
            movement.Normalize();

            // Smooth the anim
            theRB.velocity = (theRB.velocity + 7 * movement * activeMoveSpeed) * .125f;

            if (stopping && theRB.velocity == Vector2.zero)
            {
                PlayerAnimator.SetBool("Idle", true);
                PlayerAnimator.speed = 1;
            }
            else
            {
                PlayerAnimator.SetBool("Idle", false);
                PlayerAnimator.speed = theRB.velocity.magnitude / activeMoveSpeed;
            }
            /*
            if (movement.x < 0)
            {
                sr.flipX = true;

            }
            if (movement.x > 0)
            {
                sr.flipX = false;

            }
            */


            Vector2 direction = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            // direction.Normalize();
            //  Vector2 pos = transform.position;
            // pos -= direction * 4;

            // Dot.transform.position = pos;



            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Sword.rotation = Quaternion.Euler(0, 0, angle);
            bodySr.flipX = angle < -90 || angle > 90;


            if (isInTown == false)
            {
                if (attacksCounter > 0)
                {
                    attacksCounter -= Time.deltaTime;
                }
                else
                {
                    if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
                    {
                        Instantiate(swordEffect[currentSword], firePoint.position, Sword.rotation);
                        Instantiate(bullet[currentSword], firePoint.position, Sword.rotation);

                        attacksCounter = timeBetweenAttacks;


                        //StartCoroutine(CameraShaker.instance.Shake(0.15f, 0.4f));

                        if (clockwise)
                            SwordAnim.Play("Sword clockwise");
                        else
                            SwordAnim.Play("Sword anticlockwise");

                        clockwise = !clockwise;
                    }
                }
            }




            if (Input.GetKeyDown(KeyCode.Space))
            {
                //czy skonczyl sie cooldawn i czy skonczyles doskok
                if (dashCoolCounter <= 0 && dashCounter <= 0) //dashCounter is how long you dash
                {
                    activeMoveSpeed = dashSpeed;
                    dashCounter = dashLength;

                    PlayerAnimator.SetTrigger("dash");

                    if (isInTown == false)
                    {
                        swordSr.color = new Color(swordSr.color.r, swordSr.color.g, swordSr.color.b, 0f);
                    }

                    isInvincible = true;
                    //gameObject.GetComponent<BoxCollider2D>().enabled = false;

                    //PlayerHealthController.instance.MakeInvincible(dashInvincibility);//shortcut => cntrl r + r zamienia kazda zmienna 

                    //AudioManager.instance.PlaySFX(8);
                }

            }



            if (dashCounter > 0)
            {
                dashCounter -= Time.deltaTime;
                if (dashCounter <= 0) //koniec doskoku
                {
                    isInvincible = false;
                    //gameObject.GetComponent<BoxCollider2D>().enabled = true;

                    if (isInTown == false)
                    {
                        swordSr.color = new Color(swordSr.color.r, swordSr.color.g, swordSr.color.b, 1f);
                    }
                    activeMoveSpeed = moveSpeed; //ustawiam poczatkowy speed
                    dashCoolCounter = dashCooldawn; //ustawiam cooldawn
                }
            }

            if (dashCoolCounter > 0)
            {
                dashCoolCounter -= Time.deltaTime;
            }
        }



    }

    public void BuyWeaponUpgrade(int swordIndex)
    {
        currentSword = swordIndex;

        switch (currentSword)
        {
            case 0:
                swordSr.sprite = swordsUpgrades[swordIndex];
                break;

            case 1:
                swordSr.sprite = swordsUpgrades[swordIndex];
                break;
            case 2:
                swordSr.sprite = swordsUpgrades[swordIndex];
                break;
            case 3:
                swordSr.sprite = swordsUpgrades[swordIndex];
                break;
        }
    }

    public void StopMoving()
    {
        PlayerAnimator.SetBool("Idle", true);
        theRB.velocity = Vector2.zero;
    }


    void ResetMaterial()
    {
        bodySr.material = matDefault;
    }

}
