using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderSpritesInScene : MonoBehaviour
{
    float distance = 25f;

    GameObject player;

    bool hasLooped;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if(player != null)
        {
            if (Vector2.Distance(transform.position, player.transform.position) > distance)
            {
                if (hasLooped == false)
                {
                    EnablingAndDisablingSpritesInScene(false);
                }
            }
            else
            {
                if (hasLooped == true)
                {
                    EnablingAndDisablingSpritesInScene(true);
                }

            }
        }
        
    }

    void EnablingAndDisablingSpritesInScene(bool shouldGoActive)
    {

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(shouldGoActive);
        }
        hasLooped = !hasLooped;

    }
}
