using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSortOrder : MonoBehaviour
{
    SpriteRenderer theSR;

    void Start()
    {
        theSR = GetComponentInChildren<SpriteRenderer>();

        if(theSR != null)
        {
            theSR.sortingOrder = Mathf.RoundToInt(transform.position.y * -100f); //dziala jak castowanie na z float na int (int) 
        }
    }

    void Update()
    {
        
    }
}
