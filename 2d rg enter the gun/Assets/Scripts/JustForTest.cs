using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustForTest : MonoBehaviour
{
    public GameObject prefab;
    public GameObject prefab2;
    public GameObject prefab3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Instantiate(prefab);
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            Instantiate(prefab2);
        }

        if(Input.GetKeyDown(KeyCode.T))
        {
            Instantiate(prefab3);
        }
    }
}
