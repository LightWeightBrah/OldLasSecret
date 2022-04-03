using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public float moveSpeed;

    public Transform target;

    public Camera mainCamera, bigMapCamera;

    bool bigMapActive;

    public bool isBossRoom;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
/*        if(isBossRoom)
        {
            target = PlayerController.instance.transform;
        }*/
    }

    void Update()
    {
       
        if(target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), moveSpeed * Time.deltaTime);
        }

        if(Input.GetKeyDown(KeyCode.M) && !isBossRoom)
        {
            if(!bigMapActive)
            {
                ActivateBigMap();
                //minimap
                //SAYUIController.instance.mapDisplay.SetActive(false);
                //minimap
                //SAYUIController.instance.canEnableMinimap = false;
            }
            else
            {
                DeactivateBigMap();
                //minimap
                //SAYUIController.instance.canEnableMinimap = true;

            }
        }

    }

    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ActivateBigMap()
    {
        if(!SAYLevelManager.instance.isPaused)
        {   
            bigMapActive = true;

            bigMapCamera.enabled = true; //.enabled acces component to enable or disable 
            mainCamera.enabled = false;

            SAYController.instance.canMove = false;

            Time.timeScale = 0f;

            //minimap
            //SAYUIController.instance.mapDisplay.SetActive(false);

            //SAYUIController.instance.bigMapText.SetActive(true);
        }
    }

    public void DeactivateBigMap()
    {
        if(!SAYLevelManager.instance.isPaused)
        {
            bigMapActive = false;

            bigMapCamera.enabled = false; //.enabled acces component to enable or disable 
            mainCamera.enabled = true;

            SAYController.instance.canMove = true;

            Time.timeScale = 1f;

            //minimap
            //SAYUIController.instance.mapDisplay.SetActive(false);
            SAYUIController.instance.bigMapText.SetActive(false);
        }
    }
}
