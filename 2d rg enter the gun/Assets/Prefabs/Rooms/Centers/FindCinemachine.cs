using System.Collections;
using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class FindCinemachine : MonoBehaviour
{
    public bool isBossRoom;

    private GameObject Player;

    public CinemachineVirtualCamera cinemachine;

    GameObject mainCamera;

    public bool isInTown;
    private void Awake()
    {
        //cinemachine = gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
        Player = GameObject.FindGameObjectWithTag("Player");

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

    }

    IEnumerator SetCameraToBoss()
    {
        cinemachine.gameObject.SetActive(true);
        cinemachine.Follow = GameObject.FindGameObjectWithTag("Boss").transform;
        yield return new WaitForSeconds(0.5f);
        BossClass.instance.playIntro = true;


        SAYController.instance.canMove = false;
        SAYController.instance.StopMoving();

        //CameraController.instance.ChangeTarget(transform);

        yield return new WaitForSeconds(2.5f);

        SAYUIController.instance.bossBarAll.gameObject.SetActive(true);

        SAYController.instance.canMove = true;

        cinemachine.Follow = Player.transform;
    }
    


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && isBossRoom)
        {
            StartCoroutine(SetCameraToBoss());
            CinemachineShake.instance.cinemachineVirtualCamera = cinemachine;//moze trzeba bedzie usunac nie wiem
        }

        else if (other.tag == "Player")
        {
            //cinemachine

            cinemachine.gameObject.SetActive(true);
            cinemachine.Follow = Player.transform;
            if(!isInTown)
            {
                CinemachineShake.instance.cinemachineVirtualCamera = cinemachine;
            }

            // CameraController.instance.ChangeTarget(transform);

        }

        

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {

            //cinemachine
            cinemachine.Follow = null;
            cinemachine.gameObject.SetActive(false);
        }
    }
}
