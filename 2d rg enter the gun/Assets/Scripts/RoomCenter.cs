using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCenter : MonoBehaviour
{
    public Transform[] potentialSpawnPoints;

    public bool openWhenEnemiesCleared;

    public List<GameObject> enemies = new List<GameObject>(); //list cause on array when the game 
    //is running you can't easy adjust the size of array

    public Room theRoom;

    GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        if(openWhenEnemiesCleared)
        {
            theRoom.closeWhenEntered = true;

            int randomEnemyNumber = Random.Range(1, potentialSpawnPoints.Length);

            for(int i = 0; i < randomEnemyNumber;i++)
            {
                int randomEnemy = Random.Range(0, EnemiesList.instance.allEnemies.Length);

                

                //indicator.target
                GameObject g = Instantiate(EnemiesList.instance.allEnemies[randomEnemy],potentialSpawnPoints[i].position,Quaternion.identity);

                GameObject enemyIndicator = Instantiate(EnemiesList.instance.indicator, player.transform.position, player.transform.rotation);

                enemyIndicator.GetComponent<IndicatorScript>().target = g.transform;
                enemyIndicator.GetComponent<IndicatorScript>().theRoom = theRoom;

                enemyIndicator.transform.SetParent(player.transform);

                switch (EnemiesList.instance.level)
                {
                    case 1:
                        switch (randomEnemy)
                        {
                            case 0:
                                g.GetComponent<SmallGreenSlime>().theRoom = theRoom;
                                break;

                            case 1:
                                g.GetComponent<SmallPurpleSlime>().theRoom = theRoom;
                                break;

                            case 2:
                                g.GetComponent<SmallRedSlime>().theRoom = theRoom;
                                break;
                            case 3:
                                g.GetComponent<BigRedSlime>().theRoom = theRoom;
                                break;
                        }
                        break;
                    case 2:
                        switch (randomEnemy)
                        {
                            case 0:
                                g.GetComponent<TreeEnemy>().theRoom = theRoom;
                                break;

                            case 1:
                                g.GetComponent<GreenTreeMonster>().theRoom = theRoom;
                                break;

                            case 2:
                                g.GetComponent<RedBlueTreeMonster>().theRoom = theRoom;
                                break;

                        }
                        break;
                    case 3:
                        switch (randomEnemy)
                        {
                            case 0:
                                g.GetComponent<GoblinController>().theRoom = theRoom;
                                break;

                            case 1:
                                g.GetComponent<BlueGoblin>().theRoom = theRoom;
                                break;

                            case 2:
                                g.GetComponent<RedGoblin>().theRoom = theRoom;
                                break;

                        }

                        break;
                }
                
                enemies.Add(g);
            }
        }
    }
    
    void Update()
    {
        if (enemies.Count > 0 && theRoom.roomActive && openWhenEnemiesCleared)//list.Count == array.Length
        {
            for (int i = 0; i < enemies.Count; i++) //for a nie foreach bo chcemy wiedziec dokladnie ktory zniszczony
            {
                if (enemies[i] == null)  //jezeli przeciwnik zniszczony
                {
                    Debug.Log("Delete");

                    enemies.RemoveAt(i); //usuwa z listy

                    i--;
                    //dekrementacja bo zalozmy ze mamy 3 elementy ,element 1 jest pusty i usuwamy go ,wtedy
                    //dlugosc listy zmienjszy sie do 2 , wiec element o indeksie 2 staje sie 
                    //elementem o indeksie 1 ALE JUZ MIELIMSY ITERACJE Z INDEKSEM 1 WIEC SIE MUSZE COFNAC
                }
            }

            if (enemies.Count <= 0) //jezeli nie ma przeciwnikow na liscie
            {
                theRoom.OpenDoors();
            }
        }
    }
    
}
