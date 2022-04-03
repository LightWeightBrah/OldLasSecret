using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorScript : MonoBehaviour
{
    public Room theRoom;

    public Transform target;

    public float HideDistance;

    public GameObject arrow;

    public bool isInTown;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            Destroy(gameObject);
        }

        if(!isInTown)
        {
            if (!theRoom.roomActive)
            {
                arrow.gameObject.SetActive(false);
                return;
            }
            else
            {
                arrow.gameObject.SetActive(true);
            }
        }
        
 
        if(target != null)
        {
            var dir = target.position - transform.position;

            if (dir.magnitude < HideDistance)
            {
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(false);
                }
            }
            else
            {
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(true);
                }

                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }

        

        
    }
}
