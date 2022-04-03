using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsInvisible : MonoBehaviour
{
    public float fadeSpeed = 2;
    bool fadeToNormal, fadeToInvisible;
    SpriteRenderer sr;
    float howMuchTransparent = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeToInvisible)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.MoveTowards(sr.color.a, 0f, fadeSpeed * Time.deltaTime));
            if (sr.color.a < howMuchTransparent)
            {
                fadeToInvisible = false;
            }
        }

        if (fadeToNormal)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.MoveTowards(sr.color.a, 1f, fadeSpeed * Time.deltaTime));
            if (sr.color.a == 1f)
            {
                fadeToNormal = false;
            }
        }
    }

    public void FadeToNormal()
    {
        fadeToNormal = true;
        fadeToInvisible = false;
    }

    public void FadeToInvisible()
    {
        fadeToNormal = false;
        fadeToInvisible = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" || other.tag == "Enemy")
        {
            FadeToInvisible();            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player" || other.tag == "Enemy")
        {
            FadeToNormal();
        }
    }
}
