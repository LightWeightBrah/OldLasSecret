using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool closeWhenEntered/*, openWhenEnemiesCleared*/;

    public GameObject[] doors;

    //public List<GameObject> enemies = new List<GameObject>(); //list cause on array when the game 
    //is running you can't easy adjust the size of array

    [HideInInspector]
    public bool roomActive;

    public GameObject mapHider;

    private GameObject Player;

    public GameObject playerIcon;

    private void Awake()
    {
        //cinemachine = gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
        Player = GameObject.FindGameObjectWithTag("Player");

    }
    void Start()
    {
        
    }

    void Update()
    {
        /*
        if(enemies.Count > 0 && roomActive && openWhenEnemiesCleared)//list.Count == array.Length
        {
            for(int i = 0; i < enemies.Count; i++) //for a nie foreach bo chcemy wiedziec dokladnie ktory zniszczony
            {
                if (enemies[i] == null)  //jezeli przeciwnik zniszczony
                {
                    enemies.RemoveAt(i); //usuwa z listy

                    i--;
                    //dekrementacja bo zalozmy ze mamy 3 elementy ,element 1 jest pusty i usuwamy go ,wtedy
                    //dlugosc listy zmienjszy sie do 2 , wiec element o indeksie 2 staje sie 
                    //elementem o indeksie 1 ALE JUZ MIELIMSY ITERACJE Z INDEKSEM 1 WIEC SIE MUSZE COFNAC
                }
            }

            if(enemies.Count == 0) //jezeli nie ma przeciwnikow na liscie
            {
                foreach (GameObject door in doors)
                {
                    door.SetActive(false);

                    closeWhenEntered = false;
                }
            }
        }
        */
    }

    public void OpenDoors()
    {
        foreach (GameObject door in doors)
        {
            door.SetActive(false);

            closeWhenEntered = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            playerIcon.gameObject.SetActive(true);

            //cinemachine
           
            if(closeWhenEntered)
            {
                foreach(GameObject door in doors)
                {
                    door.SetActive(true);
                }
            }

            roomActive = true;

            mapHider.SetActive(false);
        } 
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            playerIcon.gameObject.SetActive(false);

            roomActive = false;
        }
    }
}
