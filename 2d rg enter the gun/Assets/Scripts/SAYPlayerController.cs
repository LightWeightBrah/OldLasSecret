using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAYPlayerController : MonoBehaviour
{
    private static SAYPlayerController me;
    private static List<TreeEnemy> enemies;

    public float moveSpeed;
    public Rigidbody2D theRB;
    Vector2 movement;
    public Animator animator;
    public Animator swordAnimator;
    public SpriteRenderer PlayerBody;
    public SpriteRenderer Sword;

    public float enemyDistanceForSword = 1f;

    bool isSwordOnRight = true;

    public GameObject sword;
    public Transform right;
    public Transform left;

    public static void RegisterEnemy(TreeEnemy enemy)
    {
        if (enemies == null) enemies = new List<TreeEnemy>();
        enemies.Add(enemy);
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        bool stopping = (movement.x == 0 && movement.y == 0);
        movement.Normalize();

        // Smooth the anim
        theRB.velocity = (theRB.velocity + 7 * movement * moveSpeed) * .125f;

        if (stopping && theRB.velocity == Vector2.zero)
        {
            animator.SetBool("Idle", true);
            animator.speed = 1;
        }
        else
        {
            animator.SetBool("Idle", false);
            animator.speed = theRB.velocity.magnitude / moveSpeed;
        }

        if (movement.x < 0)
        {
            PlayerBody.flipX = true;
            Sword.flipX = true;
        }
        if (movement.x > 0)
        {
            PlayerBody.flipX = false;
            Sword.flipX = false;
        }
        /*
        if (Input.GetMouseButtonDown(0))
        {
            float xm = (2f * Input.mousePosition.x / Screen.width) - 1f;
            float ym = (2f * Input.mousePosition.y / Screen.height) - 1f;

            if (Mathf.Abs(xm) > (Mathf.Abs(ym)))
            { // Horizontal
                if (xm >= 0)
                {
                    PlayerBody.flipX = false;
                    swordAnimator.Play("Sword Swing");
                    HitEnemy(1, 0);
                }
                else
                {
                    PlayerBody.flipX = true;
                    swordAnimator.Play("Sword Swing Left");
                    HitEnemy(-1, 0);
                }
            }
            else if (ym > 0)
            {
                swordAnimator.Play("Sword Swing Top");
                HitEnemy(0, 1);
            }
            else
            {
                swordAnimator.Play("Sword Swing Down");
                HitEnemy(0, -1);
            }
        }
        */

        if(Input.GetMouseButtonDown(0))
        {
            if(isSwordOnRight)
            {
                sword.transform.localPosition = left.localPosition;

                isSwordOnRight = false;
            }

            else
            {
                sword.transform.localPosition = right.localPosition;
                
                isSwordOnRight = true;
            }
        }

        /*
        if (swordAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && isSwordOnRight == false)
        {  //If normalizedTime is 0 to 1 means animation is playing, if greater than 1 means finished
            Debug.Log("not playing");

            sword.transform.SetParent(left);
            sword.transform.position = Vector3.zero;
        }
        else if (swordAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && isSwordOnRight == true)
        {  //If normalizedTime is 0 to 1 means animation is playing, if greater than 1 means finished
            Debug.Log("not playing");

            sword.transform.SetParent(right);
            sword.transform.position = Vector3.zero;
        }*/
    }

  

    void HitEnemy(int xm, int ym)
    {
        TreeEnemy enemy = null;
        foreach (TreeEnemy e in enemies)
        {
            if (xm == -1 && e.transform.position.x < transform.position.x)
            {
                if (enemy == null || enemy.transform.position.x < e.transform.position.x)
                    enemy = e;
            }
            else if (xm == 1 && e.transform.position.x > transform.position.x)
            {
                if (enemy == null || enemy.transform.position.x > e.transform.position.x)
                    enemy = e;
            }
            else if (ym == -1 && e.transform.position.y < transform.position.y)
            {
                if (enemy == null || enemy.transform.position.y < e.transform.position.y)
                    enemy = e;
            }
            else if (ym == 1 && e.transform.position.y > transform.position.y)
            {
                if (enemy == null || enemy.transform.position.y > e.transform.position.y)
                    enemy = e;
            }
        }
        if (enemy == null) return; // No enemy

        if (Vector2.Distance(enemy.transform.position, transform.position) > enemyDistanceForSword) return; // Too far away


        Debug.Log("Hit enemy: " + enemy.name);
    }
}