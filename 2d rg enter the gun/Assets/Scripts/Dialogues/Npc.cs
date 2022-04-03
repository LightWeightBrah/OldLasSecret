using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public Dialogue dialogue;

    public bool canInteract;

    public Sprite defaultSprite;
    public Sprite interactableSprite;

    public GameObject dialogueBox;

    SpriteRenderer sr;

    public DialogueManager man;

    GameObject player;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        
        dialogueBox.SetActive(false);

        player = GameObject.FindGameObjectWithTag("Player");

    }

    private void Update()
    {
        if (canInteract)
        {
            if(player.transform.position.x > transform.position.x)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = true;

            }


            if (Input.GetKeyDown(KeyCode.E) && man.isDialogueRunning)
            {
                
                NextDialogue();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                dialogueBox.SetActive(true);
                TriggerDialogue();

            }


        }
    }

    public void TriggerDialogue()
    {
        man.StartDialogue(dialogue);
    }

    public void NextDialogue()
    {
        man.DisplayNextSentence();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            sr.sprite = interactableSprite;
            canInteract = true;

            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            sr.sprite = defaultSprite;
            canInteract = false;

            man.sentences.Clear();
            man.EndDialogue();
           // dialogueBox.SetActive(false);


        }

    }
}
